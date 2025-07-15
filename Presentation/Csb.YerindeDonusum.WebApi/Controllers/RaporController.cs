using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeYapiRuhsatRapor;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirBinaOdemeRapor;
using Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.GetirListeBinabazliOranServerSide;
using Csb.YerindeDonusum.Application.Enums;
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
public class RaporController : ControllerBase
{
    private readonly IMediator _mediator;

    public RaporController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("GetirListeBinabazliOranServerSide")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.Raporlama}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<DataTableResponseModel<List<GetirListeBinabazliOranServerSideQueryResponseModel>>>> GetirListeBinabazliOranServerSide([FromBody] GetirListeBinabazliOranServerSideQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirListeYapiRuhsat")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.Raporlama}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<List<GetirListeYapiRuhsatRaporQueryResponseModel>>> GetirListeYapiRuhsat([FromQuery] GetirListeYapiRuhsatRaporQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirListeBinaOdeme")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.Raporlama}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<List<GetirListeBinaOdemeRaporQueryResponseModel>>> GetirListeBinaOdeme([FromQuery] GetirListeBinaOdemeRaporQuery request)
    {
        return await _mediator.Send(request);
    }
}