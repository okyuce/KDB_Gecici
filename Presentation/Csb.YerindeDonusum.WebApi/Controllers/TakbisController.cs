using Asp.Versioning;
using Csb.YerindeDonusum.Application.CQRS.IlCQRS.Queries.GetAllTakbisDepremIlQuery;
using Csb.YerindeDonusum.Application.CQRS.IlCQRS.Queries.GetAllTakbisIlQuery;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirAdaMahalleIdDenQuery;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeIlceByTakbisIlId;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeMahalleByTakbisIlceIdQuery;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirParselMahalleIdAdaIdDenQuery;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.Takbis;
using Csb.YerindeDonusum.WebApi.BasicAuthFiles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

/// <summary>
/// takbis web servisi islemleri
/// </summary>
[Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme)]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class TakbisController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="mediator"></param>
    public TakbisController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// takbis servisi uzerinden tum illerin listesini doner
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("Il/GetirListe")]
    public async Task<ResultModel<List<IlModel>>> GetirListeIl()
    {
        var query = new GetAllTakbisIlQuery();

        return await _mediator.Send(query);
    }

    /// <summary>
    /// takbis servisi uzerinden deprem illerin listesini doner
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("Il/GetirListeDepremIl")]
    public async Task<ResultModel<List<IlModel>>> GetirListeDepremIl()
    {
        var query = new GetAllTakbisDepremIlQuery();

        return await _mediator.Send(query);
    }

    /// <summary>
    /// takbis il id bilgisi ile takbis servis uzerinden ildeki ilcelerin listesini doner
    /// </summary>
    /// <param name="takbisIlId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("Ilce/GetirListeByTakbisIlId")]
    public async Task<ResultModel<List<GetirListeIlceByTakbisIlIdQueryResponseModel>>> GetirListeByTakbisIlId(int takbisIlId)
    {
        var query = new GetirListeIlceByTakbisIlIdQuery() { TakbisIlId = takbisIlId };

        return await _mediator.Send(query);
    }

    /// <summary>
    /// ilçe id bilgisi ile takbis servis üzerinden ilçedeki mahalle listesini donuyor
    /// </summary>
    /// <param name="takbisIlceId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("Mahalle/GetirListeByTakbisIlceId")]
    public async Task<ResultModel<List<GetirListeMahalleByTakbisIlceIdQueryResponseModel>>> GetirListeMahalleByIlceId(int takbisIlceId)
    {
        var query = new GetirListeMahalleByTakbsiIlceIdQuery() { TakbisIlceId = takbisIlceId };

        return await _mediator.Send(query);
    }

    /// <summary>
    /// mahalle id bilgisi ile takbis servis üzerinden ilçedeki ada listesini donuyor
    /// </summary>
    /// <param name="takbisMahalleId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("Ada/GetirListeByMahalleId")]
    public async Task<ResultModel<List<GetirListeAdaByTakbisMahalleIdQueryResponseModel>>> GetirListeAdaByMahalleId(int takbisMahalleId)
    {
        var query = new GetirListeAdaByTakbisMahalleIdQuery() { TakbisMahalleId = takbisMahalleId };

        return await _mediator.Send(query);
    }

    /// <summary>
    /// mahalle id bilgisi ile takbis servis üzerinden ilçedeki ada listesini donuyor
    /// </summary>
    /// <param name="takbisMahalleId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("Parsel/GetirListeByMahalleId")]
    public async Task<ResultModel<List<GetirListeParselByTakbisMahalleIdAdaIdQueryResponseModel>>> GetirListeParselByMahalleId(int takbisMahalleId, string adaNo)
    {
        var query = new GetirListeParselByTakbisMahalleIdAdaIdQuery() { TakbisMahalleId = takbisMahalleId,AdaNo = adaNo };

        return await _mediator.Send(query);
    }
}
