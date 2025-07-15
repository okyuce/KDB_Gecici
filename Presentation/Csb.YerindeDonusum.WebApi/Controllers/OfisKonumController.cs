using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Commands.EkleOfisKonum;
using Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
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
public class OfisKonumController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OfisKonumController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    /// <summary>
    /// Web portalı için ofis konum listesini döndürmektedir.
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetirOfisKonumListe")]
    [AllowAnonymous]
    public async Task<ResultModel<List<OfisKonumDto>>> GetirOfisKonumListe([FromQuery] GetirOfisKonumListeQuery request)
    {
        return await _mediator.Send(request);
    }

    /// <summary>
    /// Admin paneli için ofis konum listesini döndürmektedir.
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetirDetayliListe")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.OfislerimizYoneticisi}")]
    public async Task<ResultModel<List<GetirOfisKonumDetayResponseModel>>> GetirDetayliListe([FromQuery] GetirOfisKonumListeDetayliQuery request)
    {
        return await _mediator.Send(request);
    }

    /// <summary>
    /// Web portalı için ofis konum detayını getirir.
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetirOfisKonumDetay")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.OfislerimizYoneticisi}")]
    [AddInfoLog]
    public async Task<ResultModel<GetirOfisKonumDetayResponseModel>> GetirOfisKonumDetay([FromQuery] GetirOfisKonumDetayQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("EkleOfisKonum")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.OfislerimizYoneticisi}")]
    [AddInfoLog]

    public async Task<ResultModel<EkleOfisKonumCommandResponseModel>> EkleOfisKonum([FromBody] EkleOfisKonumCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("GuncelleOfisKonum")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.OfislerimizYoneticisi}")]
    [AddInfoLog]
    public async Task<ResultModel<GuncelleOfisKonumCommandResponseModel>> GuncelleOfisKonum([FromBody] GuncelleOfisKonumCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("SilOfisKonum")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.OfislerimizYoneticisi}")]
    [AddInfoLog]
    public async Task<ResultModel<SilOfisKonumCommandResponseModel>> SilOfisKonum([FromBody] SilOfisKonumCommand request)
    {
        return await _mediator.Send(request);
    }
}