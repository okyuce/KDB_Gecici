using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazByTakbisTasinmazId;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.Takbis;
using Csb.YerindeDonusum.Domain.Entities;
using EFCore.BulkExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.GuncelleBasvuruAfadDurum;

public class GuncelleTabloBasvuruKamuUstlenecekJob : IRequest<ResultModel<string>>
{
    public int? Take { get; set; }
    public bool? JobMuCalisiyor { get; set; } = true;
    public int? UavtMahalleNo { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }

    public class GuncelleTabloBasvuruKamuUstlenecekJobHandler : IRequestHandler<GuncelleTabloBasvuruKamuUstlenecekJob, ResultModel<string>>
    {
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IMediator _mediator;
        private readonly IBasvuruKamuUstlenecekRepository _basvuruKamuUstlenecekRepository;
        private readonly ITakbisService _takbisService;

        public GuncelleTabloBasvuruKamuUstlenecekJobHandler(ITakbisService takbisService, IBasvuruKamuUstlenecekRepository basvuruKamuUstlenecekRepository, IBasvuruRepository basvuruRepository)
        {
            _takbisService = takbisService;
            _basvuruKamuUstlenecekRepository = basvuruKamuUstlenecekRepository;
            _basvuruRepository = basvuruRepository;
        }

        public async Task<ResultModel<string>> Handle(GuncelleTabloBasvuruKamuUstlenecekJob request, CancellationToken cancellationToken)
        {
            ResultModel<string> result = new();

            if (request.JobMuCalisiyor == true)
            {
                result.ErrorMessage("Job için metot kapalıdır.");
                return result;
            }

            long bakilanSonBasvuruId = 0;
            var bakilanSonTapuServisi = new StringBuilder();

            try
            {
                StringBuilder sb = new();

                if (request?.JobMuCalisiyor == true)
                    sb.AppendLine(string.Format("{0} GuncelleTabloBasvuruKamuUstlenecekJob çalışmaya başladı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                var guncellemeTarihKontrol = DateTime.Now.AddDays(-3);

                // mahalle no ve aski koduna gore gruplaniyor yani bina listesi oluyor.
                var basvuruQuery = _basvuruRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                                            && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                            && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                            && x.TapuMahalleId > 0
                                                            && x.TapuTasinmazId > 0
                                    , true);

                var basvuruKamuUstlenecekQuery = _basvuruKamuUstlenecekRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true, true);

                // job degilse zaten tek bir binaya bakilacagi icin bu sorguyu eklemiyoruz.
                if (request?.JobMuCalisiyor == true)
                {
                    basvuruQuery = basvuruQuery.Where(x => x.BasvuruKamuUstlenecekGuncellemeTarihi == null || x.BasvuruKamuUstlenecekGuncellemeTarihi < guncellemeTarihKontrol);
                }
                if (FluentValidationExtension.NotEmpty(request?.UavtMahalleNo))
                {
                    basvuruQuery = basvuruQuery.Where(x => request.UavtMahalleNo == x.UavtMahalleNo);
                    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => request.UavtMahalleNo == x.UavtMahalleNo);
                }
                if (FluentValidationExtension.NotWhiteSpace(request?.HasarTespitAskiKodu))
                {
                    basvuruQuery = basvuruQuery.Where(x => request.HasarTespitAskiKodu == x.HasarTespitAskiKodu);
                }
                if (FluentValidationExtension.NotEmpty(request?.TapuAda))
                {
                    basvuruQuery = basvuruQuery.Where(x => request.TapuAda == x.TapuAda);
                    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => request.TapuAda == x.TapuAda);
                }
                if (FluentValidationExtension.NotEmpty(request?.TapuParsel))
                {
                    basvuruQuery = basvuruQuery.Where(x => request.TapuParsel == x.TapuParsel);
                    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => request.TapuParsel == x.TapuParsel);
                }

                var basvuruListe = basvuruQuery.GroupBy(x => new { x.UavtMahalleNo, x.HasarTespitAskiKodu })
                                                .Select(x => x.FirstOrDefault()!)
                                                //.Take(request?.Take ?? 100000)
                                                .ToList();

                var basvuruKamuUstlenecekListe = await basvuruKamuUstlenecekQuery.ToListAsync();

