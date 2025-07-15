using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeYapiRuhsatRapor;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirBinaOdemeRapor;
using Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.GetirListeBinabazliOranServerSide;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]")]
[Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.Raporlama}")]
public class RaporController : Controller
{
    public readonly IHttpService _httpService;

    public RaporController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    //[Route("/[area]/[controller]/BinaBazliOran")]
    //public IActionResult BinaBazliOran()
    //{
    //    return View();
    //}

    [Route("/[area]/[controller]/YapiRuhsat")]
    public IActionResult YapiRuhsat()
    {
        return View();
    }
    
    [Route("/[area]/[controller]/YapiIlerleme")]
    public IActionResult YapiIlerleme()
    {
        return View();
    }

    [Route("/[area]/[controller]/YdBilgi")]
    public IActionResult YdBilgi()
    {
        return View();
    }

    [Route("/[area]/[controller]/BinaOdeme")]
    public async Task<IActionResult> BinaOdeme()
    {
        var response = await _httpService.GetAsync<List<GetirListeBinaOdemeRaporQueryResponseModel>>("Rapor/GetirListeBinaOdeme", new GetirListeBinaOdemeRaporQuery());

        return View(response.ResultModel.IsError ? null : response.ResultModel?.Result);
    }

    [HttpPost("GetirListeBinabazliOranServerSide")]
    public async Task<IActionResult> GetirListeBinaBazliOranServerSide([FromForm] GetirListeBinabazliOranServerSideQuery request)
    {
        var response = await _httpService.PostAsync<DataTableResponseModel<List<GetirListeBinabazliOranServerSideQueryResponseModel>>, GetirListeBinabazliOranServerSideQuery>("Rapor/GetirListeBinabazliOranServerSide", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirListeYapiRuhsat")]
    public async Task<IActionResult> GetirListeYapiRuhsat(GetirListeYapiRuhsatRaporQuery request)
    {
        var response = await _httpService.GetAsync<List<GetirListeYapiRuhsatRaporQueryResponseModel>>("Rapor/GetirListeYapiRuhsat", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirListeBinaOdeme")]
    public async Task<IActionResult> GetirListeBinaOdeme(GetirListeBinaOdemeRaporQuery request)
    {
        var response = await _httpService.GetAsync<List<GetirListeBinaOdemeRaporQueryResponseModel>>("Rapor/GetirListeBinaOdeme", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
}