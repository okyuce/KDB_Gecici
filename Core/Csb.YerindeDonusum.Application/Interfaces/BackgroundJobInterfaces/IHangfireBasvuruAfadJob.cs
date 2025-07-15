using Csb.YerindeDonusum.Application.Models;
using Hangfire;

namespace Csb.YerindeDonusum.Application.Interfaces.BackgroundJobInterfaces;

public interface IHangfireBasvuruAfadJob
{
    Task<ResultModel<string>> BasvuruAfadDurumUpdate(IJobCancellationToken jobCancellationToken);
    Task<ResultModel<string>> BasvuruAfadDurumTopluUpdate(IJobCancellationToken jobCancellationToken);
    Task<ResultModel<string>> BasvuruAfadDurumTopluDegisenUpdate(IJobCancellationToken jobCancellationToken);
    Task<ResultModel<string>> KaydetAfadBasvuru(IJobCancellationToken jobCancellationToken);
}
