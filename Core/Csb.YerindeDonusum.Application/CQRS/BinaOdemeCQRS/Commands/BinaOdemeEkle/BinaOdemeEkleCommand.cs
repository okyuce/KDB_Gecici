using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeMalikler;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System.Linq;

namespace Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Commands.BinaOdemeEkle;

public class BinaOdemeEkleCommand : IRequest<ResultModel<BinaOdemeEkleResponseModel>>
{
    public long? BinaDegerlendirmeId { get; set; }
    public int? Seviye { get; set; }
    public bool? YapimaYonelikDigerHibeOdemesiMi { get; set; }

    public class BinaOdemeEkleCommandHandler : IRequestHandler<BinaOdemeEkleCommand, ResultModel<BinaOdemeEkleResponseModel>>
    {
        private readonly IMediator _mediator;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IBinaOdemeRepository _binaOdemeRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public BinaOdemeEkleCommandHandler(IMediator mediator
            , IBinaDegerlendirmeRepository binaDegerlendirmeRepository
            , IKullaniciBilgi kullaniciBilgi
            , IBinaOdemeRepository binaOdemeRepository)
        {
            _mediator = mediator;
            _binaOdemeRepository = binaOdemeRepository;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _kullaniciBilgi = kullaniciBilgi;
        }

        public async Task<ResultModel<BinaOdemeEkleResponseModel>> Handle(BinaOdemeEkleCommand request, CancellationToken cancellationToken)
        {
            int seviye = request?.Seviye ?? 0;
            var dateTimeNow = DateTime.Now;

            ResultModel<BinaOdemeEkleResponseModel> result = new();

            long.TryParse(_kullaniciBilgi.GetUserInfo().KullaniciId, out long kullaniciId);
            string? ipAdresi = _kullaniciBilgi.GetUserInfo()?.IpAdresi;

            var binaDegerlendirme = await _binaDegerlendirmeRepository.GetAllQueryable()
                                            .Include(x => x.Basvurus.Where(y => y.SilindiMi == false && y.AktifMi == true
                                                         && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruReddedilmistir)
                                                    )
                                                .ThenInclude(x => x.BasvuruImzaVerens.Where(y => y.SilindiMi == false && y.AktifMi == true))
                                            .Include(x => x.BinaMuteahhits.Where(y => y.SilindiMi == false && y.AktifMi == true))
                                            .Include(x => x.BinaOdemes.Where(y => y.SilindiMi == false && y.AktifMi == true && y.BinaOdemeDurumId != (int)BinaOdemeDurumEnum.Reddedildi))
                                                    .ThenInclude(x => x.BinaOdemeDetays.Where(y => y.SilindiMi == false && y.AktifMi == true))
                .Where(x => x.SilindiMi == false && x.AktifMi == true && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId)

                                            .FirstOrDefaultAsync();

            if (binaDegerlendirme == null)
            {
                result.ErrorMessage("Bina Değerlendirme bilgileri bulunamadı!");
                return await Task.FromResult(result);
            }

            //Include daki where koşulu düzgün çalışmadığı için eklenmiştir.
            binaDegerlendirme.Basvurus = binaDegerlendirme.Basvurus.Where(y => y.SilindiMi == false && y.AktifMi == true
                                                         && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruReddedilmistir).ToList();

            if (binaDegerlendirme.Basvurus?.Any(x => x.BasvuruAfadDurumId == (long)BasvuruAfadDurumEnum.Kabul) == true)
            {
                result.ErrorMessage("AFAD durumu Kabul olan başvurular olduğundan ödeme işlemine devam edilemiyor!");
                return await Task.FromResult(result);
            }

            if (binaDegerlendirme.Basvurus?.Any(x => x.BasvuruImzaVerens == null || x.BasvuruImzaVerens.Count == 0) == true)
            {
                result.ErrorMessage($"Başvurusu bulunan ancak imza veren detayı olmayan kayıtlar var! İşleme devam edilemiyor.");
                return await Task.FromResult(result);
            }

            BinaOdeme binaOdeme = binaDegerlendirme.BinaOdemes.FirstOrDefault(x => x.Seviye == seviye);
            binaOdeme ??= new BinaOdeme()
            {
                BinaOdemeDurumId = (int)BinaOdemeDurumEnum.Bekleniyor,
                Seviye = seviye,
                BinaDegerlendirmeId = binaDegerlendirme.BinaDegerlendirmeId,
                OlusturanIp = ipAdresi,
                OlusturmaTarihi = dateTimeNow,
                OlusturanKullaniciId = kullaniciId,
                AktifMi = true,
                SilindiMi = false,
                TalepDurumu = "Açık",
            };

            if (request?.YapimaYonelikDigerHibeOdemesiMi != true && binaOdeme.BinaOdemeId > 0)
            {
                result.ErrorMessage("Bu Seviye'ye ait bir ödeme bulunmaktadır!");
                return await Task.FromResult(result);
            }

            var seviyeGecersizMi = binaDegerlendirme.BinaOdemes.Any(x => x.Seviye >= seviye);
            if (request?.YapimaYonelikDigerHibeOdemesiMi != true && seviyeGecersizMi)
            {
                result.ErrorMessage("Lütfen halihazırda onaylanan seviyelerden daha yüksek bir seviye giriniz.");
                return await Task.FromResult(result);
            }

            if (seviye == 100)
            {
                var oncekiSeviyeMevcutMu = binaDegerlendirme.BinaOdemes.Any(x => x.Seviye == 60);
                if (!oncekiSeviyeMevcutMu)
                {
                    result.ErrorMessage("Lütfen önce %60 için ödeme talebini oluşturunuz.");
                    return await Task.FromResult(result);
                }
            }
            else if (seviye == 60)
            {
                var oncekiSeviyeMevcutMu = binaDegerlendirme.BinaOdemes.Any(x => x.Seviye == 20);
                if (!oncekiSeviyeMevcutMu)
                {
                    result.ErrorMessage("Lütfen önce %20 için ödeme talebini oluşturunuz.");
                    return await Task.FromResult(result);
                }
            }

            #region ODEME TUTARI HESAPLANIYOR
            var malikListesiResult = await _mediator.Send(new GetirListeMaliklerQuery()
            {
                BagimsizBolumlerAlinmasin = true,
                SadeceImzaVerenlerAlinsin = true,
                UavtMahalleNo = binaDegerlendirme.UavtMahalleNo,
                HasarTespitAskiKodu = binaDegerlendirme.HasarTespitAskiKodu,
                BinaDisKapiNo = binaDegerlendirme.BinaDisKapiNo
            });

            var adaParselBosKarakterListe = new List<string?>() { "", "-", "0", null };

            if (adaParselBosKarakterListe.Contains(binaDegerlendirme.Ada))
                malikListesiResult.Result = malikListesiResult.Result.Where(x => adaParselBosKarakterListe.Contains(x.TapuAda)).ToList();
            else
                malikListesiResult.Result = malikListesiResult.Result.Where(x => x.TapuAda == binaDegerlendirme.Ada).ToList();

            if (adaParselBosKarakterListe.Contains(binaDegerlendirme.Parsel))
                malikListesiResult.Result = malikListesiResult.Result.Where(x => adaParselBosKarakterListe.Contains(x.TapuParsel)).ToList();
            else
                malikListesiResult.Result = malikListesiResult.Result.Where(x => x.TapuParsel == binaDegerlendirme.Parsel).ToList();

            decimal imzaVerenMalikSayisi = (malikListesiResult.Result?.Sum(x => ((x.HissePayda != null && x.HissePay != null) ? (decimal)(x.HissePay) / (decimal)x.HissePayda : 1)) ?? 0);
            decimal imzaVerenMalikSayisiFeragatsiz = (malikListesiResult.Result?.Where(x => !(x.HibeOdemeTutar == 0 && x.KrediOdemeTutar == 0)).Sum(x => ((x.HissePayda != null && x.HissePay != null) ? (decimal)(x.HissePay) / (decimal)x.HissePayda : 1)) ?? 0);

            if (imzaVerenMalikSayisi == 0)
            {
                result.ErrorMessage("Malik listesi alınamadı.");
                return await Task.FromResult(result);
            }

            if (request?.YapimaYonelikDigerHibeOdemesiMi == true)
            {
                binaOdeme.Seviye = 0;
                binaOdeme.DigerHibeOdemeTutari = (imzaVerenMalikSayisiFeragatsiz * 40000);
                binaOdeme.OdemeTutari = binaOdeme.DigerHibeOdemeTutari;

                if (!binaOdeme.BinaOdemeDetays.Any())
                {
                    decimal imzaVerenIbanGirenKisiSayisi = 0;

                    foreach (var basvuru in binaDegerlendirme.Basvurus)
                    {
                        var basvuruImzaVeren = basvuru.BasvuruImzaVerens.FirstOrDefault();

                        if (basvuruImzaVeren == null)
                        {
                            result.ErrorMessage($"{basvuru.Ad} {basvuru.Soyad} kişisine ait imza veren detayı olmadığı için işleme devam edilemiyor.");
                            return await Task.FromResult(result);
                        }

                        var hisseMiktari = (basvuruImzaVeren.HissePayda != null && basvuruImzaVeren.HissePay != null ? (decimal)(basvuruImzaVeren.HissePay) / (decimal)basvuruImzaVeren.HissePayda : (decimal)1);
                        if (basvuruImzaVeren.IbanGirildiMi)
                        {
                            imzaVerenIbanGirenKisiSayisi += hisseMiktari;
                            if (!(basvuruImzaVeren.HibeOdemeTutar == 0 && basvuruImzaVeren.KrediOdemeTutar == 0))
                            {
                                binaOdeme.BinaOdemeDetays.Add(new BinaOdemeDetay
                                {
                                    Ad = string.Concat(basvuru.Ad, " ", basvuru.Soyad),
                                    Iban = basvuruImzaVeren.IbanNo,
                                    MuteahhitMi = false,
                                    OdemeTutari = (int)(40000 * hisseMiktari),
                                    HibeOdemeTutari = null,
                                    KrediOdemeTutari = null,
                                    DigerHibeOdemeTutari = (int)(40000 * hisseMiktari),
                                    OlusturanIp = ipAdresi,
                                    OlusturmaTarihi = dateTimeNow,
                                    OlusturanKullaniciId = kullaniciId,
                                    AktifMi = true,
                                    SilindiMi = false
                                });
                            }
                        }
                    }

                    decimal imzaVerenIbanGirmeyenKisiSayisi = imzaVerenMalikSayisiFeragatsiz - imzaVerenIbanGirenKisiSayisi;
                    if (imzaVerenIbanGirmeyenKisiSayisi > 0)
                    {
                        var binaMuteahhit = binaDegerlendirme.BinaMuteahhits.First();

                        binaOdeme.BinaOdemeDetays.Add(new BinaOdemeDetay
                        {
                            Ad = binaMuteahhit.Adsoyadunvan,
                            Iban = binaMuteahhit.IbanNo,
                            MuteahhitMi = true,
                            OdemeTutari = (imzaVerenIbanGirmeyenKisiSayisi * 40000),
                            HibeOdemeTutari = null,
                            KrediOdemeTutari = null,
                            DigerHibeOdemeTutari = (imzaVerenIbanGirmeyenKisiSayisi * 40000),
                            OlusturanIp = ipAdresi,
                            OlusturmaTarihi = dateTimeNow,
                            OlusturanKullaniciId = kullaniciId,
                            AktifMi = true,
                            SilindiMi = false
                        });
                    }
                    // hisseli işlem yapıldığı için artık bu kontrole gerek yok.
                    //if (binaDegerlendirme.Basvurus.Count() != imzaVerenMalikSayisi)
                    //{
                    //    result.ErrorMessage("İmza veren malik sayısı ile başvuru yapan kişi sayısı aynı olmadığı için işleme devam edilemiyor.");
                    //    return await Task.FromResult(result);
                    //}
                }
            }
            else
            {
                decimal? toplamHibeYuzde = 0;
                decimal? toplamKrediYuzde = 0;

                malikListesiResult.Result.ForEach(x =>
                {
                    toplamHibeYuzde += (x.HibeOdemeTutar ?? 0);
                    toplamKrediYuzde += (x.KrediOdemeTutar ?? 0);
                });

                if (seviye == 10)
                {
                    toplamHibeYuzde = toplamHibeYuzde * seviye / 100;
                    toplamKrediYuzde = toplamKrediYuzde * seviye / 100;
                }
                else if (seviye == 20)
                {
                    var oncekiHibeOdemeTutari = binaDegerlendirme.BinaOdemes.Sum(x => x.HibeOdemeTutari);
                    var oncekiKrediOdemeTutari = binaDegerlendirme.BinaOdemes.Sum(x => x.KrediOdemeTutari);

                    toplamHibeYuzde -= oncekiHibeOdemeTutari;
                    toplamKrediYuzde -= oncekiKrediOdemeTutari;
                    toplamHibeYuzde /= 3;
                    toplamKrediYuzde /= 3;
                    ////toplamHibeYuzde = toplamHibeYuzde*10/100;
                    ////toplamKrediYuzde = toplamKrediYuzde * 10 / 100;
                }
                else if (seviye == 60)
                {
                    var seviye20Olan = binaDegerlendirme.BinaOdemes.FirstOrDefault(x => x.Seviye == 20);
                    toplamHibeYuzde = seviye20Olan?.HibeOdemeTutari ?? 0;
                    toplamKrediYuzde = seviye20Olan?.KrediOdemeTutari ?? 0;

                    ////var oncekiHibeOdemeTutari = binaDegerlendirme.BinaOdemes.Sum(x => x.HibeOdemeTutari);
                    ////var oncekiKrediOdemeTutari = binaDegerlendirme.BinaOdemes.Sum(x => x.KrediOdemeTutari);

                    ////toplamHibeYuzde -= oncekiHibeOdemeTutari;
                    ////toplamKrediYuzde -= oncekiKrediOdemeTutari;
                    ////toplamHibeYuzde = toplamHibeYuzde * 40 / 100;
                    ////toplamKrediYuzde = toplamKrediYuzde * 40 / 100;
                }
                else if (seviye == 100)
                {
                    var oncekiHibeOdemeTutari = binaDegerlendirme.BinaOdemes.Sum(x => x.HibeOdemeTutari);
                    var oncekiKrediOdemeTutari = binaDegerlendirme.BinaOdemes.Sum(x => x.KrediOdemeTutari);

                    toplamHibeYuzde -= oncekiHibeOdemeTutari;
                    toplamKrediYuzde -= oncekiKrediOdemeTutari;
                }

                binaOdeme.OdemeTutari = toplamHibeYuzde + toplamKrediYuzde;
                binaOdeme.HibeOdemeTutari = toplamHibeYuzde;
                binaOdeme.KrediOdemeTutari = toplamKrediYuzde;

                if (!binaOdeme.BinaOdemeDetays.Any())
                {
                    var binaMuteahhit = binaDegerlendirme.BinaMuteahhits.First();

                    binaOdeme.BinaOdemeDetays.Add(new BinaOdemeDetay
                    {
                        Ad = binaMuteahhit.Adsoyadunvan,
                        Iban = binaMuteahhit.IbanNo,
                        MuteahhitMi = true,
                        OdemeTutari = binaOdeme.OdemeTutari.Value,
                        HibeOdemeTutari = binaOdeme.HibeOdemeTutari,
                        KrediOdemeTutari = binaOdeme.KrediOdemeTutari,
                        DigerHibeOdemeTutari = binaOdeme.DigerHibeOdemeTutari,
                        OlusturanIp = ipAdresi,
                        OlusturmaTarihi = dateTimeNow,
                        OlusturanKullaniciId = kullaniciId,
                        AktifMi = true,
                        SilindiMi = false
                    });
                }
            }

            if (binaOdeme.OdemeTutari <= 0)
            {
                return await Task.FromResult(result);
            }
            #endregion

            if (!(binaOdeme.BinaOdemeId > 0))
            {
                await _binaOdemeRepository.AddAsync(binaOdeme);
                await _binaOdemeRepository.SaveChanges();

                binaOdeme.TalepNumarasi = dateTimeNow.ToString("yyyyMMdd") + "-" + binaOdeme.BinaOdemeId;
            }

            // talep numarasinda Id kullanilacagi icin Update her halukarda calisacak.
            _binaOdemeRepository.Update(binaOdeme);
            await _binaOdemeRepository.SaveChanges();

            result.Result = new BinaOdemeEkleResponseModel
            {
                Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.",
            };

            return await Task.FromResult(result);
        }
    }
}