using Csb.YerindeDonusum.Application.CQRS.YapiDenetimSeviyeCQRS.Queries;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class YapiDenetimSeviyeController : Controller
{
    public readonly IHttpService _httpService;
    string apiControllerName = nameof(YapiDenetimSeviyeController).Replace("Controller", "");

    public YapiDenetimSeviyeController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    [HttpGet]
    [Route(nameof(GetirSeviyeByYibfNo))]
    public async Task<IActionResult> GetirSeviyeByYibfNo([FromQuery] GetirYapiDenetimSeviyeByYibfNoQuery request)
    {
        var response = await _httpService.GetAsync<GetirYapiDenetimSeviyeByYibfNoQueryResponseModel>($"{apiControllerName}/{nameof(GetirSeviyeByYibfNo)}", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
}