                List<BasvuruKamuUstlenecek> basvuruKamuUstlenecekListeEklenecek = new();
                List<BasvuruKamuUstlenecek> basvuruKamuUstlenecekListeGuncellenecek = new();
                List<Basvuru> basvuruListeGuncellenecek = new();

                var metotCalismaBaslangicTarihi = DateTime.Now;

                foreach (var basvuru in basvuruListe)
                //await Parallel.ForEachAsync(basvuruListe, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = 5 }, async (basvuru, cancellationToken) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    bakilanSonBasvuruId = basvuru.BasvuruId;

                    var bagimsizBolumQuery = new GetirBagimsizBolumModel
                    {
                        AdaNo = basvuru.TapuAda,
                        ParselNo = basvuru.TapuParsel,
                        TapuBolumDurum = TapuBolumDurumEnum.Hepsi,
                        MahalleIds = new decimal[] { basvuru.TapuMahalleId!.Value },
                        //Blok = s.TapuBlok
                    };

                    bakilanSonTapuServisi.Clear();
                    bakilanSonTapuServisi.Append("_takbisService.GetirBagimsizBolumAsync -> " + JsonConvert.SerializeObject(bagimsizBolumQuery));

                    // binanin bulundugu ada, parsel, mahalleid bilgisine gore bagimsiz bolumler aliniyor.
                    var bagimsizBolumListe = await _takbisService.GetirBagimsizBolumAsync(bagimsizBolumQuery);

                    var datetimeNow = DateTime.Now;

                    //await Parallel.ForEachAsync(bagimsizBolumListe, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = 5 }, async (bagimsizBolum, cancellationToken) =>
                    foreach (var bagimsizBolum in bagimsizBolumListe)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        string? katStr = bagimsizBolum.BagimsizBolum?.Kat?.ToLower();
                        int kat = 0;
                        int.TryParse(katStr, out kat);
                        if (katStr == "bodrum") kat = -1;
                        else if (katStr == "zemin") kat = 0;


                        bakilanSonTapuServisi.Clear();
                        bakilanSonTapuServisi.Append("_takbisService.GetirHisseByTakbisTasinmazIdAsync -> " + JsonConvert.SerializeObject(new GetirHisseTasinmazIdDenQueryModel
                        {
                            TakbisTasinmazId = (int)bagimsizBolum.Id,
                            TapuBolumDurum = TapuBolumDurumEnum.Hepsi.ToString(),
                        }));

