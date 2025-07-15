using Asp.Versioning;
using Csb.YerindeDonusum.Application.CQRS.IlCQRS.Queries.GetAllTakbisDepremIlQuery;
using Csb.YerindeDonusum.Application.CQRS.IlCQRS.Queries.GetAllTakbisIlQuery;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirAdaMahalleIdDenQuery;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeIlceByTakbisIlId;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeMahalleByTakbisIlceIdQuery;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirParselMahalleIdAdaIdDenQuery;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazTcDenQuery;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.Takbis;
using CSB.Core.LogHandler.Attr;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

/// <summary>
/// takbis web servisi islemleri
/// </summary>
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class TakbisWebController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="mediator"></param>
    public TakbisWebController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// takbis servisi uzerinden tum illerin listesini doner
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("Il/GetirListe")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<ResultModel<List<IlModel>>> GetirListeIl([FromQuery] GetAllTakbisIlQuery request)
    {
        return await _mediator.Send(request);
    }

    /// <summary>
    /// takbis servisi uzerinden deprem illerin listesini doner
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("Il/GetirListeDepremIl")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<ResultModel<List<IlModel>>> GetirListeDepremIl([FromQuery] GetAllTakbisDepremIlQuery request)
    {
        return await _mediator.Send(request);
    }

    /// <summary>
    /// takbis il id bilgisi ile takbis servis uzerinden ildeki ilcelerin listesini doner
    /// </summary>
    /// <param name="takbisIlId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("Ilce/GetirListeByTakbisIlId")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<ResultModel<List<GetirListeIlceByTakbisIlIdQueryResponseModel>>> GetirListeByTakbisIlId([FromQuery] GetirListeIlceByTakbisIlIdQuery request)
    {
        return await _mediator.Send(request);
    }

    /// <summary>
    /// ilçe id bilgisi ile takbis servis üzerinden ilçedeki mahalle listesini donuyor
    /// </summary>
    /// <param name="takbisIlceId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("Mahalle/GetirListeByTakbisIlceId")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<ResultModel<List<GetirListeMahalleByTakbisIlceIdQueryResponseModel>>> GetirListeMahalleByIlceId([FromQuery] GetirListeMahalleByTakbsiIlceIdQuery request)
    {
        return await _mediator.Send(request);
    }

    /// <summary>
    /// mahalle id bilgisi ile takbis servis üzerinden ilçedeki ada listesini donuyor
    /// </summary>
    /// <param name="takbisMahalleId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("Ada/GetirListeByMahalleId")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<ResultModel<List<GetirListeAdaByTakbisMahalleIdQueryResponseModel>>> GetirListeAdaByMahalleId([FromQuery] GetirListeAdaByTakbisMahalleIdQuery request)
    {
        return await _mediator.Send(request);
    }

    /// <summary>
    /// mahalle id bilgisi ile takbis servis üzerinden ilçedeki ada listesini donuyor
    /// </summary>
    /// <param name="takbisMahalleId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("Parsel/GetirListeByMahalleId")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<ResultModel<List<GetirListeParselByTakbisMahalleIdAdaIdQueryResponseModel>>> GetirListeParselByMahalleId([FromQuery] GetirListeParselByTakbisMahalleIdAdaIdQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet]
    [Route("Tasinmaz/GetirListeTcDen")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    [AddInfoLog]
    public async Task<ResultModel<List<GetirTasinmazTcDenQueryResponseModel>>> GetirListeTcDen([FromQuery] GetirTasinmazTcDenQuery request)
    {
        return await _mediator.Send(request);
    }
}