using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.AydinlatmaMetniCQRS.Queries;
using Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries;
using Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.KdsHasarTespitVeriByUid;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Hubs;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Addons.StringAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS.Commands.EkleSikcaSorulanSoru;

public class EkleSikcaSorulanSoruCommand : IRequest<ResultModel<EkleSikcaSorulanSoruCommandResponseModel>>
{
	//public EkleSikcaSorulanSoruCommandModel Model { get; set; }

	public string? Soru { get; set; }
	public string? Cevap { get; set; }
	public int? SiraNo { get; set; }
	public bool? AktifMi { get; set; }

	public class EkleSikcaSorulanSoruCommandHandler : IRequestHandler<EkleSikcaSorulanSoruCommand, ResultModel<EkleSikcaSorulanSoruCommandResponseModel>>
	{
		private readonly IMapper _mapper;
		private readonly ISikcaSorulanSoruRepository _sssRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly ICacheService _cacheService;
		private readonly IKullaniciBilgi _kullaniciBilgi;

		public EkleSikcaSorulanSoruCommandHandler(IMapper mapper
			, ISikcaSorulanSoruRepository sssRepository
			, ICacheService cacheService
			, IWebHostEnvironment webHostEnvironment
			, IKullaniciBilgi kullaniciBilgi
		)
		{
			_cacheService = cacheService;
			_webHostEnvironment = webHostEnvironment;
			_mapper = mapper;
			_sssRepository = sssRepository;
			_kullaniciBilgi = kullaniciBilgi;
		}

		public async Task<ResultModel<EkleSikcaSorulanSoruCommandResponseModel>> Handle(EkleSikcaSorulanSoruCommand request, CancellationToken cancellationToken)
		{
			var result = new ResultModel<EkleSikcaSorulanSoruCommandResponseModel>();

			try
			{
				// input bilgilerinin kontrol edildigi method
				var soruMevcutMu = _sssRepository.GetWhereEnumerable(x => x.SilindiMi == false && StringAddons.NormalizeText(x.Soru).Equals(StringAddons.NormalizeText(request.Soru))).Any();
				if (soruMevcutMu)
				{
					result.ErrorMessage("Bu soru zaten mevcut.");
				}

				// kontroller sirasında bir hata ile karsilasiliyorsa program akisini keserek geriye donuyoruz.
				if (result.IsError)
					return await Task.FromResult(result);

				long.TryParse(_kullaniciBilgi.GetUserInfo()?.KullaniciId, out long kullaniciId);

				var SikcaSorulanSoru = new SikcaSorulanSoru()
				{
					Soru = request.Soru,
					Cevap = request.Cevap,
					AktifMi = request.AktifMi,
					SilindiMi = false,
					SiraNo = request.SiraNo,
					OlusturanKullaniciId = kullaniciId,
					OlusturmaTarihi = DateTime.Now,
					OlusturanIp = _kullaniciBilgi.GetUserInfo()?.IpAdresi,
				};

				//_mapper.Map<SikcaSorulanSoru>(request);


				await _sssRepository.AddAsync(SikcaSorulanSoru);

				await _sssRepository.SaveChanges(cancellationToken);

				//await _hubContext.Clients.All.SendAsync("SikcaSorulanSoruSayisi", new { SikcaSorulanSoruSayisi = _appealRepository.Count(p => p.SikcaSorulanSoruDurumId == SikcaSorulanSoruDurumEnum.SikcaSorulanSorunuzAlinmistir && p.AktifMi == true && !p.SilindiMi) });

				var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirSikcaSorulanSoruListeQuery)}";
				await _cacheService.Clear(cacheKey);

				result.Result = new EkleSikcaSorulanSoruCommandResponseModel
				{
					//SikcaSorulanSoruId = SikcaSorulanSoru.SikcaSorulanSoruGuid,
					Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.",
				};
			}
			catch (ArgumentNullException ex)
			{
				result.Exception(ex, "Bir Hata Meydana Geldi. Hatalı veya Geçersiz (Boş) Bilgi Gönderildi. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
			}
			catch (ArgumentException ex)
			{
				result.Exception(ex, "Bir Hata Meydana Geldi. Hatalı veya Geçersiz Bilgi Gönderildi. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
			}
			catch (Exception ex)
			{
				if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
				{
					result.Exception(ex, "Bir Hata Meydana Geldi. Hatalı veya Geçersiz İlişkili Bilgi Gönderildi. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
				}
				else
				{
					result.Exception(ex, "Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz.");
				}
			}

			return await Task.FromResult(result);
		}
	}
}