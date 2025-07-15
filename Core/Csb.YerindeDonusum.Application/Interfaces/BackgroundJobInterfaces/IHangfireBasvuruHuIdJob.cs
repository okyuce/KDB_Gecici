using Csb.YerindeDonusum.Application.Models;
using Hangfire;

namespace Csb.YerindeDonusum.Application.Interfaces.BackgroundJobInterfaces;

public interface IHangfireBasvuruHuIdJob
{
    Task<ResultModel<string>> BasvuruHuIdTopluUpdate(IJobCancellationToken jobCancellationToken);
}
