using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazByTakbisTasinmazId;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.BackgroundJobInterfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.Takbis;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Csb.YerindeDonusum.BackgroundJob.Hangfire.RecurringJobs;

public class BasvuruTapuSahiplikOraniUpdateJobManagers : IHangfireBasvuruTapuSahiplikOraniUpdateJob
{
    private readonly IBasvuruRepository _basvuruRepository;
    private readonly ITakbisService _takbisService;

    public BasvuruTapuSahiplikOraniUpdateJobManagers(ITakbisService takbisService, IBasvuruRepository basvuruRepository)
    {
        _takbisService = takbisService;
        _basvuruRepository = basvuruRepository;
    }

    /// <summary>
    /// BasvuruTapuSahiplikNullKayitKontrolJob; Hangfire job'i ile basvuru tablosunda tapu_bina_sahiplik_orani bilgisi null olan kayitlari 
    /// kontrol edip ve takbis uzerinden sorgulayip db uzerinde guncellemektedir.
    /// </summary>
    /// <returns></returns>
    public async Task<ResultModel<string>> BasvuruTapuSahiplikNullKayitKontrolJob(IJobCancellationToken jobCancellationToken)
    {
        ResultModel<string> result = new();

        try
        {
            StringBuilder sb = new();
            sb.AppendFormat("{0} BasvuruTapuSahiplikNullKayitKontrolJob çalışmaya başladı. ", DateTime.Now);

            var sahiplikOraniBosOlanBasvurular = await _basvuruRepository.GetAllQueryable(x =>
                        x.AktifMi == true
                        &&
                        x.SilindiMi == false
                        &&
                        x.TapuMahalleId > 0
                        && 
                        x.TapuToplamBagimsizBolumSayisi == null
                )
                .OrderBy(o => o.BasvuruId)
                .Take(10000)
                .ToListAsync();

            if (!sahiplikOraniBosOlanBasvurular.Any())
            {
                sb.AppendFormat("{0} BasvuruTapuSahiplikNullKayitKontrolJob güncellenecek başvuru kaydı bulunamadığı için işlem yapmadan tamamlandı. ", DateTime.Now);

                result.Result = sb.ToString();
                return result;
            }

            await Parallel.ForEachAsync(sahiplikOraniBosOlanBasvurular, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = 5 }, async (basvuru, cancellationToken) =>
            {
                jobCancellationToken.ThrowIfCancellationRequested();
                try
                {
                    var bagimsizBolumQuery = new GetirBagimsizBolumModel
                    {
                        AdaNo = basvuru.TapuAda,
                        ParselNo = basvuru.TapuParsel,
                        TapuBolumDurum = TapuBolumDurumEnum.Aktif,
                        MahalleIds = new decimal[] { basvuru.TapuMahalleId!.Value },
                        Blok = basvuru.TapuBlok
                    };
                    var bagimsizBolumResult = await _takbisService.GetirBagimsizBolumAsync(bagimsizBolumQuery);

                    var bagimsizBolumHisseOranList = new List<decimal>();
                    await Parallel.ForEachAsync(bagimsizBolumResult, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = 5 }, async (bagimsizBolum, cancellationToken2) =>
                    {
                        jobCancellationToken.ThrowIfCancellationRequested();
                        var tumHisseler = await _takbisService.GetirHisseByTakbisTasinmazIdAsync(new GetirHisseTasinmazIdDenQueryModel
                        {
                            TakbisTasinmazId = (int)bagimsizBolum.Id,
                            TapuBolumDurum = TapuBolumDurumEnum.Aktif.ToString(),
                        });

                        if (tumHisseler.Count() > 0)
                        {
                            var kisiyeAitHisseler = tumHisseler.Where(x => x.MalikTCNo?.ToString() == basvuru.TcKimlikNo && x.Pay != "0.000");

                            //bağımsız bölüm bir daire ama hisseli ortaklı olabilir. o dairenin ne kadarı bu adamın diye aşağıda hesaplama yapılıyor.
                            //0 ile 1 arası bir değer çıkacak.
                            //hesaplanan 0 ile 1 arasındaki değer her bağımsız bölüm için olacağı için listeye alınıyor. 30 daire varsa ve her dairenin tamamı
                            //aynı kişininse bu listedeki 30 itemin değeri 1 olacak ve toplam 30 olacak.
                            //yani bagimsizBolumHisseOranList listesini sum edip bagimsizBolumResult.count ile oranına bakacaz gibi. aynı ise %100,
                            //yarısı ise %50 gibi oran çıkacak
                            decimal hisseOrani = kisiyeAitHisseler.Sum(s => decimal.Parse(s.Pay.Replace(".000", "")) / decimal.Parse(s.Payda.Replace(".000", "")));
                            bagimsizBolumHisseOranList.Add(hisseOrani);
                        }
                    });

                    basvuru.TapuToplamKisiBagimsizBolumSayisi = bagimsizBolumHisseOranList.Where(x => x > 0).Count();
                    basvuru.TapuToplamKisiHisseOrani = bagimsizBolumHisseOranList.Sum();
                    basvuru.TapuToplamBagimsizBolumSayisi = bagimsizBolumResult.Count();
                }
                catch (Exception ex)
                {
                    sb.AppendFormat($"Takbis sorgulamasında hata oluştu. Başvuru ID: {basvuru.BasvuruId} Taşınmaz ID : {basvuru.TapuAnaTasinmazId}. Hata Detayı : " + ex.Message);
                }
            });

            await _basvuruRepository.BulkUpdate(sahiplikOraniBosOlanBasvurular.Where(x=> x.TapuToplamKisiBagimsizBolumSayisi != null), CancellationToken.None);
            
            sb.AppendFormat($"Update islemi tamamlandi. Toplam kayit sayisi: {sahiplikOraniBosOlanBasvurular.Count}");
            sb.AppendFormat($"{DateTime.Now} BasvuruTapuSahiplikNullKayitKontrolJob {sahiplikOraniBosOlanBasvurular.Count} adet (başvuru sayısı)  için çalışma tamamlandı.");

            result.Result = sb.ToString();
        }
        catch (Exception ex)
        {
            result.Result = string.Concat("İşlem sırasında bir hata meydana geldi", ex.InnerException);
        }

        return result;
    }
}