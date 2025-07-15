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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS.Commands
{
    public class GuncelleSikcaSorulanSoruCommand : IRequest<ResultModel<GuncelleSikcaSorulanSoruCommandResponseModel>>
    {
		//public GuncelleSikcaSorulanSoruCommandModel Model { get; set; }

		public long SikcaSorulanSoruId { get; set; }

		public string? Soru { get; set; }

		public string? Cevap { get; set; }

		public int? SiraNo { get; set; }

		public bool? AktifMi { get; set; }

		public class GuncelleSikcaSorulanSoruCommandHandler : IRequestHandler<GuncelleSikcaSorulanSoruCommand, ResultModel<GuncelleSikcaSorulanSoruCommandResponseModel>>
        {
            private readonly IMapper _mapper;
            private readonly ISikcaSorulanSoruRepository _sssRepository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly ICacheService _cacheService;
            private readonly IKullaniciBilgi _kullaniciBilgi;

            public GuncelleSikcaSorulanSoruCommandHandler(IMapper mapper,
                    ISikcaSorulanSoruRepository sssRepository,
                    IWebHostEnvironment webHostEnvironment,
                    ICacheService cacheService,
                    IKullaniciBilgi kullaniciBilgi
            ) {
                _cacheService = cacheService;
                _webHostEnvironment = webHostEnvironment;
                _mapper = mapper;
				_sssRepository = sssRepository;
                _kullaniciBilgi = kullaniciBilgi;
            }

            public async Task<ResultModel<GuncelleSikcaSorulanSoruCommandResponseModel>> Handle(GuncelleSikcaSorulanSoruCommand request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<GuncelleSikcaSorulanSoruCommandResponseModel>();

                try
                {

					var soruMevcutMu = _sssRepository.GetWhereEnumerable(x => x.SilindiMi == false && x.SikcaSorulanSoruId != request.SikcaSorulanSoruId && StringAddons.NormalizeText(x.Soru).Equals(StringAddons.NormalizeText(request.Soru))).Any();
					if (soruMevcutMu)
					{
						result.ErrorMessage("Bu soru zaten mevcut.");
						return await Task.FromResult(result);
					}			

                    var sikcaSorulanSoru = await _sssRepository.GetWhere(x => x.SikcaSorulanSoruId == request.SikcaSorulanSoruId).FirstOrDefaultAsync();
                    if(sikcaSorulanSoru == null)
                    {
                        result.ErrorMessage("Soru veritabanında bulunamadı.");
						return await Task.FromResult(result);
					}

                    long.TryParse(_kullaniciBilgi.GetUserInfo()?.KullaniciId, out long kullaniciId);

                    sikcaSorulanSoru.Soru = request.Soru ?? sikcaSorulanSoru.Soru;
                    sikcaSorulanSoru.Cevap = request.Cevap ?? sikcaSorulanSoru.Cevap;       
                    sikcaSorulanSoru.AktifMi = request.AktifMi ?? sikcaSorulanSoru.AktifMi;
                    sikcaSorulanSoru.GuncelleyenKullaniciId = kullaniciId;
                    sikcaSorulanSoru.GuncellemeTarihi = DateTime.Now;
                    sikcaSorulanSoru.GuncelleyenIp = _kullaniciBilgi.GetUserInfo()?.IpAdresi;
                    sikcaSorulanSoru.SiraNo = request.SiraNo ?? sikcaSorulanSoru.SiraNo;
                    
                    // 12
                    // 10 - 2
                    // 10 <= +2
                    //int yeniSira = request.SiraNo ?? sikcaSorulanSoru.SiraNo ?? 1;
                    //_sssRepository.GetWhere(x=> x.SiraNo >= request.SiraNo).ForEachAsync(x =>
                    //{
                    //    x.SiraNo = yeniSira;
                    //    yeniSira++;
                    //});

                    await _sssRepository.SaveChanges(cancellationToken);

                    //await _hubContext.Clients.All.SendAsync("SikcaSorulanSoruSayisi", new { SikcaSorulanSoruSayisi = _appealRepository.Count(p => p.SikcaSorulanSoruDurumId == SikcaSorulanSoruDurumEnum.SikcaSorulanSorunuzAlinmistir && p.AktifMi == true && !p.SilindiMi) });

                    var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirSikcaSorulanSoruListeQuery)}";
                    await _cacheService.Clear(cacheKey);

                    result.Result = new GuncelleSikcaSorulanSoruCommandResponseModel
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
}