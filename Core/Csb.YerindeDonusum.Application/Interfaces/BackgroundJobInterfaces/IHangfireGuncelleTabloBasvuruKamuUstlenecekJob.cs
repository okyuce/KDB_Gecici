using Csb.YerindeDonusum.Application.Models;
using Hangfire;

namespace Csb.YerindeDonusum.Application.Interfaces.BackgroundJobInterfaces;

public interface IHangfireGuncelleTabloBasvuruKamuUstlenecekJob
{
    Task<ResultModel<string>> GuncelleTabloBasvuruKamuUstlenecekJob(IJobCancellationToken jobCancellationToken);
}
