using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Commands.BinaOdemeDurumGuncelle;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Commands.BinaOdemeEkle;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeBinaOdemeServerSide;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeGruplanmamisOdemeServerSide;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeOdemeTalepleriServerSide;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeOdemeYapilanServerSide;
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
public class BinaOdemeController : ControllerBase
{
    private readonly IMediator _mediator;

    public BinaOdemeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("BinaOdemeEkle")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele},{RoleEnum.OdemeTalebiOnay}")]
    [AddInfoLog]
    public async Task<ResultModel<BinaOdemeEkleResponseModel>> BinaOdemeEkle([FromBody] BinaOdemeEkleCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("BinaOdemeDurumGuncelle")]
    [Authorize(Roles = $"{RoleEnum.Admin},{RoleEnum.OdemeTalebiOnay}")]
    [AddInfoLog]
    public async Task<ResultModel<BinaOdemeDurumGuncelleResponseModel>> BinaOdemeDurumGuncelle([FromBody] BinaOdemeDurumGuncelleCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("GetirListeBinaOdemeServerSide")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele},{RoleEnum.OdemeTalebiOnay}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<DataTableResponseModel<List<BinaOdemeDto>>>> GetirListeBinaOdemeServerSide([FromBody] GetirListeBinaOdemeServerSideQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(GetirListeOdemeTalepleriServerSide))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele},{RoleEnum.OdemeTalebiOnay}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<DataTableResponseModel<List<GetirListeOdemeTalepleriServerSideResponseModel>>>> GetirListeOdemeTalepleriServerSide([FromBody] GetirListeOdemeTalepleriServerSideQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(GetirListeOdemeYapilanServerSide))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele},{RoleEnum.OdemeTalebiOnay}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<DataTableResponseModel<List<GetirListeOdemeYapilanServerSideResponseModel>>>> GetirListeOdemeYapilanServerSide([FromBody] GetirListeOdemeYapilanServerSideQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("GetirListeGruplanmamisOdemeServerSide")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele},{RoleEnum.OdemeTalebiOnay}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<DataTableResponseModel<List<BinaOdemeDto>>>> GetirListeGruplanmamisOdemeServerSide([FromBody] GetirListeGruplanmamisOdemeServerSideQuery request)
    {
        return await _mediator.Send(request);
    }
}