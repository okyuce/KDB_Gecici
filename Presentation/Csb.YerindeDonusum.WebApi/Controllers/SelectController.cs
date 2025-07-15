using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BasvuruHasarDurumuCQRS;
using Csb.YerindeDonusum.Application.CQRS.BasvuruIptalTurCQRS.Queries;
using Csb.YerindeDonusum.Application.CQRS.BasvuruTurCQRS;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeDurumCQRS;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeDurumCQRS.Queries;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeDurumCQRS.Queries.GetirBinaOdemeDurumListe;
using Csb.YerindeDonusum.Application.CQRS.DestekTurCQRS.Queries;
using Csb.YerindeDonusum.Application.CQRS.TuzelKisilikTipiCQRS;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiDisKapiNo;
using Csb.YerindeDonusum.Application.Dtos;
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
public class SelectController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SelectController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet("GetirBasvuruHasarDurumuListe")]
    public async Task<ResultModel<List<BasvuruHasarDurumuDto>>> GetirBasvuruHasarDurumuListe()
    {
        var result = await _mediator.Send(new GetirBasvuruHasarDurumuListeQuery());
        return _mapper.Map<ResultModel<List<BasvuruHasarDurumuDto>>>(result);
    }

    [HttpGet("GetirBinaDegerlendirmeDurumListe")]
    public async Task<ResultModel<List<BasvuruDurumDto>>> GetirBinaDegerlendirmeDurumListe()
    {
        return await _mediator.Send(new GetirBinaDegerlendirmeDurumListeQuery());
    }

    [HttpGet("GetirBinaOdemeDurumListe")]
    public async Task<ResultModel<List<DurumDto>>> GetirBinaOdemeDurumListe()
    {
        return await _mediator.Send(new GetirBinaOdemeDurumListeQuery());
    }

    [HttpGet("GetirListeYeniYapiDisKapiNo")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<List<SelectDto<string>>>> GetirListeYeniYapiDisKapiNo([FromQuery] GetirListeYeniYapiDisKapiNoQuery request)
    {
        return await _mediator.Send(request);
    }
    
    [HttpGet("GetirBasvuruTurListe")]
    public async Task<ResultModel<List<BasvuruTurCsbDto>>> GetirBasvuruTurListe()
    {
        var result = await _mediator.Send(new GetirBasvuruTurListeQuery());
        return _mapper.Map<ResultModel<List<BasvuruTurCsbDto>>>(result);
    }

    [HttpGet("GetirBasvuruDestekTurListe")]
    public async Task<ResultModel<List<BasvuruDestekTurCsbDto>>> GetirBasvuruDestekTurListe()
    {
        var result = await _mediator.Send(new GetirBasvuruDestekTurListeQuery());
        return _mapper.Map<ResultModel<List<BasvuruDestekTurCsbDto>>>(result);
    }

    [HttpGet("GetirBasvuruIptalTurListe")]
    public async Task<ResultModel<List<BasvuruIptalTurDto>>> GetirBasvuruIptalTurListe()
    {
        var result = await _mediator.Send(new GetirBasvuruIptalTurListeQuery());
        return _mapper.Map<ResultModel<List<BasvuruIptalTurDto>>>(result);
    }

    [HttpGet("GetirBasvuruKanalListe")]
    public async Task<ResultModel<List<BasvuruKanalDto>>> GetirBasvuruKanalListe()
    {
        var result = await _mediator.Send(new GetirBasvuruKanalListeQuery());
        return _mapper.Map<ResultModel<List<BasvuruKanalDto>>>(result);
    } 
    [HttpGet("GetirTuzelKisilikTipiListe")]
    public async Task<ResultModel<List<TuzelKisilikTipiDto>>> GetirTuzelKisilikTipiListe()
    {
        var result = await _mediator.Send(new GetirTuzelKisilikTipiListeQuery());
        return _mapper.Map<ResultModel<List<TuzelKisilikTipiDto>>>(result);
    }
}