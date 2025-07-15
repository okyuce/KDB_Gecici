using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazByTakbisTasinmazId;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.BackgroundJobInterfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

namespace Csb.YerindeDonusum.BackgroundJob.Hangfire.RecurringJobs;

public class BasvuruTapuTasinmazArsaPayPaydaJobManagers : IHangfireBasvuruArsaPayPaydaUpdateJob
{
    private readonly IBasvuruRepository _basvuruRepository;
    private readonly IBasvuruTapuBilgiRepository _basvuruTapuBilgiRepository;
    private readonly ITakbisService _takbisService;

    public BasvuruTapuTasinmazArsaPayPaydaJobManagers(ITakbisService takbisService, IBasvuruRepository basvuruRepository, IBasvuruTapuBilgiRepository basvuruTapuBilgiRepository)
    {
        _takbisService = takbisService;
        _basvuruRepository = basvuruRepository;
        _basvuruTapuBilgiRepository = basvuruTapuBilgiRepository;
    }

    /// <summary>
    /// BasvuruTapuTasinmazArsaPayPaydaJobManagers; Hangfire job'i ile basvuru tablosundaki tasinmaz id ile takbis uzerinden arsa pay payda bilgisini
    /// sorgulayip db uzerinde guncellemektedir.
    /// </summary>
    /// <returns></returns>
    public async Task<ResultModel<string>> BasvuruTapuTasinmazArsaPayPaydaDoldurmaJob(IJobCancellationToken jobCancellationToken)
    {
        ResultModel<string> result = new();

        try
        {
            StringBuilder sb = new();
            sb.AppendFormat("{0} BasvuruTapuTasinmazArsaPayPaydaDoldurmaJob çalışmaya başladı. ", DateTime.Now);

            var arsaPaypaydasiBosOlanKayitlar = await _basvuruRepository.GetWhere(x =>
                        x.TapuTasinmazId != null
                    &&
                    x.TapuTasinmazTipi!="Ana Taşınmaz"
                    &&
                        x.TapuArsaPay == null
                    &&
                        x.TapuArsaPayda == null
                    &&
                        x.AktifMi == true
                    &&
                        x.SilindiMi == false
                    , false)
                    .ToListAsync();

            if (!arsaPaypaydasiBosOlanKayitlar.Any())
            {
                sb.AppendFormat("{0} BasvuruTapuTasinmazArsaPayPaydaDoldurmaJob güncellenecek başvuru kaydı bulunamadığı için işlem yapmadan tamamlandı. ", DateTime.Now);

                result.Result = sb.ToString();
                return result;
            }
            Stopwatch stopwatchBasvuruJob = new();


            //foreach (var basvuru in arsaPaypaydasiBosOlanKayitlar)
            await Parallel.ForEachAsync(arsaPaypaydasiBosOlanKayitlar, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = 5 }, async (basvuru, cancellationToken) =>
            {
                jobCancellationToken.ThrowIfCancellationRequested();
                try
                {
                    var tasinmazmodel = new GetirTasinmazByTakbisTasinmazIdQueryModel();
                    tasinmazmodel.TakbisTasinmazId = basvuru.TapuTasinmazId.Value;

                    var tasinmazBilgi = await _takbisService.GetirTasinmazByTakbisTasinmazIdAsync(tasinmazmodel);
                    basvuru.TapuArsaPay = tasinmazBilgi.BagimsizBolum == null ? null : Convert.ToInt64(tasinmazBilgi.BagimsizBolum.ArsaPay);
                    basvuru.TapuArsaPayda = tasinmazBilgi.BagimsizBolum == null ? null : Convert.ToInt64(tasinmazBilgi.BagimsizBolum.ArsaPayda);

                }
                catch (Exception ex)
                {
                    sb.AppendFormat($"Takbis sorgulamasında hata oluştu. Başvuru ID: {basvuru.BasvuruId} Taşınmaz ID : {basvuru.TapuTasinmazId}. Hata Detayı : " + ex.Message);
                }
            });
            stopwatchBasvuruJob.Restart();
            await _basvuruRepository.BulkUpdate(arsaPaypaydasiBosOlanKayitlar, CancellationToken.None);
            stopwatchBasvuruJob.Stop();

            long bulkUpdateGecenMilisaniye = stopwatchBasvuruJob.ElapsedMilliseconds;

            sb.AppendFormat($"Update islemi {bulkUpdateGecenMilisaniye} ms icerisinde tamamlandi. Toplam kayit sayisi: {arsaPaypaydasiBosOlanKayitlar.Count}");

            sb.AppendFormat($"{DateTime.Now} BasvuruTapuTasinmazArsaPayPaydaDoldurmaJob {arsaPaypaydasiBosOlanKayitlar.Count} adet (başvuru sayısı)  için çalışma tamamlandı.");

            result.Result = sb.ToString();
        }
        catch (Exception ex)
        {
            result.Result = string.Concat("İşlem sırasında bir hata meydana geldi", ex.InnerException);
        }

        return result;
    }
}