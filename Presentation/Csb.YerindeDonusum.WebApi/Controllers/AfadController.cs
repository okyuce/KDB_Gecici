using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirBasvuruListeAfadIcin;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirBasvuruListeByTcNoAfadIcin;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirBasvuruListeDegisenAfadIcin;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirRezervAlanListeAfadIcin;
using Csb.YerindeDonusum.Application.Models;
using CSB.Core.LogHandler.Attr;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AfadController : ControllerBase
{
    private readonly IMediator _mediator;

    public AfadController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetirBasvuruListeByTcNo")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<List<GetirAfadIcinBasvuruDetayDto>>> GetirBasvuruListeByTcNo(string tcKimlikNo)
    {
        return await _mediator.Send(new GetirBasvuruListeByTcNoAfadIcinQuery() { TcKimlikNo = tcKimlikNo });
    }

    [HttpPost("GetirBasvuruListe")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirBasvuruListeAfadIcinResponseModel>> GetirBasvuruListe(GetirBasvuruListeAfadIcinQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("GetirBasvuruListeDegisen")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirBasvuruListeDegisenAfadIcinResponseModel>> GetirBasvuruListeDegisen(GetirBasvuruListeDegisenAfadIcinQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirRezervAlanListe")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirRezervAlanListeAfadIcinQueryResponseModel>> GetirRezervAlanListe(int offset)
    {
        return await _mediator.Send(new GetirRezervAlanListeAfadIcinQuery { Offset = offset });
    }
}