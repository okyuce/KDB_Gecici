using Csb.YerindeDonusum.Application.Models;
using Hangfire;

namespace Csb.YerindeDonusum.Application.Interfaces.BackgroundJobInterfaces;

public interface IHangfireBasvuruArsaPayPaydaUpdateJob
{
    Task<ResultModel<string>> BasvuruTapuTasinmazArsaPayPaydaDoldurmaJob(IJobCancellationToken jobCancellationToken);
}
