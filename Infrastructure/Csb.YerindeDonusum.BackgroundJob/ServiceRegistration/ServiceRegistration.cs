using Csb.YerindeDonusum.Application.Interfaces.BackgroundJobInterfaces;
using Csb.YerindeDonusum.BackgroundJob.Hangfire.RecurringJobs;
using Microsoft.Extensions.DependencyInjection;

namespace Csb.YerindeDonusum.BackgroundJob.ServiceRegistration;

public static class ServiceRegistration
{
    public static IServiceCollection AddBackgroundJobServiceRegistration(this IServiceCollection serviceProviders)
    {
        serviceProviders.AddScoped<IHangfireBasvuruAfadJob, BasvuruAfadJobManagers>();
        serviceProviders.AddScoped<IHangfireBasvuruHuIdJob, BasvuruHuIdJobManagers>();
        serviceProviders.AddScoped<IHangfireBasvuruHissePayPaydaUpdateJob, BasvuruTapuPayPaydaJobManagers>();
        serviceProviders.AddScoped<IHangfireBasvuruArsaPayPaydaUpdateJob, BasvuruTapuTasinmazArsaPayPaydaJobManagers>();
        serviceProviders.AddScoped<IHangfireBasvuruTapuSahiplikOraniUpdateJob, BasvuruTapuSahiplikOraniUpdateJobManagers>();
        serviceProviders.AddScoped<IHangfireGuncelleTabloBasvuruKamuUstlenecekJob, GuncelleTabloBasvuruKamuUstlenecekJobManagers>();
        return serviceProviders;
    }
}