using Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiAdSoyadTcDen;
using Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiBilgileriTcDen;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class KpsController : Controller
{
    public readonly IHttpService _httpService;

    public KpsController(IHttpService httpService)
    {
        _httpService = httpService;
    }
   
	[HttpGet("GetirKisiBilgileriTcDen")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> GetirKisiBilgileriTcDen([FromQuery] GetirKisiBilgileriTcDenQuery request)
	{
        var response = await _httpService.GetAsync<GetirKisiBilgileriTcDenQueryResponseModel> ("KpsWeb/GetirKisiBilgileriTcDen", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }	
   
	[HttpGet("GetirKisiAdSoyadTcDen")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> GetirKisiAdSoyadTcDen([FromQuery] GetirKisiAdSoyadTcDenQuery request)
	{
        var response = await _httpService.GetAsync<GetirKisiAdSoyadTcDenQueryResponseModel> ("KpsWeb/GetirKisiAdSoyadTcDen", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
}