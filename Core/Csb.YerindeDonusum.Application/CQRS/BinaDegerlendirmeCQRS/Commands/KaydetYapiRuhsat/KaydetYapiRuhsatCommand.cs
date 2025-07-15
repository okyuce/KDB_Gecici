using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Commands.BinaOdemeEkle;
using Csb.YerindeDonusum.Application.CQRS.YapiBelgeCQRS.Queries.GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNo;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetYapiRuhsat;

public class KaydetYapiRuhsatCommand : IRequest<ResultModel<KaydetYapiRuhsatCommandResponseModel>>
{
    public string? HasarTespitAskiKodu { get; set; }
    public string? BinaDisKapiNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public long? BinaDegerlendirmeId { get; set; }
    public long? BultenNo { get; set; }
    public long? IzinBelgesiSayi { get; set; }
    public DateTime? IzinBelgesiTarih { get; set; }
    public DosyaDto? BelgeYapiRuhsatIzinDosya { get; set; }
    public bool? IzinBelgesiSeciliMi { get; set; }

    public class KaydetYapiRuhsatCommandHandler : IRequestHandler<KaydetYapiRuhsatCommand, ResultModel<KaydetYapiRuhsatCommandResponseModel>>
    {
        private readonly IMediator _mediator;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IAfadBasvuruTekilRepository _afadBasvuruTekilRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IConfiguration _configuration;

        public KaydetYapiRuhsatCommandHandler(IBinaDegerlendirmeRepository binaDegerlendirmeRepository
            , IKullaniciBilgi kullaniciBilgi
            , IMediator mediator
            , IConfiguration configuration
            , IAfadBasvuruTekilRepository afadBasvuruTekilRepository)
        {
            _mediator = mediator;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _kullaniciBilgi = kullaniciBilgi;
            _configuration = configuration;
            _afadBasvuruTekilRepository = afadBasvuruTekilRepository;
        }

        public async Task<ResultModel<KaydetYapiRuhsatCommandResponseModel>> Handle(KaydetYapiRuhsatCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<KaydetYapiRuhsatCommandResponseModel>();

            request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);

            var binaDegerlendirme = await _binaDegerlendirmeRepository.GetAllQueryable()
                                            .Include(x => x.Basvurus.Where(y => y.SilindiMi == false && y.AktifMi == true
                                                         && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruReddedilmistir)
                                                    )
                                                .ThenInclude(x => x.BasvuruImzaVerens.Where(y => y.SilindiMi == false && y.AktifMi == true))
                                            .Include(i => i.BinaDegerlendirmeDosyas.Where(x => x.SilindiMi == false && x.AktifMi == true))
                                            .Include(i => i.BinaMuteahhits.Where(x => x.SilindiMi == false && x.AktifMi == true))
                                            .Include(i => i.BinaYapiRuhsatIzinDosyas.Where(x => x.SilindiMi == false && x.AktifMi == true)).Where(x => !x.SilindiMi && x.AktifMi == true
                                                && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId
                                            )
                                            .FirstOrDefaultAsync();

            if (binaDegerlendirme == null)
            {
                result.ErrorMessage("Bina Değerlendirme bilgisi bulunamadı.");
                return result;
            }
            if (!binaDegerlendirme.BinaDegerlendirmeDosyas.Any())
            {
                result.ErrorMessage("Lütfen önce 'Belge Bilgileri' alanını doldurunuz.");
                return result;
            }

            if (binaDegerlendirme.Basvurus?.Any(x => x.BasvuruImzaVerens == null || x.BasvuruImzaVerens.Count == 0) == true)
            {
                result.ErrorMessage($"Başvurusu bulunan ancak imza veren detayı olmayan kayıtlar var! İşleme devam edilemiyor.");
                return await Task.FromResult(result);
            }
            if (!binaDegerlendirme.BinaMuteahhits.Any())
            {
                result.ErrorMessage("Lütfen önce 'Müteahhit Bilgileri' alanını doldurunuz.");
                return result;
            }

            #region Ortak Kontroller
            if (request?.IzinBelgesiSeciliMi == true && (request?.IzinBelgesiSayi.HasValue == false || request?.IzinBelgesiSayi.Value == 0))
            {
                result.ErrorMessage("Lütfen izin belge sayısını giriniz!.");
                return result;
            }

            if (request?.IzinBelgesiSeciliMi == true && request?.IzinBelgesiTarih == null)
            {
                result.ErrorMessage("Lütfen izin belge tarihini giriniz!.");
                return result;
            }

