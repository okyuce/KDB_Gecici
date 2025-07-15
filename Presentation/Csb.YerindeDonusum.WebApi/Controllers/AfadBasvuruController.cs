using Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirAfadBasvuruById;
using Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirListeAfadBasvuruIl;
using Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirListeAfadBasvuruIlce;
using Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirListeAfadBasvuruMahalle;
using Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirListeAfadBasvuruServerSide;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using CSB.Core.LogHandler.Attr;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AfadBasvuruController : ControllerBase
{
    private readonly IMediator _mediator;

    public AfadBasvuruController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetirListeIl")]
    public async Task<ResultModel<List<SelectDto<int>>>> GetirListeIl([FromQuery] GetirListeAfadBasvuruIlQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirListeIlce")]
    public async Task<ResultModel<List<SelectDto<int>>>> GetirListeIlce([FromQuery] GetirListeAfadBasvuruIlceQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirListeMahalle")]
    public async Task<ResultModel<List<SelectDto<int>>>> GetirListeMahalle([FromQuery] GetirListeAfadBasvuruMahalleQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirById")]
    [AddInfoLog]
    public async Task<ResultModel<GetirAfadBasvuruByIdQueryResponseModel>> GetirById([FromQuery] GetirAfadBasvuruByIdQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("GetirListeServerSide")]
    [AddInfoLog(PassResponse =true)]
    public async Task<ResultModel<DataTableResponseModel<List<GetirListeAfadBasvuruServerSideQueryResponseModel>>>> GetirListeServerSide([FromBody] GetirListeAfadBasvuruServerSideQuery request)
    {
        return await _mediator.Send(request);
    }
}