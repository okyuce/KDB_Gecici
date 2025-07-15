using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazByTakbisTasinmazId;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.BackgroundJobInterfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

namespace Csb.YerindeDonusum.BackgroundJob.Hangfire.RecurringJobs;

public class BasvuruTapuPayPaydaJobManagers : IHangfireBasvuruHissePayPaydaUpdateJob
{
    private readonly IBasvuruRepository _basvuruRepository;
    private readonly IBasvuruTapuBilgiRepository _basvuruTapuBilgiRepository;
    private readonly ITakbisService _takbisService;

    public BasvuruTapuPayPaydaJobManagers(ITakbisService takbisService, IBasvuruRepository basvuruRepository, IBasvuruTapuBilgiRepository basvuruTapuBilgiRepository)
    {
        _takbisService = takbisService;
        _basvuruRepository = basvuruRepository;
        _basvuruTapuBilgiRepository = basvuruTapuBilgiRepository;
    }

    /// <summary>
    /// BasvuruTapuPayPaydaNullKayitKontrolJob; Hangfire job'i ile basvuru_tapu_bilgi tablosunda pay payda bilgisi 0 olan kayitlari 
    /// kontrol edip ve takbis uzerinden sorgulayip db uzerinde guncellemektedir.
    /// </summary>
    /// <returns></returns>

    public async Task<ResultModel<string>> BasvuruHissePayPaydaUpdateJob(IJobCancellationToken jobCancellationToken)
    {
        ResultModel<string> result = new();

        try
        {
            StringBuilder sb = new();
            sb.AppendFormat("{0} BasvuruTapuPayPaydaNullKayitKontrolJob çalışmaya başladı. ", DateTime.Now);

            var paypaydasiBosOlanKayitlar = await _basvuruRepository.GetAllQueryable(x => 
                           x.AktifMi == true 
                        && x.SilindiMi == false
                        && x.TapuTasinmazId != null 
                        && x.TuzelKisiTipId == null
                        && x.BasvuruTapuBilgis.Any(m => m.HissePay == 0 && m.HissePayda == 0)
                    ).Include(x => x.BasvuruTapuBilgis).Select(x => new { 
                            BasvuruId = x.BasvuruId,
                            TasinmazId = x.TapuTasinmazId,
                            IstirakNo = x.TapuIstirakNo,
                            TCKimlikNo = x.TcKimlikNo,
                            TapuPayPayda = x.BasvuruTapuBilgis                             
                    }).ToListAsync();

            if (!paypaydasiBosOlanKayitlar.Any())
            {
                sb.AppendFormat("{0} BasvuruTapuPayPaydaNullKayitKontrolJob güncellenecek başvuru kaydı bulunamadığı için işlem yapmadan tamamlandı. ", DateTime.Now);

                result.Result = sb.ToString();
                return result;
            }

            Stopwatch stopwatchBasvuruJob = new();

            List<BasvuruTapuBilgi> dbHisseUpdateList = new();

            foreach (var paypayda in paypaydasiBosOlanKayitlar)
            {
                try
                {
                    var hissemodel = new GetirHisseTasinmazIdDenQueryModel();
                    hissemodel.TapuBolumDurum = "Aktif";
                    hissemodel.TakbisTasinmazId = paypayda.TasinmazId.Value;

                    var takbisHisseBilgi = await _takbisService.GetirHisseByTakbisTasinmazIdAsync(hissemodel);
                    var tcyeaithisseler = takbisHisseBilgi.Where(x => x.MalikTCNo?.ToString() == paypayda.TCKimlikNo).ToList();                    

                    foreach (var hisse in tcyeaithisseler)
                    {
                        var dbHisse = await _basvuruTapuBilgiRepository.GetWhere(x =>x.BasvuruId == paypayda.BasvuruId &&  x.YevmiyeNo == hisse.YevmiyeNo && x.IstirakNo == hisse.IstirakNo, false).ToListAsync();
                        
                        if(dbHisse.Count!= 1)
                        {
                            sb.AppendFormat($"{dbHisse.FirstOrDefault()?.BasvuruId} Id li başvuru için (Taşınmaz ID : {paypayda.TasinmazId}) {dbHisse.Count} adet kayıt bulundu.");
                            continue;
                        }
                        
                        dbHisse.First().HissePay = Convert.ToInt64(hisse.Pay.Replace(".000",""));
                        dbHisse.First().HissePayda = Convert.ToInt64(hisse.Payda.Replace(".000", ""));

                        dbHisseUpdateList.Add(dbHisse.First());
                    }                }
                catch (Exception ex)
                {
                    sb.AppendFormat($"Takbis sorgulamasında hata oluştu. Başvuru ID: {paypayda.BasvuruId} Taşınmaz ID : {paypayda.TasinmazId}. Hata Detayı : " +ex.Message);
                }

            }
            stopwatchBasvuruJob.Restart();
            await _basvuruTapuBilgiRepository.BulkUpdate(dbHisseUpdateList, CancellationToken.None);
            stopwatchBasvuruJob.Stop();

            long bulkUpdateGecenMilisaniye = stopwatchBasvuruJob.ElapsedMilliseconds;

            sb.AppendFormat($"Update islemi {bulkUpdateGecenMilisaniye} ms icerisinde tamamlandi. Toplam kayit sayisi: {paypaydasiBosOlanKayitlar.Count}");

            sb.AppendFormat($"{DateTime.Now} BasvuruTapuPayPaydaNullKayitKontrolJob {paypaydasiBosOlanKayitlar.Count} adet (başvuru sayısı)  için çalışma tamamlandı.");

            result.Result = sb.ToString();
        }
        catch (Exception ex)
        {
            result.Result = string.Concat("İşlem sırasında bir hata meydana geldi", ex.InnerException);
        }

        return result;
    }
}