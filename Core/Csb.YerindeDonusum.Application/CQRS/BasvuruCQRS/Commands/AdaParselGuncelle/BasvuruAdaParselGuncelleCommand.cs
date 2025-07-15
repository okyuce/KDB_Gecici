using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.AdaParselGuncelle;

public class BasvuruAdaParselGuncelleCommand : IRequest<ResultModel<string>>
{
    public BasvuruAdaParselGuncelleCommandModel? Model { get; set; }

    public class BasvuruAdaParselGuncelleCommandHandler : IRequestHandler<BasvuruAdaParselGuncelleCommand, ResultModel<string>>
    {
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IBasvuruKamuUstlenecekRepository _basvuruKamuUstlenecekRepository;
        private readonly IConfiguration _configuration;

        public BasvuruAdaParselGuncelleCommandHandler(
            IKullaniciBilgi kullaniciBilgi,
            IBinaDegerlendirmeRepository binaDegerlendirmeRepository,
            IBasvuruKamuUstlenecekRepository basvuruKamuUstlenecekRepository,
            IConfiguration configuration)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _basvuruKamuUstlenecekRepository = basvuruKamuUstlenecekRepository;
            _configuration = configuration;
        }

        public async Task<ResultModel<string>> Handle(BasvuruAdaParselGuncelleCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            var userInfo = _kullaniciBilgi.GetUserInfo();
            long.TryParse(userInfo.KullaniciId, out long kullaniciId);
            var binaDegerlendirmeQuery = _binaDegerlendirmeRepository.GetAllQueryable(x => x.AktifMi == true && x.SilindiMi == false
                                                                                        && x.BinaDegerlendirmeDurumId != (long)BinaDegerlendirmeDurumEnum.Reddedildi
                                                                                        && x.BinaDegerlendirmeId == request.Model.BinaDegerlendirmeId
                                            ).Include(i => i.Basvurus.Where(y => y.SilindiMi == false && y.AktifMi == true && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir))
                                            .Include(i => i.BinaDegerlendirmeDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true && y.BinaDegerlendirmeDosyaTurId == (long)BinaDegerlendirmeDosyaTurEnum.AdaParselGuncelleme))
                                            .FirstOrDefault();

            //if (userInfo.BirimId != BirimlerEnum.AltyapiGenelMudurlugu)
            //{
            //    result.ErrorMessage("Bu işlemi yapmaya yetkiniz yoktur.");
            //    return await Task.FromResult(result);
            //}

