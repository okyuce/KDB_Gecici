using Csb.YerindeDonusum.Application.CQRS.BoyutCQRS.Queries.GetirListeBagimsizBolum;
using Csb.YerindeDonusum.Application.CQRS.BoyutCQRS.Queries.GetirListeIl;
using Csb.YerindeDonusum.Application.CQRS.MaksCQRS.Queries.GetirBagimsizBolumByNumaratajUavtKodu;
using Csb.YerindeDonusum.Application.CQRS.MaksCQRS.Queries.GetirNumaratajByCsbmUavtKodu;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class UavtController : Controller
{
    public readonly IHttpService _httpService;

    public UavtController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("GetirListeIl")]
    public async Task<IActionResult> GetirListeIl([FromQuery] GetirListeIlQuery request)
    {
        var response = await _httpService.GetAsync<List<BoyutKonumDto>>("UavtWeb/GetirListeIl", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet]
    [Route("GetirListeDepremIl")]
    public async Task<IActionResult> GetirListeDepremIl([FromQuery] GetirListeDepremIlQuery request)
    {
        var response = await _httpService.GetAsync<List<BoyutKonumDto>>("UavtWeb/GetirListeDepremIl", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet]
    [Route("GetirListeIlce")]
    public async Task<IActionResult> GetirListeIlce([FromQuery] GetirListeIlceQuery request)
    {
        var response = await _httpService.GetAsync<List<BoyutKonumDto>>("UavtWeb/GetirListeIlce", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet]
    [Route("GetirListeMahalle")]
    public async Task<IActionResult> GetirListeMahalle([FromQuery] GetirListeMahalleQuery request)
    {
        var response = await _httpService.GetAsync<List<BoyutKonumDto>>("UavtWeb/GetirListeMahalle", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet]
    [Route("GetirListeCsbm")]
    public async Task<IActionResult> GetirListeCsbm([FromQuery] GetirListeCsbmQuery request)
    {
        var response = await _httpService.GetAsync<List<BoyutKonumDto>>("UavtWeb/GetirListeCsbm", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet]
    [Route("GetirListeDisKapi")]
    //public async Task<IActionResult> GetirListeDisKapi([FromQuery] GetirListeNumaratajQuery request)
    public async Task<IActionResult> GetirListeDisKapi([FromQuery] GetirNumaratajByCsbmUavtKoduQuery request)
    {
        var response = await _httpService.GetAsync<List<BoyutKonumDto>>("UavtWeb/GetirListeDisKapi", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet]
    [Route("GetirListeIcKapi")]
    //public async Task<IActionResult> GetirListeIcKapi([FromQuery] GetirListeBagimsizBolumQuery request)
    public async Task<IActionResult> GetirListeIcKapi([FromQuery] GetirBagimsizBolumByNumaratajUavtKoduQuery request)
    {
        var response = await _httpService.GetAsync<List<BagimsizBolumDto>>("UavtWeb/GetirListeIcKapi", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

}