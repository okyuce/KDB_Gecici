using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBelediyeBasvuruListeServerSide;
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
public class BelediyeController : ControllerBase
{
    private readonly IMediator _mediator;

    public BelediyeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("GetirBelediyeBasvuruListeServerSide")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<DataTableResponseModel<List<GetirBelediyeBasvuruListeServerSideResponseModel>>>> GetirBelediyeBasvuruListeServerSide([FromBody] GetirBelediyeBasvuruListeServerSideQuery request)
    {
        return await _mediator.Send(request);
    }
}