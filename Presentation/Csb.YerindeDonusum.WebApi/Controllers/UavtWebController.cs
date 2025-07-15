using Csb.YerindeDonusum.Application.CQRS.BoyutCQRS.Queries.GetirListeBagimsizBolum;
using Csb.YerindeDonusum.Application.CQRS.BoyutCQRS.Queries.GetirListeIl;
using Csb.YerindeDonusum.Application.CQRS.MaksCQRS.Queries.GetirBagimsizBolumByNumaratajUavtKodu;
using Csb.YerindeDonusum.Application.CQRS.MaksCQRS.Queries.GetirNumaratajByCsbmUavtKodu;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UavtWebController : ControllerBase
{
    private readonly IMediator _mediator;

    public UavtWebController(IMediator mediator)
    {
        _mediator = mediator;
    }
  
    [HttpGet("GetirListeIl")]
    public async Task<ResultModel<List<BoyutKonumDto>>> GetirListeIl([FromQuery] GetirListeIlQuery request)
    {
        return await _mediator.Send(request);
    }
  
    [HttpGet("GetirListeDepremIl")]
    public async Task<ResultModel<List<BoyutKonumDto>>> GetirListeDepremIl([FromQuery] GetirListeDepremIlQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirListeIlce")]
    public async Task<ResultModel<List<BoyutKonumDto>>> GetirListeIlce([FromQuery] GetirListeIlceQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirListeMahalle")]
    public async Task<ResultModel<List<BoyutKonumDto>>> GetirListeMahalle([FromQuery] GetirListeMahalleQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirListeCsbm")]
    public async Task<ResultModel<List<BoyutKonumDto>>> GetirListeCsbm([FromQuery] GetirListeCsbmQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirListeDisKapi")]
    //public async Task<ResultModel<List<BoyutKonumDto>>> GetirListeDisKapi([FromQuery] GetirListeNumaratajQuery request)
    public async Task<ResultModel<List<BoyutKonumDto>>> GetirListeDisKapi([FromQuery] GetirNumaratajByCsbmUavtKoduQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirListeIcKapi")]
    //public async Task<ResultModel<List<BoyutKonumDto>>> GetirListeIcKapi([FromQuery] GetirListeBagimsizBolumQuery request)
    public async Task<ResultModel<List<BagimsizBolumDto>>> GetirListeIcKapi([FromQuery] GetirBagimsizBolumByNumaratajUavtKoduQuery request)
    {
        return await _mediator.Send(request);
    }
}