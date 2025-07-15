using Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Commands.EkleOfisKonum;
using Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Queries;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.WebApp.Models;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class OfisKonumController : Controller
{
    public readonly IHttpService _httpService;
    private readonly OptionsModel _options;

    public OfisKonumController(IHttpService httpService, IOptions<OptionsModel> options)
    {
        _httpService = httpService;
        _options = options.Value;
    }

    [Route("/[area]/ofisler")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.OfislerimizYoneticisi}")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("GetirDetay")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.OfislerimizYoneticisi}")]
    public async Task<IActionResult> GetirDetay([FromQuery] GetirOfisKonumDetayQuery request)
    {
        var response = await _httpService.GetAsync<GetirOfisKonumDetayResponseModel>("OfisKonum/GetirOfisKonumDetay", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirDetayliListe")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.OfislerimizYoneticisi}")]
    public async Task<IActionResult> GetirListe([FromQuery] GetirOfisKonumListeQuery request)
    {
        var response = await _httpService.GetAsync<List<GetirOfisKonumDetayResponseModel>>("OfisKonum/GetirDetayliListe", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost("ekle")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.OfislerimizYoneticisi}")]
    public async Task<IActionResult> Ekle([FromForm] EkleOfisKonumCommand request)
    {
        var response = await _httpService.PostAsync<EkleOfisKonumCommandResponseModel,
                                    EkleOfisKonumCommand>("OfisKonum/EkleOfisKonum", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost("guncelle")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.OfislerimizYoneticisi}")]
    public async Task<IActionResult> Guncelle([FromForm] GuncelleOfisKonumCommand request)
    {
        var response = await _httpService.PostAsync<GuncelleOfisKonumCommandResponseModel,
                                        GuncelleOfisKonumCommand>("OfisKonum/GuncelleOfisKonum", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost("sil")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.OfislerimizYoneticisi}")]
    public async Task<IActionResult> Sil([FromForm] SilOfisKonumCommand request)
    {
        var response = await _httpService.PostAsync<SilOfisKonumCommandResponseModel, SilOfisKonumCommand>("OfisKonum/SilOfisKonum", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
}
