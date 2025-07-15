using Csb.YerindeDonusum.Application.CQRS.YapiBelgeCQRS.Queries.GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNo;
using Csb.YerindeDonusum.Application.CQRS.YapiBelgeCQRS.Queries.GetirYapiBelgeByYapiKimlikNo;
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
public class YapiBelgeController : ControllerBase
{
    private readonly IMediator _mediator;

    public YapiBelgeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetirRuhsatByBultenNo")]
    [AddInfoLog]
    public async Task<ResultModel<GetirYapiBelgeRuhsatByBultenNoQueryResponseModel>> GetirRuhsatByBultenNo([FromQuery] GetirYapiBelgeRuhsatByBultenNoQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNo")]
    [AddInfoLog]
    public async Task<ResultModel<GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQueryResponseModel>> GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNo([FromQuery] GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQuery request)
    {
        return await _mediator.Send(request);
    }
}