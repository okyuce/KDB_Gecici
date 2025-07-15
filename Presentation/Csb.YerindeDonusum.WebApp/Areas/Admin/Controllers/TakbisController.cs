using Csb.YerindeDonusum.Application.CQRS.IlCQRS.Queries.GetAllTakbisDepremIlQuery;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirAdaMahalleIdDenQuery;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeIlceByTakbisIlId;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeMahalleByTakbisIlceIdQuery;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirParselMahalleIdAdaIdDenQuery;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazTcDenQuery;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models.Takbis;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class TakbisController : Controller
{
    public readonly IHttpService _httpService;

    public TakbisController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    /// <summary>
    /// takbis servisi uzerinden deprem illerin listesini doner
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("Il/GetirListeDepremIl")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> GetirListeDepremIl([FromQuery] GetAllTakbisDepremIlQuery request)
    {
        var response = await _httpService.GetAsync<List<IlModel>>("TakbisWeb/Il/GetirListeDepremIl", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    /// <summary>
    /// takbis il id bilgisi ile takbis servis uzerinden ildeki ilcelerin listesini doner
    /// </summary>
    /// <param name="takbisIlId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("Ilce/GetirListeByTakbisIlId")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> GetirListeByTakbisIlId([FromQuery] GetirListeIlceByTakbisIlIdQuery request)
    {
        var response = await _httpService.GetAsync<List<GetirListeIlceByTakbisIlIdQueryResponseModel>>("TakbisWeb/Ilce/GetirListeByTakbisIlId", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    /// <summary>
    /// ilçe id bilgisi ile takbis servis üzerinden ilçedeki mahalle listesini donuyor
    /// </summary>
    /// <param name="takbisIlceId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("Mahalle/GetirListeByTakbisIlceId")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> GetirListeMahalleByIlceId([FromQuery] GetirListeMahalleByTakbsiIlceIdQuery request)
    {
        var response = await _httpService.GetAsync<List<GetirListeMahalleByTakbisIlceIdQueryResponseModel>>("TakbisWeb/Mahalle/GetirListeByTakbisIlceId", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    /// <summary>
    /// mahalle id bilgisi ile takbis servis üzerinden ilçedeki ada listesini donuyor
    /// </summary>
    /// <param name="takbisMahalleId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("Ada/GetirListeByMahalleId")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> GetirListeAdaByMahalleId([FromQuery] GetirListeAdaByTakbisMahalleIdQuery request)
    {
        var response = await _httpService.GetAsync<List<GetirListeAdaByTakbisMahalleIdQueryResponseModel>>("TakbisWeb/Ada/GetirListeByMahalleId", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    /// <summary>
    /// mahalle id bilgisi ile takbis servis üzerinden ilçedeki ada listesini donuyor
    /// </summary>
    /// <param name="takbisMahalleId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("Parsel/GetirListeByMahalleId")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> GetirListeParselByMahalleId([FromQuery] GetirListeParselByTakbisMahalleIdAdaIdQuery request)
    {
        var response = await _httpService.GetAsync<List<GetirListeParselByTakbisMahalleIdAdaIdQueryResponseModel>>("TakbisWeb/Parsel/GetirListeByMahalleId", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet]
    [Route("Tasinmaz/GetirListeTcDen")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> TasinmazGetirListeTcDen([FromQuery] GetirTasinmazTcDenQuery request)
    {
        var response = await _httpService.GetAsync<List<GetirTasinmazTcDenQueryResponseModel>>("TakbisWeb/Tasinmaz/GetirListeTcDen", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
}