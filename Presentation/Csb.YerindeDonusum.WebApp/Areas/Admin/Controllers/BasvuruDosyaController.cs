using Csb.YerindeDonusum.Application.CQRS.BasvuruDosyaCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeDosyaCQRS.Commands;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class BasvuruDosyaController : Controller
{
    public readonly IHttpService _httpService;

    public BasvuruDosyaController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    [HttpPost("BasvuruDosyaIndir")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> BasvuruDosyaIndir([FromForm] BasvuruDosyaIndirCommand request)
    {
        var response = await _httpService.PostAsync<BasvuruDosyaIndirCommandResponseModel, BasvuruDosyaIndirCommand>("BasvuruDosyaWeb/BasvuruDosyaIndir", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
    [HttpPost("BinaDegerlendirmeDosyaIndir")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> BinaDegerlendirmeDosyaIndir([FromForm] BinaDegerlendirmeDosyaIndirCommand request)
    {
        var response = await _httpService.PostAsync<BinaDegerlendirmeDosyaIndirCommandResponseModel, BinaDegerlendirmeDosyaIndirCommand>("BasvuruDosyaWeb/BinaDegerlendirmeDosyaIndir", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
}