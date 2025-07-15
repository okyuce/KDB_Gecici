using Csb.YerindeDonusum.Application.Models;
using Hangfire;

namespace Csb.YerindeDonusum.Application.Interfaces.BackgroundJobInterfaces;

public interface IHangfireBasvuruHissePayPaydaUpdateJob
{
    Task<ResultModel<string>> BasvuruHissePayPaydaUpdateJob(IJobCancellationToken jobCancellationToken);
}
