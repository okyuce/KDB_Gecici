using Csb.YerindeDonusum.Application.Interfaces.BackgroundJobInterfaces;
using Hangfire;

namespace Csb.YerindeDonusum.BackgroundJob.Hangfire.Schedules;

public static class RecurringJobInit
{
    public static void Init(bool isProduction)
    {
        if (isProduction)
        {
            //cron utc ye göre veriliyor, +3 zaman diliminde olduğumuz düşünülerek 3 saat gerideki saatleri vermemiz gerekmektedir.

            // At (Development -> 04:30, Production -> 07:30 + 12:30 + 23:30)
            //RecurringJob.AddOrUpdate<IHangfireBasvuruAfadJob>("BasvuruAfadDurumTopluUpdate", (x) => x.BasvuruAfadDurumTopluUpdate(JobCancellationToken.Null), isProduction ? "30 4,9,20 * * *" : "30 1 * * *", new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });

            // At 04:30, 19:30
            //25.05.2024 kapatıldı, gerekliyse açılabilir
            //RecurringJob.AddOrUpdate<IHangfireBasvuruTapuSahiplikOraniUpdateJob>("BasvuruTapuSahiplikOraniUpdateJob", (x) => x.BasvuruTapuSahiplikNullKayitKontrolJob(JobCancellationToken.Null), "30 1,16 * * *", new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });

            // At 23:30 (her gyb 23:30 da calisacak bir job yaziyoruz)
            //25.05.2024 kapatıldı, gerekliyse açılabilir
            //RecurringJob.AddOrUpdate<IHangfireBasvuruHissePayPaydaUpdateJob>("BasvuruTapuPayPaydaNullKayitKontrolJob", (x) => x.BasvuruHissePayPaydaUpdateJob(JobCancellationToken.Null), "30 20 * * *", new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });

            // her 7:10, 8:10, 12:10, 17:10, 18:10, 19:10, 20:10, 01:10 saatlerinde calisacak job yaziyoruz, bulkupdate yaparken dev kullanıcı kullanıldığı için bu saatler ayarlandı)
            //25.05.2024 kapatıldı, gerekliyse açılabilir
            //RecurringJob.AddOrUpdate<IHangfireBasvuruArsaPayPaydaUpdateJob>("BasvuruTapuTasinmazArsaPayPaydaDoldurmaJob", (x) => x.BasvuruTapuTasinmazArsaPayPaydaDoldurmaJob(JobCancellationToken.Null), "10 4,5,9,14,15,16,17,22 * * *", new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });

            //10.06.2024 tarihinde kapatıldı, job ile sorgu sayısı çok oluyordu. başvuru değerlendirmeye tıklanınca sadece ilgili ada parsel için sorgulama yapılacak
            // her 7:55, 12:55, 23:55 saatlerinde calisacak job yaziyoruz, bulkupdate yaparken dev kullanıcı kullanıldığı için bu saatler ayarlandı)
            //RecurringJob.AddOrUpdate<IHangfireGuncelleTabloBasvuruKamuUstlenecekJob>("GuncelleTabloBasvuruKamuUstlenecekJob", (x) => x.GuncelleTabloBasvuruKamuUstlenecekJob(JobCancellationToken.Null), isProduction ? "55 4,20 * * *" : "55 1 * * *", new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });

            // her pazar saat 9 da calisacak job yaziyoruz
            RecurringJob.AddOrUpdate<IHangfireBasvuruHuIdJob>("BasvuruHuIdTopluUpdate", (x) => x.BasvuruHuIdTopluUpdate(JobCancellationToken.Null), "0 9 * * 0", new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });

            #region AFAD
            // her 3 saatte bir calisacak job yaziyoruz
            RecurringJob.AddOrUpdate<IHangfireBasvuruAfadJob>("BasvuruAfadDurumTopluDegisenUpdate", (x) => x.BasvuruAfadDurumTopluDegisenUpdate(JobCancellationToken.Null), "0 */3 * * *", new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });

            // her 2 saatte bir calisacak job yaziyoruz
            RecurringJob.AddOrUpdate<IHangfireBasvuruAfadJob>("KaydetAfadBasvuruJob", (x) => x.KaydetAfadBasvuru(JobCancellationToken.Null), "0 */2 * * *", new RecurringJobOptions { TimeZone = TimeZoneInfo.Utc });
            #endregion
        }
    }
}