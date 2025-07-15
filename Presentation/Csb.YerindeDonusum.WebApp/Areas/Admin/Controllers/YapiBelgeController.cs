using Csb.YerindeDonusum.Application.CQRS.YapiBelgeCQRS.Queries.GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNo;
using Csb.YerindeDonusum.Application.CQRS.YapiBelgeCQRS.Queries.GetirYapiBelgeByYapiKimlikNo;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class YapiBelgeController : Controller
{
    public readonly IHttpService _httpService;

    public YapiBelgeController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    [HttpGet]
    [Route("GetirRuhsatByBultenNo")]
    public async Task<IActionResult> GetirRuhsatByBultenNo([FromQuery] GetirYapiBelgeRuhsatByBultenNoQuery request)
    {
        var response = await _httpService.GetAsync<GetirYapiBelgeRuhsatByBultenNoQueryResponseModel>("YapiBelge/GetirRuhsatByBultenNo", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet]
    [Route("GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNo")]
    public async Task<IActionResult> GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNo([FromQuery] GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQuery request)
    {
        var response = await _httpService.GetAsync<GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQueryResponseModel>("YapiBelge/GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNo", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
}