            if (request?.IzinBelgesiSeciliMi == true
                && string.IsNullOrWhiteSpace(request?.BelgeYapiRuhsatIzinDosya?.DosyaBase64)
                && !binaDegerlendirme.BinaYapiRuhsatIzinDosyas.Any(x => x.AktifMi == true && x.SilindiMi == false))
            {
                result.ErrorMessage("Lütfen izin belgesini yükleyiniz.!.");
                return result;
            }

            if (request?.IzinBelgesiSeciliMi != true && request?.BultenNo.HasValue != true || (request?.BultenNo.HasValue == true && request?.BultenNo.Value == 0))
            {
                result.ErrorMessage("Lütfen bülten numaranızı giriniz.!.");
                return result;
            }
            #endregion

            // Bütün Malikler İmza Vermemişse İşleme Devam Edilemez.
            var butunMaliklerImzaVerdiMiResult = await DomainServices.ButunMaliklerImzaVerdiMi(_mediator, binaDegerlendirme);
            if (butunMaliklerImzaVerdiMiResult.IsError)
            {
                result.ErrorMessage(butunMaliklerImzaVerdiMiResult.ErrorMessageContent);
                return result;
            }

            var disKapiVarMi = _binaDegerlendirmeRepository
                                            .GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                && x.BinaDegerlendirmeDurumId != (long)BinaDegerlendirmeDurumEnum.Reddedildi
                                                && x.BinaDisKapiNo.ToLower() == request.BinaDisKapiNo.Trim().ToLower()
                                                && x.UavtMahalleNo == request.UavtMahalleNo
                                                && x.HasarTespitAskiKodu == request.HasarTespitAskiKodu
                                                && x.BinaDegerlendirmeId != binaDegerlendirme.BinaDegerlendirmeId,
                                                asNoTracking: false
                                            ).Any();

            if (disKapiVarMi)
            {
                result.ErrorMessage("Bu Dış Kapı No bilgisine sahip bir Yapı sistemde mevcut.");
                return result;
            }

            if (request?.IzinBelgesiSeciliMi != true)
            {
                var yapiBelgeRuhsat = await _mediator.Send(new GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQuery
                {
                    BultenNo = request?.BultenNo,
                    BinaDegerlendirmeId = binaDegerlendirme.BinaDegerlendirmeId,
                });

                if (yapiBelgeRuhsat.IsError)
                {
                    result.ErrorMessage(yapiBelgeRuhsat.ErrorMessageContent);
                    return result;
                }

                binaDegerlendirme.BultenNo = request?.BultenNo;
                binaDegerlendirme.YapiKimlikNo = yapiBelgeRuhsat.Result.YapiKimlikNo;
                binaDegerlendirme.YapiInsaatAlan = yapiBelgeRuhsat.Result.ToplamYapiInsaatAlan;
                binaDegerlendirme.BagimsizBolumSayisi = yapiBelgeRuhsat.Result.ToplamBBSayi;
                binaDegerlendirme.ToplamKatSayisi = yapiBelgeRuhsat.Result.ToplamKatSayi;
                binaDegerlendirme.KotUstKatSayisi = yapiBelgeRuhsat.Result.YolKotUstKatSayi;
                binaDegerlendirme.KotAltKatSayisi = yapiBelgeRuhsat.Result.YolKotAltKatSayi;
                binaDegerlendirme.RuhsatOnayTarihi = yapiBelgeRuhsat.Result.RuhsatOnayTarihi;
                binaDegerlendirme.GuncelleyenKullaniciId = yapiBelgeRuhsat.Result.YolKotAltKatSayi;
                binaDegerlendirme.IzinBelgesiSayi = null;
                binaDegerlendirme.IzinBelgesiTarih = null;
            }
            else
            {
                binaDegerlendirme.BultenNo = null;
                binaDegerlendirme.IzinBelgesiTarih = request.IzinBelgesiTarih;
                binaDegerlendirme.IzinBelgesiSayi = request.IzinBelgesiSayi;
                binaDegerlendirme.YapiKimlikNo = null;
                binaDegerlendirme.YapiInsaatAlan = null;
                binaDegerlendirme.BagimsizBolumSayisi = null;
                binaDegerlendirme.ToplamKatSayisi = null;
                binaDegerlendirme.KotUstKatSayisi = null;
                binaDegerlendirme.KotAltKatSayisi = null;
                binaDegerlendirme.RuhsatOnayTarihi = null;
                binaDegerlendirme.GuncelleyenKullaniciId = null;
            }

