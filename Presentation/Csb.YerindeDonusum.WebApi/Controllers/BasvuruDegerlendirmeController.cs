using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetPasifMalikKamuUstelenecek;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.EkleYeniYapi;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.GuncelleYeniYapi;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.GuncelleYeniYapiDisKapiNo;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiServerSideGroupped;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiServerSideGruplanmamis;
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
public class BasvuruDegerlendirmeController : ControllerBase
{

    #region ...: Constructor Injection :...

    private readonly IMediator _mediator;

    public BasvuruDegerlendirmeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #endregion ...: Constructor Injection :...

    [HttpPost(nameof(EkleYeniYapi))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog]
    public async Task<ResultModel<EkleYeniYapiCommandResponseModel>> EkleYeniYapi([FromBody] EkleYeniYapiCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(GuncelleYeniYapi))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog]
    public async Task<ResultModel<GuncelleYeniYapiCommandResponseModel>> GuncelleYeniYapi([FromBody] GuncelleYeniYapiCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(GuncelleYeniYapiDisKapiNo))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog]
    public async Task<ResultModel<GuncelleYeniYapiDisKapiNoCommandResponseModel>> GuncelleYeniYapiDisKapiNo([FromBody] GuncelleYeniYapiDisKapiNoCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(SilYeniYapi))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruIptal}")]
    [AddInfoLog]
    public async Task<ResultModel<SilYeniYapiCommandResponseModel>> SilYeniYapi([FromBody] SilYeniYapiCommand request)
    {
        return await _mediator.Send(request);
    }

    #region ...: List Methods :...

    [HttpPost(nameof(GetirListeYeniYapiServerSideGroupped))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog(PassResponse =true)]
    public async Task<ResultModel<DataTableResponseModel<List<GetirListeYeniYapiServerSideGrouppedResponseModel>>>> GetirListeYeniYapiServerSideGroupped([FromBody] GetirListeYeniYapiServerSideGrouppedQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(KaydetPasifMalikKamuUstelenecek))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog]
    public async Task<ResultModel<KaydetPasifMalikKamuUstelenecekCommandResponseModel>> KaydetPasifMalikKamuUstelenecek([FromBody] KaydetPasifMalikKamuUstelenecekCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(GetirListeYeniYapiServerSideGruplanmamis))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<DataTableResponseModel<List<GetirListeYeniYapiServerSideGruplanmamisResponseModel>>>> GetirListeYeniYapiServerSideGruplanmamis([FromBody] GetirListeYeniYapiServerSideGruplanmamisQuery request)
    {
        return await _mediator.Send(request);
    }

    #endregion ...: List Methods :...
}