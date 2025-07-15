using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.GuncelleBasvuruAfadDurum;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.GuncelleTopluBasvuruAfadDurum;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.GuncelleTopluDegisenBasvuruAfadDurum;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.KaydetAfadBasvuru;
using Csb.YerindeDonusum.Application.Interfaces.BackgroundJobInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Hangfire;
using MediatR;

namespace Csb.YerindeDonusum.BackgroundJob.Hangfire.RecurringJobs;

public class BasvuruAfadJobManagers : IHangfireBasvuruAfadJob
{
    private readonly IMediator _mediator;

    public BasvuruAfadJobManagers(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ResultModel<string>> BasvuruAfadDurumUpdate(IJobCancellationToken jobCancellationToken)
    {
        return await _mediator.Send(new GuncelleBasvuruAfadDurumCommand());
    }

    public async Task<ResultModel<string>> BasvuruAfadDurumTopluUpdate(IJobCancellationToken jobCancellationToken)
    {
        return await _mediator.Send(new GuncelleTopluBasvuruAfadDurumCommand());
    }

    public async Task<ResultModel<string>> BasvuruAfadDurumTopluDegisenUpdate(IJobCancellationToken jobCancellationToken)
    {
        return await _mediator.Send(new GuncelleTopluDegisenBasvuruAfadDurumCommand());
    }

    public async Task<ResultModel<string>> KaydetAfadBasvuru(IJobCancellationToken jobCancellationToken)
    {
        return await _mediator.Send(new KaydetAfadBasvuruCommand());
    }
}