            binaDegerlendirme.BinaDisKapiNo = request?.BinaDisKapiNo ?? binaDegerlendirme.BinaDisKapiNo;

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            long.TryParse(kullaniciBilgi.KullaniciId, out long kullaniciId);
            binaDegerlendirme.GuncellemeTarihi = DateTime.Now;
            binaDegerlendirme.GuncelleyenIp = kullaniciBilgi.IpAdresi;

            binaDegerlendirme.BinaDegerlendirmeDurumId = BinaDegerlendirmeDurumEnum.YapiRuhsatinizOnaylanmistir.GetHashCode();

            #region Yapı Denetim Seviye Tespit Dosya
            var binaYapiRuhsatIzinDosya = binaDegerlendirme.BinaYapiRuhsatIzinDosyas.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(request?.BelgeYapiRuhsatIzinDosya?.DosyaBase64))
            {
                if (binaYapiRuhsatIzinDosya == null)
                {
                    binaYapiRuhsatIzinDosya = new BinaYapiRuhsatIzinDosya()
                    {
                        AktifMi = true,
                        SilindiMi = false,
                    };

                    binaDegerlendirme.BinaYapiRuhsatIzinDosyas.Add(binaYapiRuhsatIzinDosya);
                }

                binaYapiRuhsatIzinDosya!.DosyaAdi = string.Concat(Guid.NewGuid(), request.BelgeYapiRuhsatIzinDosya.DosyaUzanti);
                binaYapiRuhsatIzinDosya.DosyaYolu = DateTime.Now.ToString("yyyy-MM-dd");
                binaYapiRuhsatIzinDosya.DosyaTuru = MimeTypes.GetMimeType(request.BelgeYapiRuhsatIzinDosya.DosyaUzanti ?? "");

                byte[] data = Convert.FromBase64String(request.BelgeYapiRuhsatIzinDosya.DosyaBase64);

                var isTheFileTypeAllowed = FileTypeVerifier.Verify(data);

                if (!isTheFileTypeAllowed.IsVerified)
                {
                    result.ErrorMessage("Geçersiz veya Hatalı Dosya Uzantısı.");
                    return await Task.FromResult(result);
                }

                var uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", binaYapiRuhsatIzinDosya.DosyaYolu);

                if (!Directory.Exists(uploadDirectoryPath))
                    Directory.CreateDirectory(uploadDirectoryPath);

                var filePath = string.Concat(uploadDirectoryPath, "\\", binaYapiRuhsatIzinDosya.DosyaAdi);

                using var stream = File.Create(filePath);

                stream.Write(data, 0, data.Length);
            }
            #endregion

            #region İmza veren malik sayısı kadar 40.000 ₺ ödeme ekleniyor.
            var binaOdemeResult1 = await _mediator.Send(new BinaOdemeEkleCommand()
            {
                BinaDegerlendirmeId = binaDegerlendirme.BinaDegerlendirmeId,
                YapimaYonelikDigerHibeOdemesiMi = true,
            });
            #endregion

            #region Ödeme Listesinde %10 Peşinat yok ise otomatik olarak Ekleniyor
            // %10 seviye yok ise ekleyeceğiz ve durumu otomatik olarak IstekAlindi olacak.
            var binaOdemeResult2 = await _mediator.Send(new BinaOdemeEkleCommand()
            {
                BinaDegerlendirmeId = binaDegerlendirme.BinaDegerlendirmeId,
                Seviye = 10
            });
            #endregion

            if (binaOdemeResult1.IsError && binaOdemeResult2.IsError)
            {
                string mesajDetay = string.Empty;

                if (binaOdemeResult1.IsError)
                    mesajDetay = binaOdemeResult1.ErrorMessageContent;

                if (binaOdemeResult2.IsError)
                    mesajDetay += $"{(binaOdemeResult1.IsError ? " - " : string.Empty)}{binaOdemeResult2.ErrorMessageContent}";

                result.ErrorMessage($"Ödeme talepleri hesaplanamadığı için işleme devam edilemiyor! Detay: {mesajDetay}");
                return await Task.FromResult(result);
            }

            _binaDegerlendirmeRepository.Update(binaDegerlendirme);
            await _binaDegerlendirmeRepository.SaveChanges();

            result.Result = new KaydetYapiRuhsatCommandResponseModel
            {
                Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.",
                DosyaGuid = binaYapiRuhsatIzinDosya!.BinaYapiRuhsatIzinDosyaGuid,
                DosyaAdi = binaYapiRuhsatIzinDosya!.DosyaAdi,
            };

            return await Task.FromResult(result);
        }
    }
}