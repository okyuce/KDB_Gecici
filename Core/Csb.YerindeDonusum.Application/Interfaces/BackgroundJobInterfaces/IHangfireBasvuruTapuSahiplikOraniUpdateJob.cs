using Csb.YerindeDonusum.Application.Models;
using Hangfire;

namespace Csb.YerindeDonusum.Application.Interfaces.BackgroundJobInterfaces;

public interface IHangfireBasvuruTapuSahiplikOraniUpdateJob
{
    Task<ResultModel<string>> BasvuruTapuSahiplikNullKayitKontrolJob(IJobCancellationToken jobCancellationToken);
}
