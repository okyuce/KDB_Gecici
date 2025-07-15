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
    public class SilSikcaSorulanSoruCommand : IRequest<ResultModel<SilSikcaSorulanSoruCommandResponseModel>>
    {
		//public SilSikcaSorulanSoruCommandModel Model { get; set; }

		public long? SikcaSorulanSoruId { get; set; }

		public class SilSikcaSorulanSoruCommandHandler : IRequestHandler<SilSikcaSorulanSoruCommand, ResultModel<SilSikcaSorulanSoruCommandResponseModel>>
        {
            private readonly ISikcaSorulanSoruRepository _sssRepository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly ICacheService _cacheService;
            private readonly IKullaniciBilgi _kullaniciBilgi;

            public SilSikcaSorulanSoruCommandHandler(ISikcaSorulanSoruRepository sssRepository,
                ICacheService cacheService,
                IWebHostEnvironment webHostEnvironment,
                IKullaniciBilgi kullaniciBilgi
            ) {
                _webHostEnvironment = webHostEnvironment;
                _cacheService = cacheService;
                _sssRepository = sssRepository;
                _kullaniciBilgi = kullaniciBilgi;
            }

            public async Task<ResultModel<SilSikcaSorulanSoruCommandResponseModel>> Handle(SilSikcaSorulanSoruCommand request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<SilSikcaSorulanSoruCommandResponseModel>();

                try
                {

                    // kontroller sirasında bir hata ile karsilasiliyorsa program akisini keserek geriye donuyoruz.
                    if (result.IsError)
                        return await Task.FromResult(result);

                    var sikcaSorulanSoru = await _sssRepository.GetWhere(x => x.SikcaSorulanSoruId == request.SikcaSorulanSoruId).FirstOrDefaultAsync();
                    if(sikcaSorulanSoru == null)
                    {
                        result.ErrorMessage("Soru veritabanında bulunamadı.");
						return await Task.FromResult(result);
					}

                    long.TryParse(_kullaniciBilgi.GetUserInfo()?.KullaniciId, out long kullaniciId);

                    sikcaSorulanSoru.SilindiMi = true;
                    sikcaSorulanSoru.GuncelleyenKullaniciId = kullaniciId;
                    sikcaSorulanSoru.GuncellemeTarihi = DateTime.Now;
                    sikcaSorulanSoru.GuncelleyenIp = _kullaniciBilgi.GetUserInfo()?.IpAdresi;

                    await _sssRepository.SaveChanges(cancellationToken);

                    //await _hubContext.Clients.All.SendAsync("SikcaSorulanSoruSayisi", new { SikcaSorulanSoruSayisi = _appealRepository.Count(p => p.SikcaSorulanSoruDurumId == SikcaSorulanSoruDurumEnum.SikcaSorulanSorunuzAlinmistir && p.AktifMi == true && !p.SilindiMi) });

                    var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirSikcaSorulanSoruListeQuery)}";
                    await _cacheService.Clear(cacheKey);

                    result.Result = new SilSikcaSorulanSoruCommandResponseModel
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