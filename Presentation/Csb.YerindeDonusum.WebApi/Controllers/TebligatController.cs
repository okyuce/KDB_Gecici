using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CSB.Core.LogHandler.Attr;
using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirListeTebligatMalikler;
using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Commands.TebligatGonder;
using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirTebligatGonderimDetayDosyaByDetayId;
using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirTebligatGonderimDetayDosyaByGuid;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Commands.KaydetTebligatDonderimDetayDosya;
using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirTebligatGonderimDetayById;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TebligatController : ControllerBase
{
    private readonly IMediator _mediator;

    public TebligatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("GetirListeTebligatMalikler")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}, {RoleEnum.Tebligat}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<List<GetirListeTebligatMaliklerQueryResponseModel>>> GetirListeTebligatMalikler([FromBody] GetirListeTebligatMaliklerQuery request)
    {
        //request.MaskelemeKapaliMi = false;
        return await _mediator.Send(request);
    }

    [HttpPost("TebligatGonder")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}, {RoleEnum.Tebligat}")]
    [AddInfoLog]
    public async Task<ResultModel<string>> TebligatGonder(TebligatGonderCommand model)
    {
        return await _mediator.Send(model);
    }


    [HttpGet("GetirTebligatGonderimDetayDosyaByDetayId")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}, {RoleEnum.Tebligat}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<TebligatGonderimDetayDosyaDto>> GetirTebligatGonderimDetayDosyaByDetayId([FromQuery] GetirTebligatGonderimDetayDosyaByDetayIdQuery model)
    {
        return await _mediator.Send(model);
    }
    
    [HttpGet("GetirTebligatGonderimDetayDosyaByGuid")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}, {RoleEnum.Tebligat}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirTebligatGonderimDetayDosyaByGuidQueryResponseModel>> GetirTebligatGonderimDetayDosyaByGuid([FromQuery] GetirTebligatGonderimDetayDosyaByGuidQuery model)
    {
        return await _mediator.Send(model);
    }

    [HttpGet("GetirTebligatGonderimDetayById")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}, {RoleEnum.Tebligat}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirTebligatGonderimDetayByIdQueryResponseModel>> GetirTebligatGonderimDetayById([FromQuery] GetirTebligatGonderimDetayByIdQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("KaydetTebligatGonderimDetayDosya")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}, {RoleEnum.Tebligat}")]
    [AddInfoLog]
    public async Task<ResultModel<TebligatGonderimDetayDosyaDto>> KaydetTebligatGonderimDetayDosya([FromBody] KaydetTebligatGonderimDetayDosyaCommand request)
    {
        return await _mediator.Send(request);
    }
}