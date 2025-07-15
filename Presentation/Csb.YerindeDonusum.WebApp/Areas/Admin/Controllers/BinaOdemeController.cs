using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Commands.BinaOdemeDurumGuncelle;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Commands.BinaOdemeEkle;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeBinaOdemeServerSide;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeGruplanmamisOdemeServerSide;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeOdemeTalepleriServerSide;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeOdemeYapilanServerSide;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class BinaOdemeController : Controller
{
    public readonly IHttpService _httpService;

    string apiControllerName = nameof(BinaOdemeController).Replace("Controller", "");

    public BinaOdemeController(IHttpService httpService)
    {
        _httpService = httpService;
    }
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}, {RoleEnum.OdemeTalebiOnay}")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("BinaOdemeEkle")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}, {RoleEnum.OdemeTalebiOnay}")]
    public async Task<IActionResult> BinaOdemeEkle([FromForm] BinaOdemeEkleCommand request)
    {
        var res = await _httpService.PostAsync<BinaOdemeEkleResponseModel, BinaOdemeEkleCommand>($"{apiControllerName}/BinaOdemeEkle", request);
        if (!res.ResultModel.IsError)
            return Ok(res.ResultModel.Result);

        return StatusCode(res.StatusCode, res.ResultModel.ErrorMessageContent);
    } 

    [HttpPost("BinaOdemeDurumGuncelle")]
    [Authorize(Roles = $"{RoleEnum.Admin},{RoleEnum.OdemeTalebiOnay}")]
    public async Task<IActionResult> BinaOdemeDurumGuncelle([FromForm] BinaOdemeDurumGuncelleCommand request)
    {
        var res = await _httpService.PostAsync<BinaOdemeDurumGuncelleResponseModel, BinaOdemeDurumGuncelleCommand>
                                                ($"{apiControllerName}/BinaOdemeDurumGuncelle", request);
        if (!res.ResultModel.IsError)
            return Ok(res.ResultModel.Result);

        return StatusCode(res.StatusCode, res.ResultModel.ErrorMessageContent);
    }

    [HttpPost("GetirListeBinaOdemeServerSide")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}, {RoleEnum.OdemeTalebiOnay}")]
    public async Task<IActionResult> GetirListeBinaOdemeServerSide([FromForm] GetirListeBinaOdemeServerSideQuery request)
    {
        var response = await _httpService.PostAsync<DataTableResponseModel<List<BinaOdemeDto>>, GetirListeBinaOdemeServerSideQuery>
                                    ($"{apiControllerName}/GetirListeBinaOdemeServerSide", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost(nameof(GetirListeOdemeTalepleriServerSide))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.OdemeTalebiOnay}")]
    public async Task<IActionResult> GetirListeOdemeTalepleriServerSide([FromQuery] bool? bosTabloGetir, [FromForm] GetirListeOdemeTalepleriServerSideQuery request)
    {
        if (bosTabloGetir == true)
            return Ok(new DataTableResponseModel<List<GetirListeOdemeTalepleriServerSideResponseModel>>() { draw = request?.draw ?? 0 });

        var response = await _httpService.PostAsync<DataTableResponseModel<List<GetirListeOdemeTalepleriServerSideResponseModel>>, GetirListeOdemeTalepleriServerSideQuery>
                                        ($"{apiControllerName}/{nameof(GetirListeOdemeTalepleriServerSide)}", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost(nameof(GetirListeOdemeYapilanServerSide))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.OdemeTalebiOnay}")]
    public async Task<IActionResult> GetirListeOdemeYapilanServerSide([FromForm] GetirListeOdemeYapilanServerSideQuery request)
    {
        var response = await _httpService.PostAsync<DataTableResponseModel<List<GetirListeOdemeYapilanServerSideResponseModel>>, GetirListeOdemeYapilanServerSideQuery>
                                        ($"{apiControllerName}/{nameof(GetirListeOdemeYapilanServerSide)}", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost("GetirListeGruplanmamisOdemeServerSide")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}, {RoleEnum.OdemeTalebiOnay}")]
    public async Task<IActionResult> GetirListeGruplanmamisOdemeServerSide([FromForm] GetirListeGruplanmamisOdemeServerSideQuery request, [FromQuery] bool? bosTabloGetir)
    {
        if (bosTabloGetir == true)
            return Ok(new DataTableResponseModel<List<BinaOdemeDto>>() { draw = request?.draw ?? 0 });

        var response = await _httpService.PostAsync<DataTableResponseModel<List<BinaOdemeDto>>, GetirListeGruplanmamisOdemeServerSideQuery>
                                    ($"{apiControllerName}/GetirListeGruplanmamisOdemeServerSide", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

}

