using Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiAdSoyadTcDen;
using Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiBilgileriTcDen;
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
public class KpsWebController : ControllerBase
{
    private readonly IMediator _mediator;

    public KpsWebController(IMediator mediator)
    {
        _mediator = mediator;
    }
  
    [HttpGet("GetirKisiBilgileriTcDen")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    [AddInfoLog]
    public async Task<ResultModel<GetirKisiBilgileriTcDenQueryResponseModel>> GetirKisiBilgileriTcDen([FromQuery] GetirKisiBilgileriTcDenQuery request)
    {
        request.MaskelemeKapaliMi = false;
        return await _mediator.Send(request);
    }
  
    [HttpGet("GetirKisiAdSoyadTcDen")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    [AddInfoLog]
    public async Task<ResultModel<GetirKisiAdSoyadTcDenQueryResponseModel>> GetirKisiAdSoyadTcDen([FromQuery] GetirKisiAdSoyadTcDenQuery request)
    {
        request.MaskelemeKapaliMi = false;
        return await _mediator.Send(request);
    }
}