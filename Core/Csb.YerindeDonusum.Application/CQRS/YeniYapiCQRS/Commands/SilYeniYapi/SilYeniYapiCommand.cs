using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiServerSideGroupped;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands
{
    public class SilYeniYapiCommand : IRequest<ResultModel<SilYeniYapiCommandResponseModel>>
    {
        //public SilYeniYapiCommandModel Model { get; set; }

        public long? BinaDegerlendirmeId { get; set; }
        public string? Aciklama { get; set; }
        public DosyaDto? RuhsatliYapiSilDosya { get; set; }

        public class SilYeniYapiCommandHandler : IRequestHandler<SilYeniYapiCommand, ResultModel<SilYeniYapiCommandResponseModel>>
        {
            private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly IKullaniciBilgi _kullaniciBilgi;
            private readonly ICacheService _cacheService;
            private readonly IConfiguration _configuration;

            public SilYeniYapiCommandHandler(IBinaDegerlendirmeRepository binaDegerlendirmeRepository,
                    ICacheService cacheService,
                    IWebHostEnvironment webHostEnvironment,
                    IKullaniciBilgi kullaniciBilgi,
                    IConfiguration configuration
            )
            {
                _webHostEnvironment = webHostEnvironment;
                _cacheService = cacheService;
                _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
                _kullaniciBilgi = kullaniciBilgi;
                _configuration = configuration;
            }

            public async Task<ResultModel<SilYeniYapiCommandResponseModel>> Handle(SilYeniYapiCommand request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<SilYeniYapiCommandResponseModel>();

                var binaDegerlendirme = await _binaDegerlendirmeRepository.GetAllQueryable(x => !x.SilindiMi
                                                    && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId)
                                                .Include(i => i.Basvurus).ThenInclude(x => x.BasvuruImzaVerens)
                                                .Include(i => i.BasvuruKamuUstleneceks)
                                                .Include(i => i.BinaMuteahhits)
                                                .Include(i => i.BinaDegerlendirmeDosyas)
                                                .Include(i => i.BinaYapiRuhsatIzinDosyas)
                                                .Include(i => i.BinaYapiDenetimSeviyeTespits)
                                                .Include(i => i.BinaOdemes).ThenInclude(x => x.BinaOdemeDurum)
                                                .FirstOrDefaultAsync();

                if (binaDegerlendirme == null)
                {
                    result.ErrorMessage("Bina Değerlendirme bilgisi bulunamadı.");
                    return await Task.FromResult(result);
                }
                if (!_kullaniciBilgi.IsInRole("Yeni Yapı Silme", "Admin"))
                {
                    if (binaDegerlendirme.AktifMi == true && binaDegerlendirme.SilindiMi == false && (binaDegerlendirme.BultenNo != null || binaDegerlendirme.IzinBelgesiTarih != null))
                    {
                        result.ErrorMessage($"Bina Değerlendirme Durumu : {BinaDegerlendirmeDurumEnum.YapiRuhsatinizOnaylanmistir.GetDisplayName()} Olduğu İçin Yeni Yapı Bilgisi Silinemez.");
                        return await Task.FromResult(result);
                    }
                    if (binaDegerlendirme.BinaOdemes.Any(x => x.BinaOdemeDurumId == (long)BinaOdemeDurumEnum.HYSAktarildi
                                                            || x.BinaOdemeDurumId == (long)BinaOdemeDurumEnum.MuteahhiteAktarildi))
                    {
                        result.ErrorMessage($"Bina Değerlendirme Ödeme Talebi Durumu : {string.Join(",", binaDegerlendirme.BinaOdemes
                                                    .Where(x => x.BinaOdemeDurumId == (long)BinaOdemeDurumEnum.HYSAktarildi
                                                            || x.BinaOdemeDurumId == (long)BinaOdemeDurumEnum.MuteahhiteAktarildi).Select(c => c.BinaOdemeDurum.Ad))} Olduğu İçin Yeni Yapı Bilgisi Silinemez.");
                        return await Task.FromResult(result);
                    }
                }

                long.TryParse(_kullaniciBilgi.GetUserInfo().KullaniciId, out long kullaniciId);
                var ipAdresi = _kullaniciBilgi.GetUserInfo().IpAdresi;

                binaDegerlendirme.BinaOdemes.ToList().ForEach(x =>
                {
                    x.SilindiMi = true;
                    x.AktifMi = false;
                    x.GuncellemeTarihi = DateTime.Now;
                    x.GuncelleyenKullaniciId = kullaniciId;
                    x.GuncelleyenIp = ipAdresi;
                });
                binaDegerlendirme.BinaYapiRuhsatIzinDosyas.ToList().ForEach(x =>
                {
                    x.SilindiMi = true;
                    x.AktifMi = false;
                });
                binaDegerlendirme.BinaDegerlendirmeDosyas.Where(x => x.BinaDegerlendirmeDosyaTurId != (long)BinaDegerlendirmeDosyaTurEnum.YapiSilme).ToList().ForEach(x =>
                {
                    x.SilindiMi = true;
                    x.AktifMi = false;
                });
                binaDegerlendirme.BinaYapiDenetimSeviyeTespits.ToList().ForEach(x =>
                {
                    x.SilindiMi = true;
                    x.AktifMi = false;
                    x.GuncellemeTarihi = DateTime.Now;
                    x.GuncelleyenKullaniciId = kullaniciId;
                    x.GuncelleyenIp = ipAdresi;
                });
                binaDegerlendirme.BinaMuteahhits.ToList().ForEach(x =>
                {
                    x.SilindiMi = true;
                    x.AktifMi = false;
                    x.GuncellemeTarihi = DateTime.Now;
                    x.GuncelleyenKullaniciId = kullaniciId;
                    x.GuncelleyenIp = ipAdresi;
                });
                binaDegerlendirme.Basvurus.ToList().ForEach(x =>
                {
                    x.BinaDegerlendirmeId = null;
                    x.TapuAda = x.EskiTapuAda ?? x.TapuAda;
                    x.TapuParsel = x.EskiTapuParsel ?? x.TapuParsel;
                    x.TapuIlceAdi = x.EskiTapuIlceAdi ?? x.TapuIlceAdi;
                    x.TapuIlceId = x.EskiTapuIlceId ?? x.TapuIlceId;
                    x.TapuMahalleAdi = x.EskiTapuMahalleAdi ?? x.TapuMahalleAdi;
                    x.TapuMahalleId = x.EskiTapuMahalleId ?? x.TapuMahalleId;
                    x.EskiTapuAda = null;
                    x.EskiTapuGuncellemeTarihi = null;
                    x.EskiTapuGuncelleyenKullaniciId = null;
                    x.EskiTapuIlceAdi = null;
                    x.EskiTapuIlceId = null;
                    x.EskiTapuMahalleAdi = null;
                    x.EskiTapuMahalleId = null;
                    x.EskiTapuParsel = null;
                    x.GuncellemeTarihi = DateTime.Now;
                    x.GuncelleyenKullaniciId = kullaniciId;
                    x.GuncelleyenIp = ipAdresi;

                });
                binaDegerlendirme.Basvurus.SelectMany(x => x.BasvuruImzaVerens).ToList().ForEach(x =>
                {
                    x.SilindiMi = true;
                    x.AktifMi = false;
                    x.GuncellemeTarihi = DateTime.Now;
                    x.GuncelleyenKullaniciId = kullaniciId;
                    x.GuncelleyenIp = ipAdresi;
                });
                binaDegerlendirme.BasvuruKamuUstleneceks.ToList().ForEach(x =>
                {
                    x.BinaDegerlendirmeId = null;
                    x.TapuAda = x.EskiTapuAda ?? x.TapuAda;
                    x.TapuParsel = x.EskiTapuParsel ?? x.TapuParsel;
                    x.TapuIlceAdi = x.EskiTapuIlceAdi ?? x.TapuIlceAdi;
                    x.TapuIlceId = x.EskiTapuIlceId ?? x.TapuIlceId;
                    x.TapuMahalleAdi = x.EskiTapuMahalleAdi ?? x.TapuMahalleAdi;
                    x.TapuMahalleId = x.EskiTapuMahalleId ?? x.TapuMahalleId;
                    x.EskiTapuAda = null;
                    x.EskiTapuGuncellemeTarihi = null;
                    x.EskiTapuGuncelleyenKullaniciId = null;
                    x.EskiTapuIlceAdi = null;
                    x.EskiTapuIlceId = null;
                    x.EskiTapuMahalleAdi = null;
                    x.EskiTapuMahalleId = null;
                    x.EskiTapuParsel = null;
                    x.GuncellemeTarihi = DateTime.Now;
                    x.GuncelleyenKullaniciId = kullaniciId;
                    x.GuncelleyenIp = ipAdresi;
                });
                binaDegerlendirme.GuncelleyenKullaniciId = kullaniciId;
                binaDegerlendirme.GuncelleyenIp = ipAdresi;
                binaDegerlendirme.GuncellemeTarihi = DateTime.Now;
                binaDegerlendirme.SilindiMi = true;
                binaDegerlendirme.AktifMi = false;

                var binaDegerlendirmeDosya = new BinaDegerlendirmeDosya
                {
                    AktifMi = true,
                    SilindiMi = false,
                };

                if (request.RuhsatliYapiSilDosya != null)
                {
                    binaDegerlendirmeDosya.DosyaAdi = string.Concat(Guid.NewGuid(), request.RuhsatliYapiSilDosya?.DosyaUzanti);
                    binaDegerlendirmeDosya.DosyaYolu = DateTime.Now.ToString("yyyy-MM-dd");
                    binaDegerlendirmeDosya.DosyaTuru = MimeTypes.GetMimeType(request.RuhsatliYapiSilDosya?.DosyaUzanti ?? "");
                    binaDegerlendirmeDosya.BinaDegerlendirmeDosyaTurId = (long)BinaDegerlendirmeDosyaTurEnum.YapiSilme;
                    binaDegerlendirmeDosya.YeniYapiSilAciklama = request.Aciklama;

                    byte[] data = Convert.FromBase64String(request?.RuhsatliYapiSilDosya?.DosyaBase64);

                    if (!FileTypeVerifier.Verify(data).IsVerified)
                    {
                        result.ErrorMessage("Geçersiz veya Hatalı Dosya Uzantısı.");
                        return await Task.FromResult(result);
                    }

                    string uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", binaDegerlendirmeDosya.DosyaYolu);

                    if (!Directory.Exists(uploadDirectoryPath))
                        Directory.CreateDirectory(uploadDirectoryPath);

                    string filePath = string.Concat(uploadDirectoryPath, "\\", binaDegerlendirmeDosya.DosyaAdi);
                    using FileStream stream = File.Create(filePath);
                    stream.Write(data, 0, data.Length);

                    binaDegerlendirme.BinaDegerlendirmeDosyas.Add(binaDegerlendirmeDosya);            
                }

                _binaDegerlendirmeRepository.Update(binaDegerlendirme);
                await _binaDegerlendirmeRepository.SaveChanges();

                var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirListeYeniYapiServerSideGrouppedQuery)}";
                await _cacheService.Clear(cacheKey);

                result.Result = new SilYeniYapiCommandResponseModel
                {
                    Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.",
                };

                return await Task.FromResult(result);
            }
        }
    }
}