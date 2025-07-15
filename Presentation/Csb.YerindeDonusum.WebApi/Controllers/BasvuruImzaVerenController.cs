using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVeren;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVerenAfadBelge;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVerenSozlesme;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruKendimKarsilamakIstiyorum;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Queries.GetirBasvuruImzaByBasvuruId;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Queries.GetirBasvuruImzaSozlesmeDosyaByGuid;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirDetayHibeTaahhutnameSozlesme;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirDetaySozlesmeGeriOdemeBilgileri;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirDetayYerindeYapimKrediSozlesme;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirBasvuruImzaSozlesmeDosyaByGuid;
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
public class BasvuruImzaVerenController : ControllerBase
{
    private readonly IMediator _mediator;

    public BasvuruImzaVerenController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(nameof(GetirBasvuruImzaByBasvuruId))]
    [AddInfoLog]
    public async Task<ResultModel<BasvuruImzaVerenDto>> GetirBasvuruImzaByBasvuruId([FromQuery] GetirBasvuruImzaByBasvuruIdQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(KaydetBasvuruImzaVeren))]
    [AddInfoLog]
    public async Task<ResultModel<BasvuruImzaVerenDto>> KaydetBasvuruImzaVeren([FromBody] KaydetBasvuruImzaVerenCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(KaydetBasvuruKendimKarsilamakIstiyorum))]
    [AddInfoLog]
    public async Task<ResultModel<BasvuruImzaVerenDto>> KaydetBasvuruKendimKarsilamakIstiyorum([FromBody] KaydetBasvuruKendimKarsilamakIstiyorumCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(KaydetBasvuruImzaVerenSozlesme))]
    [AddInfoLog]
    public async Task<ResultModel<BasvuruImzaVerenDto>> KaydetBasvuruImzaVerenSozlesme([FromBody] KaydetBasvuruImzaVerenSozlesmeCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(KaydetBasvuruImzaVerenAfadBelge))]
    [AddInfoLog]
    public async Task<ResultModel<string>> KaydetBasvuruImzaVerenAfadBelge([FromBody] KaydetBasvuruImzaVerenAfadBelgeCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet(nameof(GetirDetayHibeTaahhutnameSozlesme))]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirDetayHibeTaahhutnameSozlesmeQueryResponseModel>> GetirDetayHibeTaahhutnameSozlesme([FromQuery] GetirDetayHibeTaahhutnameSozlesmeQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet(nameof(GetirDetayYerindeYapimKrediSozlesme))]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirDetayYerindeYapimKrediSozlesmeQueryResponseModel>> GetirDetayYerindeYapimKrediSozlesme([FromQuery] GetirDetayYerindeYapimKrediSozlesmeQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet(nameof(GetirDetaySozlesmeGeriOdemeBilgileri))]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirDetaySozlesmeGeriOdemeBilgileriQueryResponseModel>> GetirDetaySozlesmeGeriOdemeBilgileri([FromQuery] GetirDetaySozlesmeGeriOdemeBilgileriQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet(nameof(GetirBasvuruImzaSozlesmeDosyaByGuid))]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirBasvuruImzaSozlesmeDosyaByGuidResponseModel>> GetirBasvuruImzaSozlesmeDosyaByGuid([FromQuery] GetirBasvuruImzaSozlesmeDosyaByGuidQuery request)
    {
        return await _mediator.Send(request);
    }
}