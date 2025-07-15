using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetPasifMalikKamuUstelenecek;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.EkleYeniYapi;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.GuncelleYeniYapi;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.GuncelleYeniYapiDisKapiNo;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiServerSideGroupped;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiServerSideGruplanmamis;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class BasvuruDegerlendirmeController : Controller
{

    #region ...: Constructor Injection & Global Variables :...

    public readonly IHttpService _httpService;

    string apiControllerName = nameof(BasvuruDegerlendirmeController).Replace(nameof(Controller), "");

    public BasvuruDegerlendirmeController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion

    #region ...: Home Page :...

    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public IActionResult Index()
    {
        return View();
    }

    #endregion

    //#region ...: Commands :...

    [HttpPost(nameof(EkleYeniYapi))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> EkleYeniYapi([FromForm] EkleYeniYapiCommand request)
    {
        var response = await _httpService.PostAsync<EkleYeniYapiCommandResponseModel, EkleYeniYapiCommand>
            ($"{apiControllerName}/{nameof(EkleYeniYapi)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    } 

    [HttpPost(nameof(GuncelleYeniYapi))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> GuncelleYeniYapi([FromForm] GuncelleYeniYapiCommand request)
    {
        var response = await _httpService.PostAsync<GuncelleYeniYapiCommandResponseModel, GuncelleYeniYapiCommand>
                                            ($"{apiControllerName}/{nameof(GuncelleYeniYapi)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }  
    
    [HttpPost(nameof(GuncelleYeniYapiDisKapiNo))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> GuncelleYeniYapiDisKapiNo([FromForm] GuncelleYeniYapiDisKapiNoCommand request)
    {
        var response = await _httpService.PostAsync<GuncelleYeniYapiDisKapiNoCommandResponseModel, GuncelleYeniYapiDisKapiNoCommand>
                                        ($"{apiControllerName}/{nameof(GuncelleYeniYapiDisKapiNo)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost(nameof(SilYeniYapi))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruIptal}")]
    public async Task<IActionResult> SilYeniYapi([FromForm] SilYeniYapiCommand request)
    {
        var response = await _httpService.PostAsync<SilYeniYapiCommandResponseModel, SilYeniYapiCommand>
                                            ($"{apiControllerName}/{nameof(SilYeniYapi)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost(nameof(KaydetPasifMalikKamuUstelenecek))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> KaydetPasifMalikKamuUstelenecek([FromForm] KaydetPasifMalikKamuUstelenecekCommand request)
    {
        var response = await _httpService.PostAsync<KaydetPasifMalikKamuUstelenecekCommandResponseModel, KaydetPasifMalikKamuUstelenecekCommand>
            ($"{apiControllerName}/{nameof(KaydetPasifMalikKamuUstelenecek)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
    #region ...: List Methods

    [HttpPost(nameof(GetirListeYeniYapiServerSideGroupped))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> GetirListeYeniYapiServerSideGroupped([FromForm] GetirListeYeniYapiServerSideGrouppedQuery request, [FromQuery] bool? bosTabloGetir)
    {
        if (bosTabloGetir == true)
            return Ok(new DataTableResponseModel<List<GetirListeYeniYapiServerSideGrouppedResponseModel>>() { draw = request?.draw ?? 0 });

        var response = await _httpService.PostAsync<DataTableResponseModel<List<GetirListeYeniYapiServerSideGrouppedResponseModel>>, GetirListeYeniYapiServerSideGrouppedQuery>
                                        ($"{apiControllerName}/{nameof(GetirListeYeniYapiServerSideGroupped)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost(nameof(GetirListeYeniYapiServerSideGruplanmamis))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> GetirListeYeniYapiServerSideGruplanmamis([FromForm] GetirListeYeniYapiServerSideGruplanmamisQuery request, [FromQuery] bool? bosTabloGetir)
    {
        if (bosTabloGetir == true)
            return Ok(new DataTableResponseModel<List<GetirListeYeniYapiServerSideGruplanmamisResponseModel>>() { draw = request?.draw ?? 0 });

        var response = await _httpService.PostAsync<DataTableResponseModel<List<GetirListeYeniYapiServerSideGruplanmamisResponseModel>>, GetirListeYeniYapiServerSideGruplanmamisQuery>
                                        ($"{apiControllerName}/{nameof(GetirListeYeniYapiServerSideGruplanmamis)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    #endregion
}