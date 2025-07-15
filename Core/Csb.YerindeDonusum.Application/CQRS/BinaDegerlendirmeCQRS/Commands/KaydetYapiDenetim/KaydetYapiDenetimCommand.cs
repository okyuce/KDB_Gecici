using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Commands.BinaOdemeEkle;
using Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiAdSoyadTcDen;
using Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiBilgileriTcDen;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetYapiDenetim;

public class KaydetYapiDenetimCommand : IRequest<ResultModel<string>>
{
    public long? BinaDegerlendirmeId { get; set; }
    public int? YapiDenetimIlerlemeYuzdesi { get; set; }
    public long? YIBFNoSeviye { get; set; }
    public int? YIBFNo { get; set; }
    public string? FenniMesulTc { get; set; }
    public DosyaDto? BelgeYapiDenetimDosya { get; set; }
    public bool? FenniMesulSeciliMi { get; set; }

    public class KaydetYapiDenetimCommandHandler : IRequestHandler<KaydetYapiDenetimCommand, ResultModel<string>>
    {
        private readonly IMediator _mediator;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IAfadBasvuruTekilRepository _afadBasvuruTekilRepository;
        private readonly IConfiguration _configuration;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public KaydetYapiDenetimCommandHandler(IBinaDegerlendirmeRepository binaDegerlendirmeRepository
            , IKullaniciBilgi kullaniciBilgi
            , IConfiguration configuration
            , IMediator mediator
            , IAfadBasvuruTekilRepository afadBasvuruTekilRepository)
        {
            _configuration = configuration;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _kullaniciBilgi = kullaniciBilgi;
            _mediator = mediator;
            _afadBasvuruTekilRepository = afadBasvuruTekilRepository;
        }