                        var hisseListe = await _takbisService.GetirHisseByTakbisTasinmazIdAsync(new GetirHisseTasinmazIdDenQueryModel
                        {
                            TakbisTasinmazId = (int)bagimsizBolum.Id,
                            TapuBolumDurum = TapuBolumDurumEnum.Hepsi.ToString(),
                        });
                        //47- MALİYE HAZİNESİ Malik ID Değeri
                        await Parallel.ForEachAsync(hisseListe.Where(x => x.TapuBolumDurum == TapuBolumDurumEnum.Aktif
                        || (x.TapuBolumDurum == TapuBolumDurumEnum.Pasif && x.MalikId == 47)).ToList(), parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = 100 }, async (hisse, cancellationToken) =>
                        //foreach(var hisse in hisseListe)
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            if (!basvuruListe.Any(w => w.TcKimlikNo == hisse.MalikTCNo.ToString() && w.TapuTasinmazId == (int)hisse.TasinmazId))
                            {
                                bool gercekKisiMi = hisse.MalikTip == TakbisMalikTipEnum.GercekKisi;
                                if (hisse.MalikTip == TakbisMalikTipEnum.TuzelKisi)
                                {
                                    hisse.MalikAd = hisse.MalikUnvan;
                                    hisse.MalikSoyad = hisse.MalikUnvan;
                                }

                                var malikTcNo = hisse.MalikTCNo?.ToString() ?? "";
                                var basvuruKamuUstlenecekVeri = basvuruKamuUstlenecekListe.FirstOrDefault(x => x.TcKimlikNo == malikTcNo && x.TuzelKisiVergiNo == hisse.MalikVergiNo && x.TuzelKisiAdi == hisse.MalikUnvan && x.TapuAnaTasinmazId == (int)hisse.Id && (x.TapuTasinmazId == 0 || x.TapuTasinmazId == (int)hisse.TasinmazId) && ((!gercekKisiMi && x.TuzelKisiTipId != null) || (gercekKisiMi && x.TuzelKisiTipId == null)));
                                //basvuru ve kamuUstlenecek tablosunda yok ise eklenecek
                                if (basvuruKamuUstlenecekVeri == null)
                                {
                                    var basvuruKamuUstlenecek = new BasvuruKamuUstlenecek
                                    {
                                        BasvuruAfadDurumId = (long)BasvuruAfadDurumEnum.BasvuruYok,
                                        BasvuruDurumId = (long)BasvuruDurumEnum.BasvurunuzDegerlendirmeAsamasindadir,
                                        BasvuruDestekTurId = BasvuruDestekTurEnum.HibeVeKredi,
                                        BasvuruTurId = gercekKisiMi ? BasvuruTurEnum.Konut : BasvuruTurEnum.Ticarethane,
                                        TuzelKisiTipId = gercekKisiMi ? null : 1,
                                        AktifMi = true,
                                        SilindiMi = false,
                                        Ad = hisse.MalikAd,
                                        Soyad = hisse.MalikSoyad,
                                        TcKimlikNo = malikTcNo,
                                        TuzelKisiVergiNo = hisse.MalikVergiNo,
                                        TuzelKisiAdi = hisse.MalikUnvan,
                                        TapuTasinmazId = (int)hisse.TasinmazId,
                                        TapuAnaTasinmazId = (int)hisse.Id,
                                        TapuAda = basvuru.TapuAda,
                                        TapuParsel = basvuru.TapuParsel,
                                        TapuArsaPay = HisseninArsaIcinPayiniHesapla(basvuru.TapuArsaPay, hisse.Pay, hisse.Payda),
                                        TapuArsaPayda = basvuru.TapuArsaPayda,
                                        TapuMahalleId = basvuru.TapuMahalleId,
                                        TapuMahalleAdi = bagimsizBolum.Mahalle,
                                        TapuKat = kat,
                                        TapuIlceAdi = bagimsizBolum.Ilce,
                                        TapuIlceId = basvuru.TapuIlceId,
                                        TapuIstirakNo = (int)hisse.IstirakNo,
                                        TapuBlok = bagimsizBolum.BagimsizBolum?.Blok,
                                        TapuTasinmazTipi = bagimsizBolum.Tip.ToString(),
                                        TapuBagimsizBolumNo = bagimsizBolum.BagimsizBolum?.No,
                                        TapuNitelik = bagimsizBolum.Nitelik,
                                        TapuIlAdi = bagimsizBolum.Il,
                                        TapuIlId = basvuru.TapuIlId,
                                        UavtMahalleAdi = basvuru.UavtMahalleAdi,
                                        UavtIlAdi = basvuru.UavtIlAdi,
                                        UavtIlNo = basvuru.UavtIlNo,
                                        UavtIlKodu = basvuru.UavtIlKodu,
                                        UavtIlceAdi = bagimsizBolum.Ilce,
                                        UavtIlceNo = basvuru.UavtIlceNo,
                                        UavtIlceKodu = basvuru.UavtIlceKodu,
                                        UavtMahalleNo = basvuru.UavtMahalleNo,
                                        UavtMahalleKodu = basvuru.UavtMahalleKodu,
                                        OlusturmaTarihi = DateTime.Now,
                                        OlusturanKullaniciId = 1
                                    };

                                    basvuruKamuUstlenecekListeEklenecek.Add(basvuruKamuUstlenecek);

                                    basvuru.BasvuruKamuUstlenecekGuncellemeTarihi = datetimeNow;
                                    basvuruListeGuncellenecek.Add(basvuru);
                                }
                                else
                                {
                                    //kamu üstlenecek tablosundaki veri güncellenecek
                                    basvuruKamuUstlenecekVeri.Ad = hisse.MalikAd;
                                    basvuruKamuUstlenecekVeri.Soyad = hisse.MalikSoyad;
                                    basvuruKamuUstlenecekVeri.BasvuruTurId = gercekKisiMi ? BasvuruTurEnum.Konut : BasvuruTurEnum.Ticarethane;
                                    basvuruKamuUstlenecekVeri.TuzelKisiAdi = hisse.MalikUnvan;
                                    basvuruKamuUstlenecekVeri.TuzelKisiTipId = gercekKisiMi ? null : 1;
                                    basvuruKamuUstlenecekVeri.TapuAnaTasinmazId = (int)hisse.Id;
                                    basvuruKamuUstlenecekVeri.TapuTasinmazId = (int)hisse.TasinmazId;
                                    basvuruKamuUstlenecekVeri.GuncellemeTarihi = DateTime.Now;
                                    basvuruKamuUstlenecekVeri.GuncelleyenKullaniciId = 1;

                                    basvuruKamuUstlenecekListeGuncellenecek.Add(basvuruKamuUstlenecekVeri);

                                    basvuru.BasvuruKamuUstlenecekGuncellemeTarihi = datetimeNow;
                                    basvuruListeGuncellenecek.Add(basvuru);
                                }
                            }
                        }
                        );
                    }
                    //);
                }
                //);

                //güncellenmeyen veya daha önceden güncellenip şuan hissedar olarak gelmeyen kamu üstlenecekleri sil
                foreach (var basvuruKamuUstlenecekSilinecek in basvuruKamuUstlenecekListe.Where(x => x.PasifMaliyeHazinesiMi != true && (x.GuncellemeTarihi == null || x.GuncellemeTarihi < metotCalismaBaslangicTarihi)))
                {
                    basvuruKamuUstlenecekSilinecek.AktifMi = false;
                    basvuruKamuUstlenecekSilinecek.SilindiMi = true;
                    basvuruKamuUstlenecekListeGuncellenecek.Add(basvuruKamuUstlenecekSilinecek);
                }

                var veritabaniDevKullaniciAktifSaatteMi = DateTime.Now.Hour < 9 || DateTime.Now.Hour >= 17;

                if (request?.JobMuCalisiyor == true && veritabaniDevKullaniciAktifSaatteMi)
                {
                    if (basvuruKamuUstlenecekListeEklenecek.Any())
                        _basvuruKamuUstlenecekRepository.BulkInsert(basvuruKamuUstlenecekListeEklenecek);

                    if (basvuruKamuUstlenecekListeGuncellenecek.Any())
                        await _basvuruKamuUstlenecekRepository.BulkUpdate(basvuruKamuUstlenecekListeGuncellenecek, CancellationToken.None);

                    //yalnızca ilgili kolonlar update edilecek
                    var bulkConfig = new BulkConfig
                    {
                        TrackingEntities = false,
                        UpdateByProperties = new List<string> {
                            nameof(Basvuru.BasvuruKamuUstlenecekGuncellemeTarihi)
                        }
                    };

                    await _basvuruRepository.BulkUpdate(basvuruListe.Where(x => x.BasvuruKamuUstlenecekGuncellemeTarihi >= metotCalismaBaslangicTarihi), CancellationToken.None);

                    sb.AppendLine(string.Format("{0} Update işlemi tamamlandı.", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                }
                else
                {
                    if (basvuruKamuUstlenecekListeEklenecek.Any())
                        await _basvuruKamuUstlenecekRepository.AddRangeAsync(basvuruKamuUstlenecekListeEklenecek);

                    if (basvuruKamuUstlenecekListeGuncellenecek.Any())
                        _basvuruKamuUstlenecekRepository.UpdateRange(basvuruKamuUstlenecekListeGuncellenecek);

                    await _basvuruKamuUstlenecekRepository.SaveChanges();

                    _basvuruRepository.UpdateRange(basvuruListe.Where(x => x.BasvuruKamuUstlenecekGuncellemeTarihi >= metotCalismaBaslangicTarihi));
                    await _basvuruRepository.SaveChanges();
                }

                result.Result = sb.ToString();
            }
            catch (Exception ex)
            {
                result.Result = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} İşlem sırasında bir hata meydana geldi. Bakılan Son Başvuru Id: {bakilanSonBasvuruId}, Bakılan Son Tapu Servisi: {bakilanSonTapuServisi}, Exception: {ex}";
            }

            return result;
        }

        private long? HisseninArsaIcinPayiniHesapla(long? arsaPay, string hissePay, string hissePayda)
        {
            if (arsaPay == null) return null;

            try
            {
                return (long)Math.Round((arsaPay ?? 0) * (double.Parse(hissePay.Replace(".000", "")) / double.Parse(hissePayda.Replace(".000", ""))), 3);
            }
            catch { }

            return null;
        }

    }
}