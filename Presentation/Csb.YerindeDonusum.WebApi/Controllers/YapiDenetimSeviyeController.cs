using Csb.YerindeDonusum.Application.CQRS.YapiDenetimSeviyeCQRS.Queries;
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
public class YapiDenetimSeviyeController : ControllerBase
{
    private readonly IMediator _mediator;

    public YapiDenetimSeviyeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(nameof(GetirSeviyeByYibfNo))]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirYapiDenetimSeviyeByYibfNoQueryResponseModel>> GetirSeviyeByYibfNo([FromQuery] GetirYapiDenetimSeviyeByYibfNoQuery request)
    {
        return await _mediator.Send(request);
    }
}