        public async Task<ResultModel<string>> Handle(KaydetYapiDenetimCommand request, CancellationToken cancellationToken)
        {
            var kullanici = _kullaniciBilgi.GetUserInfo();
            long.TryParse(kullanici?.KullaniciId, out long kullaniciId);
            string? ipAdresi = kullanici?.IpAdresi;

            var result = new ResultModel<string>();

            #region Bina Değerlendirme

              var binaDegerlendirme = await _binaDegerlendirmeRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                   && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId
                                                   , asNoTracking: false
                           ).Include(x => x.Basvurus.Where(y => y.SilindiMi == false && y.AktifMi == true
                                                        && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                               && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                               && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruReddedilmistir)
                                                   )
                                               .ThenInclude(x => x.BasvuruImzaVerens.Where(y => y.SilindiMi == false && y.AktifMi == true))
                           .Include(i => i.BinaYapiRuhsatIzinDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true))
                           .Include(i => i.BinaMuteahhits.Where(y => y.SilindiMi == false && y.AktifMi == true && (y.VergiKimlikNo!=null || y.YetkiBelgeNo!=null)))
                           .Include(i => i.BinaDegerlendirmeDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true))
                           .Include(i => i.BinaYapiDenetimSeviyeTespits.Where(x => x.SilindiMi == false && x.AktifMi == true))
                               .ThenInclude(i => i.BinaYapiDenetimSeviyeTespitDosyas)
                           .FirstOrDefaultAsync();           

            if (binaDegerlendirme == null)
            {
                result.ErrorMessage("Bina Değerlendirme bilgisi bulunamadı.");
                return result;
            }
            if (binaDegerlendirme.Basvurus?.Any(x => x.BasvuruAfadDurumId == (long)BasvuruAfadDurumEnum.Kabul) == true)
            {
                result.ErrorMessage("AFAD durumu Kabul olan başvurular olduğundan yapı denetim işlemine devam edilemiyor!");
                return await Task.FromResult(result);
            }

            if (binaDegerlendirme.Basvurus?.Any(x => x.BasvuruImzaVerens == null || x.BasvuruImzaVerens.Count == 0) == true)
            {
                result.ErrorMessage($"Başvurusu bulunan ancak imza veren detayı olmayan kayıtlar var! İşleme devam edilemiyor.");
                return await Task.FromResult(result);
            }
            //if (DateTime.Today.Day > 15 && binaDegerlendirme.YibfNo == null)
            //{
            //    //yibf no dolu ise 1 ile 15 arasında işlem yapılma şartı yok
            //    result.ErrorMessage("Bu işlem ayın 1. gününden 15. gününe kadar yapılabilir.");
            //    return result;
            //}

            if (binaDegerlendirme.BinaDegerlendirmeDosyas?.Any() != true)
            {
                result.ErrorMessage("Lütfen önce 'Belge Bilgileri' sekmesini doldurunuz.");
                return await Task.FromResult(result);
            }
            if (binaDegerlendirme.BinaMuteahhits?.Any(x => x.SilindiMi == false && x.AktifMi == true) != true)
            {
                result.ErrorMessage("Lütfen önce 'Müteahhit Bilgileri' sekmesini doldurunuz.");
                return await Task.FromResult(result);
            }
            if (!(binaDegerlendirme.IzinBelgesiSayi > 0) && !(binaDegerlendirme.BultenNo > 0))
            {
                result.ErrorMessage("Lütfen önce 'Yapı Ruhsat Bilgileri' sekmesini doldurunuz.");
                return await Task.FromResult(result);
            }

            // Bütün Malikler İmza Vermemişse İşleme Devam Edilemez.
            var butunMaliklerImzaVerdiMiResult = await DomainServices.ButunMaliklerImzaVerdiMi(_mediator, binaDegerlendirme);
            if (butunMaliklerImzaVerdiMiResult.IsError)
            {
                result.ErrorMessage(butunMaliklerImzaVerdiMiResult.ErrorMessageContent);
                return result;
            }
           
            if (request.FenniMesulSeciliMi == true)
            {   
                binaDegerlendirme.FenniMesulTc = request.FenniMesulTc;
                binaDegerlendirme.YibfNo = null;

                var kpsKisiBilgileri = await _mediator.Send(new GetirKisiAdSoyadTcDenQuery { TcKimlikNo = long.Parse(request.FenniMesulTc), MaskelemeKapaliMi = true });
                if (kpsKisiBilgileri.IsError)
                {
                    result.ErrorMessage(kpsKisiBilgileri.ErrorMessageContent);
                    return await Task.FromResult(result);
                }
                else if (kpsKisiBilgileri.Result.OlumTarih != null)
                {
                    result.ErrorMessage("Sorgulanan Kişi Vefat Ettiği İçin Başvuruya Devam Edemezsiniz.");
                    return await Task.FromResult(result);
                }
            }
            else
            {
                binaDegerlendirme.YibfNo = request.YIBFNo;
                binaDegerlendirme.FenniMesulTc = null;
            }
            #endregion

            #region Yapı Denetim Seviye Tespit
            var binaDegerlendirmeSeviyeTespitVarMi = binaDegerlendirme.BinaYapiDenetimSeviyeTespits.Any(x => x.IlerlemeYuzdesi == request.YapiDenetimIlerlemeYuzdesi);
            if (binaDegerlendirmeSeviyeTespitVarMi)
            {
                result.ErrorMessage("Aynı ilerleme yüzdesine ait kayıt olduğu için işleme devam edilemiyor!");
                return await Task.FromResult(result);
            }

            // todo: mehmet.sumer: gerekli yapi denetim seviyelerine bakilip yapilacak.
            //var ekliOlmasiGerekenYuzdeler = AyarConstants.YapiDenetimSeviyeleri.Where(x => x < request.YapiDenetimIlerlemeYuzdesi);
            //var gerekliSeviyelerEkliMi = ekliOlmasiGerekenYuzdeler.All(x => binaDegerlendirme.BinaYapiDenetimSeviyeTespits.Any(o => o.IlerlemeYuzdesi == x));
            //if (!gerekliSeviyelerEkliMi)
            //{
            //    result.ErrorMessage("Lütfen önceki seviye için ödeme talebini oluşturunuz.");
            //    return await Task.FromResult(result);
            //}

            if (request.YapiDenetimIlerlemeYuzdesi == 100)
            {
                var oncekiSeviyeMevcutMu = binaDegerlendirme.BinaYapiDenetimSeviyeTespits.Any(x => x.IlerlemeYuzdesi == 60);
                if (!oncekiSeviyeMevcutMu)
                {
                    result.ErrorMessage("Lütfen önce %60 için ödeme talebini oluşturunuz.");
                    return await Task.FromResult(result);
                }
            }
            else if (request.YapiDenetimIlerlemeYuzdesi == 60)
            {
                var oncekiSeviyeMevcutMu = binaDegerlendirme.BinaYapiDenetimSeviyeTespits.Any(x => x.IlerlemeYuzdesi == 20);
                if (!oncekiSeviyeMevcutMu)
                {
                    result.ErrorMessage("Lütfen önce %20 için ödeme talebini oluşturunuz.");
                    return await Task.FromResult(result);
                }
            }

            var binaDegerlendirmeSeviyeTespit = new BinaYapiDenetimSeviyeTespit()
            {
                IlerlemeYuzdesi = request.YapiDenetimIlerlemeYuzdesi.Value,
                GuncellemeTarihi = DateTime.Now,
                GuncelleyenKullaniciId = kullaniciId,
                AktifMi = true,
                SilindiMi = false,
                OlusturmaTarihi = DateTime.Now,
                OlusturanIp = ipAdresi,
                OlusturanKullaniciId = kullaniciId
            };

            binaDegerlendirme.BinaYapiDenetimSeviyeTespits.Add(binaDegerlendirmeSeviyeTespit);
            #endregion

            #region Yapı Denetim Seviye Tespit Dosya
            if (!string.IsNullOrWhiteSpace(request?.BelgeYapiDenetimDosya?.DosyaBase64))
            {
                if (binaDegerlendirmeSeviyeTespit.BinaYapiDenetimSeviyeTespitDosyas.Any() == false)
                {
                    binaDegerlendirmeSeviyeTespit.BinaYapiDenetimSeviyeTespitDosyas.Add(new BinaYapiDenetimSeviyeTespitDosya()
                    {
                        AktifMi = true,
                        SilindiMi = false,
                    });
                }

                var eklenecekBelge = binaDegerlendirmeSeviyeTespit.BinaYapiDenetimSeviyeTespitDosyas.FirstOrDefault();
                eklenecekBelge!.DosyaAdi = string.Concat(Guid.NewGuid(), request.BelgeYapiDenetimDosya.DosyaUzanti);
                eklenecekBelge.DosyaYolu = DateTime.Now.ToString("yyyy-MM-dd");
                eklenecekBelge.DosyaTuru = MimeTypes.GetMimeType(request.BelgeYapiDenetimDosya.DosyaUzanti ?? "");

                byte[] data = Convert.FromBase64String(request.BelgeYapiDenetimDosya.DosyaBase64);

                var isTheFileTypeAllowed = FileTypeVerifier.Verify(data);

                if (!isTheFileTypeAllowed.IsVerified)
                {
                    result.ErrorMessage("Geçersiz veya Hatalı Dosya Uzantısı.");
                    return await Task.FromResult(result);
                }

                var uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", eklenecekBelge.DosyaYolu);

                if (!Directory.Exists(uploadDirectoryPath))
                    Directory.CreateDirectory(uploadDirectoryPath);

                var filePath = string.Concat(uploadDirectoryPath, "\\", eklenecekBelge.DosyaAdi);

                using var stream = File.Create(filePath);

                stream.Write(data, 0, data.Length);
            }
            #endregion


            #region Ödeme Listesinde %10 Peşinat yok ise otomatik olarak Ekleniyor
            // %10 seviye yok ise ekleyeceğiz ve durumu otomatik olarak Bekliyor olacak.
            var binaOdemeEkleResult = await _mediator.Send(new BinaOdemeEkleCommand()
            {
                BinaDegerlendirmeId = binaDegerlendirme.BinaDegerlendirmeId,
                Seviye = request.YapiDenetimIlerlemeYuzdesi.Value
            });

            if (!string.IsNullOrEmpty(binaOdemeEkleResult.ErrorMessageContent))
            {
                result.ErrorMessage(binaOdemeEkleResult.ErrorMessageContent);
                return await Task.FromResult(result);
            }
            #endregion

            #region Bina Değerlendirme Durumu Set Ediliyor

            if (request.YapiDenetimIlerlemeYuzdesi == 100)
            {
                binaDegerlendirme.BinaDegerlendirmeDurumId = (long)BinaDegerlendirmeDurumEnum.YapiTamamlanmistir;
            }
            else if (request.YapiDenetimIlerlemeYuzdesi == 60)
            {
                binaDegerlendirme.BinaDegerlendirmeDurumId = (long)BinaDegerlendirmeDurumEnum.YapiIlerlemeSeviyesiYuzde60;
            }
            else if (request.YapiDenetimIlerlemeYuzdesi == 20)
            {
                binaDegerlendirme.BinaDegerlendirmeDurumId = (long)BinaDegerlendirmeDurumEnum.YapiIlerlemeSeviyesiYuzde20;
            }
            #endregion

            _binaDegerlendirmeRepository.Update(binaDegerlendirme);
            await _binaDegerlendirmeRepository.SaveChanges();

            result.Result = binaDegerlendirme.BinaDegerlendirmeId.ToString();
                //"İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.";

            return await Task.FromResult(result);
        }
    }
}