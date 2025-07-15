using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.GuncelleBasvuruAfadDurum;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.GuncelleTopluBasvuruAfadDurum;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.GuncelleTopluDegisenBasvuruAfadDurum;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.KaydetAfadBasvuru;
using Csb.YerindeDonusum.Application.Interfaces.BackgroundJobInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Hangfire;
using MediatR;

namespace Csb.YerindeDonusum.BackgroundJob.Hangfire.RecurringJobs;

public class BasvuruHuIdJobManagers : IHangfireBasvuruHuIdJob
{
    private readonly IMediator _mediator;

    public BasvuruHuIdJobManagers(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ResultModel<string>> BasvuruHuIdTopluUpdate(IJobCancellationToken jobCancellationToken)
    {
        return await _mediator.Send(new GuncelleTopluBasvuruHuIdCommand());
    }
}