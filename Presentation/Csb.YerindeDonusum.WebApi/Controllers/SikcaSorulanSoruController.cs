using Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS;
using Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS.Commands.EkleSikcaSorulanSoru;
using Csb.YerindeDonusum.Application.Dtos;
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
//[Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme)]
public class SikcaSorulanSoruController : ControllerBase
{
    private readonly IMediator _mediator;

    public SikcaSorulanSoruController(IMediator mediator)
    {
        _mediator = mediator;
    }

	[AllowAnonymous]
    [HttpGet("GetirSikcaSorulanSoruListe")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<List<SikcaSorulanSoruDto>>> GetirSikcaSorulanSoruListe([FromQuery] GetirSikcaSorulanSoruListeQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("GetirListeSikcaSorulanSoruListeServerSide")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.SikcaSorulanSoruYoneticisi}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<DataTableResponseModel<List<SikcaSorulanSoruServerSideDto>>>> GetirListeSikcaSorulanSoruListeServerSide([FromBody] GetirListeSikcaSorulanSoruListeServerSideQueryModel request)
    {
        return await _mediator.Send(new GetirListeSikcaSorulanSoruListeServerSideQuery { Model = request });
    }

    [HttpGet("GetirIdIle")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.SikcaSorulanSoruYoneticisi}")]
    [AddInfoLog]
    public async Task<ResultModel<SikcaSorulanSoruDto>> GetirIdIle([FromQuery] GetirSSSIdIleQuery request)
    {
        return await _mediator.Send(request);
    }

	[HttpPost("EkleSikcaSorulanSoru")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.SikcaSorulanSoruYoneticisi}")]
    [AddInfoLog]
    public async Task<ResultModel<EkleSikcaSorulanSoruCommandResponseModel>> EkleSikcaSorulanSoru([FromBody] EkleSikcaSorulanSoruCommand request)
	{		
        return await _mediator.Send(request);
	}

	[HttpPost("GuncelleSikcaSorulanSoru")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.SikcaSorulanSoruYoneticisi}")]
    [AddInfoLog]
    public async Task<ResultModel<GuncelleSikcaSorulanSoruCommandResponseModel>> GuncelleSikcaSorulanSoru([FromBody] GuncelleSikcaSorulanSoruCommand request)
	{
		return await _mediator.Send(request);
	}

	[HttpPost("SilSikcaSorulanSoru")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.SikcaSorulanSoruYoneticisi}")]
    [AddInfoLog]
    public async Task<ResultModel<SilSikcaSorulanSoruCommandResponseModel>> SilSikcaSorulanSoru([FromBody] SilSikcaSorulanSoruCommand request)
	{
		return await _mediator.Send(request);
	}
	
}