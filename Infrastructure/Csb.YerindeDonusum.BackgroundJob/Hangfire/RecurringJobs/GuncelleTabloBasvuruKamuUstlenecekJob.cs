using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.GuncelleBasvuruAfadDurum;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.BackgroundJobInterfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Csb.YerindeDonusum.BackgroundJob.Hangfire.RecurringJobs;

public class GuncelleTabloBasvuruKamuUstlenecekJobManagers : IHangfireGuncelleTabloBasvuruKamuUstlenecekJob
{
    private readonly IMediator _mediator;

    public GuncelleTabloBasvuruKamuUstlenecekJobManagers(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ResultModel<string>> GuncelleTabloBasvuruKamuUstlenecekJob(IJobCancellationToken jobCancellationToken)
    {
        return await _mediator.Send(new GuncelleTabloBasvuruKamuUstlenecekJob()); 
    }
}