            if (!binaDegerlendirmeQuery.Basvurus.Any())
            {
                result.ErrorMessage("Binada basvuran olmadığı için işleminiz gerçekleştirilemiyor.");
                return await Task.FromResult(result);
            }
            else if (binaDegerlendirmeQuery.Basvurus.Any(x => x.EskiTapuGuncellemeTarihi == null))
            {
                foreach (var item in binaDegerlendirmeQuery.Basvurus)
                {
                    if (item.EskiTapuGuncellemeTarihi == null)
                    {
                        item.EskiTapuMahalleAdi = item.TapuMahalleAdi;
                        item.EskiTapuMahalleId = item.TapuMahalleId;
                        item.EskiTapuAda = item.TapuAda;
                        item.EskiTapuParsel = item.TapuParsel;
                        item.EskiTapuIlceAdi = item.TapuIlceAdi;
                        item.EskiTapuIlceId = item.TapuIlceId;
                        item.EskiTapuGuncellemeTarihi = DateTime.Now;
                        item.EskiTapuGuncelleyenKullaniciId = kullaniciId;
                    }

                    item.TapuIlceAdi = request.Model.TapuBeyanIlceAdi;
                    item.TapuIlceId = request.Model.TapuBeyanIlceId;
                    item.TapuMahalleAdi = request.Model.TapuBeyanMahalleAdi;
                    item.TapuMahalleId = request.Model.TapuBeyanMahalleId;
                    item.TapuAda = request.Model.TapuBeyanAda;
                    item.TapuParsel = request.Model.TapuBeyanParsel;
                }
                var kamuUstlenecekKayitlari = _basvuruKamuUstlenecekRepository.GetAllQueryable(x => x.AktifMi == true && x.SilindiMi == false
                                                                                        && x.UavtMahalleNo == binaDegerlendirmeQuery.UavtMahalleNo
                                                                                        && x.TapuAda == binaDegerlendirmeQuery.Ada
                                                                                        && x.TapuParsel == binaDegerlendirmeQuery.Parsel).ToList();
                if (kamuUstlenecekKayitlari != null && kamuUstlenecekKayitlari.Any())
                {
                    foreach (var kamuUstlenecek in kamuUstlenecekKayitlari)
                    {
                        if (kamuUstlenecek.EskiTapuGuncellemeTarihi == null)
                        {
                            kamuUstlenecek.EskiTapuMahalleAdi = kamuUstlenecek.TapuMahalleAdi;
                            kamuUstlenecek.EskiTapuMahalleId = kamuUstlenecek.TapuMahalleId;
                            kamuUstlenecek.EskiTapuAda = kamuUstlenecek.TapuAda;
                            kamuUstlenecek.EskiTapuParsel = kamuUstlenecek.TapuParsel;
                            kamuUstlenecek.EskiTapuIlceAdi = kamuUstlenecek.TapuIlceAdi;
                            kamuUstlenecek.EskiTapuIlceId = kamuUstlenecek.TapuIlceId;
                            kamuUstlenecek.EskiTapuGuncellemeTarihi = DateTime.Now;
                            kamuUstlenecek.EskiTapuGuncelleyenKullaniciId = kullaniciId;
                        }
                        kamuUstlenecek.TapuIlceAdi = request.Model.TapuBeyanIlceAdi;
                        kamuUstlenecek.TapuIlceId = request.Model.TapuBeyanIlceId;
                        kamuUstlenecek.TapuMahalleAdi = request.Model.TapuBeyanMahalleAdi;
                        kamuUstlenecek.TapuMahalleId = request.Model.TapuBeyanMahalleId;
                        kamuUstlenecek.TapuAda = request.Model.TapuBeyanAda;
                        kamuUstlenecek.TapuParsel = request.Model.TapuBeyanParsel;
                    }
                }

                binaDegerlendirmeQuery.UavtIlceAdi = request.Model.TapuBeyanIlceAdi;
                binaDegerlendirmeQuery.UavtMahalleAdi = request.Model.TapuBeyanMahalleAdi;

                binaDegerlendirmeQuery.Ada = request.Model.TapuBeyanAda;
                binaDegerlendirmeQuery.Parsel = request.Model.TapuBeyanParsel;
                binaDegerlendirmeQuery.AdaParselGuncellemeTipiId = (long)request.Model.AdaParselGuncellemeTipiId;

                #region ...: Başka Parsel Dosyası Kayıt Etme İşlemleri :...

                var binaDegerlendirmeDosya = binaDegerlendirmeQuery.BinaDegerlendirmeDosyas.FirstOrDefault();
                binaDegerlendirmeDosya ??= new BinaDegerlendirmeDosya
                {
                    AktifMi = true,
                    SilindiMi = false,
                };

                if (request.Model.BaskaParselDosya != null)
                {
                    binaDegerlendirmeDosya.DosyaAdi = string.Concat(Guid.NewGuid(), request.Model.BaskaParselDosya?.DosyaUzanti);
                    binaDegerlendirmeDosya.DosyaYolu = DateTime.Now.ToString("yyyy-MM-dd");
                    binaDegerlendirmeDosya.DosyaTuru = MimeTypes.GetMimeType(request.Model.BaskaParselDosya?.DosyaUzanti ?? "");
                    binaDegerlendirmeDosya.BinaDegerlendirmeDosyaTurId = (long)BinaDegerlendirmeDosyaTurEnum.AdaParselGuncelleme;

                    byte[] data = Convert.FromBase64String(request.Model.BaskaParselDosya?.DosyaBase64);

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

                    binaDegerlendirmeQuery.BinaDegerlendirmeDosyas.Add(binaDegerlendirmeDosya);
                }

                #endregion

                _basvuruKamuUstlenecekRepository.SaveChanges(cancellationToken);
                _binaDegerlendirmeRepository.SaveChanges(cancellationToken);
            }
            else
            {
                result.ErrorMessage("Daha önce ada parsel no güncelleme işlemi yapılmıştır.");
                return await Task.FromResult(result);
            }

            //result.Result = "Bina Ada Parsel Güncelleme İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.";

            return await Task.FromResult(result);
        }
    }
}