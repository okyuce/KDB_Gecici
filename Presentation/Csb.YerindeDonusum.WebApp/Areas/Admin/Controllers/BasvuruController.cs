using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirAfadBasvuruListeByTcNo;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.BasvuruOnaylaReddet;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.BasvuruSonuclandir;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.EkleBasvuru;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.GuncelleBasvuru;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.IptalBasvuruById;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruDetayById;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruGostergePaneliVeri;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruListeServerSide;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeMalikler;
using Csb.YerindeDonusum.Application.CQRS.BasvuruDosyaCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.BilgilendirmeMesajCQRS.Queries.GetirBilgilendirmeMesajById;
using Csb.YerindeDonusum.Application.Dtos.Afad;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.AktarKamuUstlenecek;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.AdaParselGuncelle;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.IptalBasvuruByIdFromWeb;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListePasifMalikler;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class BasvuruController : Controller
{
    public readonly IHttpService _httpService;

    public BasvuruController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("BasvuruDosyaIndir")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}, {RoleEnum.BasvuruDuzenle}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> BasvuruDosyaIndir([FromForm] BasvuruDosyaIndirCommand request)
    {
        var response = await _httpService.PostAsync<BasvuruDosyaIndirCommandResponseModel, BasvuruDosyaIndirCommand>("BasvuruDosyaWeb/BasvuruDosyaIndir", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost("AktarKamuUstlenecek")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}, {RoleEnum.BasvuruDuzenle}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> AktarKamuUstlenecek([FromForm] AktarKamuUstlenecekCommand request)
    {
        var response = await _httpService.PostAsync<string, AktarKamuUstlenecekCommand>("BasvuruWeb/AktarKamuUstlenecek", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
    
    [HttpPost("BasvuruOnaylaReddet")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}, {RoleEnum.BasvuruDuzenle}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> BasvuruOnaylaReddet([FromForm] BasvuruOnaylaReddetCommand request)
    {
        var response = await _httpService.PostAsync<string, BasvuruOnaylaReddetCommand>("BasvuruWeb/BasvuruOnaylaReddet", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    //[HttpPost("GuncelleBasvuruAfadDurum")]
    //[Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}, {RoleEnum.BasvuruDuzenle}, {RoleEnum.BasvuruListele}")]
    //public async Task<IActionResult> GuncelleBasvuruAfadDurum([FromForm] GuncelleBasvuruAfadDurumCommand request)
    //{
    //    var response = await _httpService.PostAsync<string, GuncelleBasvuruAfadDurumCommand>("BasvuruWeb/GuncelleBasvuruAfadDurum", request);

    //    if (!response.ResultModel.IsError)
    //        return Ok(response.ResultModel.Result);

    //    return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    //}

    [HttpPost("IptalBasvuru")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruIptal}")]
    public async Task<IActionResult> IptalBasvuru([FromForm] IptalBasvuruByIdFromWebCommand request)
    {
        var response = await _httpService.PostAsync<string, IptalBasvuruByIdFromWebCommand>("BasvuruWeb/IptalBasvuru", request);
        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost("BasvuruSonuclandir")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruDuzenle}, {RoleEnum.BasvuruIptal}")]
    public async Task<IActionResult> BasvuruSonuclandir([FromForm] BasvuruSonuclandirCommand request)
    {
        var response = await _httpService.PostAsync<string, BasvuruSonuclandirCommand>("BasvuruWeb/BasvuruSonuclandir", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    //[HttpGet("GetirBasvuruSonuclandirPartial")]
    //[Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}, {RoleEnum.BasvuruDuzenle}, {RoleEnum.BasvuruIptal}")]
    //public async Task<IActionResult> GetirBasvuruSonuclandirPartial([FromQuery] GetirBasvuruDetayByIdQueryModel request)
    //{
    //    //var basvuruResult = await _httpService.GetAsync<GetirBasvuruDetayByIdQueryResponseModel>("BasvuruWeb/GetirBasvuruDetay",
    //    //                new GetirBasvuruDetayByIdQueryModel { BasvuruId = request?.BasvuruId, TcKimlikNo = request?.TcKimlikNo });

    //    var afadResult = await _httpService.GetAsync<List<AfadBasvuruDto>>("BasvuruWeb/GetirAfadBasvuruListeByTcNo",
    //                    new GetirAfadBasvuruListeByTcNoQuery { TcKimlikNo = request?.TcKimlikNo });

    //    string? askiKodu = request?.HasarTespitAskiKodu; //basvuruResult.ResultModel.Result?.HasarTespitAskiKodu?.ToUpper();

    //    var afaddaVarMi = afadResult.ResultModel.Result?.Any(x => x.AskiBaskiDetayAskiKodu.ToUpper() == askiKodu) ?? false;
    //    if (afaddaVarMi == true)
    //    {
    //        return StatusCode(400, "Afadda başvurunuz olduğu için işleme devam edilemiyor.");
    //    }

    //    return Ok(afadResult.ResultModel.Result);
    //}

    [HttpPost("EkleGercekKisiBasvuru")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    [RequestFormLimits(ValueCountLimit = (2 * 1024))]
    public async Task<IActionResult> EkleGercekKisiBasvuru([FromForm] EkleBasvuruCommandModel request)
    {
        request.TuzelKisiTipId = null;
        request.VatandaslikDurumu = 1;
        request.BasvuruKanalId = "60406df6-9174-4003-b4e8-80d9b15255dc";
        request.AydinlatmaMetniId = "d368d6d8-cc92-446f-befe-9e4e0f330db4";
        request.UavtCsbmKodu ??= request.UavtCaddeNo?.ToString();
        request.UavtIlKodu ??= request.UavtIlNo?.ToString();
        request.UavtIlceKodu ??= request.UavtIlceNo?.ToString();

        var response = await _httpService.PostAsync<EkleBasvuruCommandResponseModel, EkleBasvuruCommandModel>("BasvuruWeb/EkleGercekKisiBasvuru", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost("EkleTuzelKisiBasvuru")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> EkleTuzelKisiBasvuru([FromForm] EkleBasvuruCommandModel request)
    {
        request.BasvuruKanalId = "60406df6-9174-4003-b4e8-80d9b15255dc";
        request.AydinlatmaMetniId = "d368d6d8-cc92-446f-befe-9e4e0f330db4";
        request.UavtCsbmKodu ??= request.UavtCaddeNo?.ToString();
        request.UavtIlKodu ??= request.UavtIlNo?.ToString();
        request.UavtIlceKodu ??= request.UavtIlceNo?.ToString();

        var response = await _httpService.PostAsync<EkleBasvuruCommandResponseModel, EkleBasvuruCommandModel>("BasvuruWeb/EkleTuzelKisiBasvuru", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost("GuncelleBasvuru")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruDuzenle}")]
    public async Task<IActionResult> GuncelleBasvuru([FromForm] GuncelleBasvuruCommandModel request)
    {
        var response = await _httpService.PostAsync<GuncelleBasvuruCommandResponseModel, GuncelleBasvuruCommandModel>("BasvuruWeb/GuncelleBasvuru", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [Route("ekle")]
    [Route("duzenle/{basvuruGuid}/{tcKimlikNo}")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}, {RoleEnum.BasvuruDuzenle}")]
    public async Task<IActionResult> Islem(string? basvuruGuid, string? tcKimlikNo)
	{
        var responseBilgilendirme = await _httpService.GetAsync<List<GetirBilgilendirmeMesajByIdQueryResponseModel>>("BilgilendirmeMesajWeb/GetirListeBilgilendirmeMesaj");
        if (!responseBilgilendirme.ResultModel.IsError)
            ViewBag.BilgilendirmeMesajListe = responseBilgilendirme.ResultModel.Result;

        if (string.IsNullOrWhiteSpace(basvuruGuid) || string.IsNullOrWhiteSpace(tcKimlikNo))
        {
            ViewBag.Title = "Başvuru Ekle";
            return View();
        }

        ViewBag.Title = "Başvuru Düzenle";

        GetirBasvuruDetayByIdQueryModel request = new GetirBasvuruDetayByIdQueryModel()
        {
            BasvuruId = basvuruGuid,
            TcKimlikNo = tcKimlikNo,
        };

        var responseBasvuruDetay = await _httpService.GetAsync<GetirBasvuruDetayByIdQueryResponseModel>("BasvuruWeb/GetirBasvuruDetay", request);

        if (!responseBasvuruDetay.ResultModel.IsError)
            return View(responseBasvuruDetay.ResultModel.Result);

        return StatusCode(responseBasvuruDetay.StatusCode, responseBasvuruDetay.ResultModel.ErrorMessageContent);
    }
    
    [HttpGet("GetirBasvuruDetayPartial")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> GetirBasvuruDetayPartial([FromQuery] GetirBasvuruDetayByIdQueryModel request)
    {
        var response = await _httpService.GetAsync<GetirBasvuruDetayByIdQueryResponseModel>("BasvuruWeb/GetirBasvuruDetay", request);

        if (!response.ResultModel.IsError)
            return View("_BasvuruDetayPartial", response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirBasvuruDetay")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> GetirBasvuruDetay([FromQuery] GetirBasvuruDetayByIdQueryModel request)
	{
		var response = await _httpService.GetAsync<GetirBasvuruDetayByIdQueryResponseModel>("BasvuruWeb/GetirBasvuruDetay", request);

		if (!response.ResultModel.IsError)
			return Ok(response.ResultModel.Result);

		return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
	}

	[HttpPost("GetirBasvuruListeServerSide")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> GetirBasvuruListeServerSide([FromForm] GetirBasvuruListeServerSideQuery request)
	{
        var response = await _httpService.PostAsync<DataTableResponseModel<List<GetirBasvuruListeServerSideResponseModel>>, GetirBasvuruListeServerSideQuery>("BasvuruWeb/GetirBasvuruListeServerSide", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirAfadBasvuruListeByTcNo")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> GetirAfadBasvuruListeByTcNo([FromQuery] GetirAfadBasvuruListeByTcNoQuery request)
    {
        var response = await _httpService.GetAsync<List<AfadBasvuruDto>>("BasvuruWeb/GetirAfadBasvuruListeByTcNo", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirGostergePaneliVeri")]
    public async Task<IActionResult> GetirGostergePaneliVeri()
    {
        var response = await _httpService.GetAsync<GetirBasvuruGostergePaneliVeriQueryResponseModel>("BasvuruWeb/GetirBasvuruGostergePaneliVeri");

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirListeMalikler")]
    public async Task<IActionResult> GetirListeMalikler(GetirListeMaliklerQuery request)
    {
        var response = await _httpService.GetAsync<List<GetirListeMaliklerQueryResponseModel>>("BasvuruWeb/GetirListeMalikler", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }  
    [HttpGet("GetirListePasifMalikler")]
    public async Task<IActionResult> GetirListePasifMalikler(GetirListePasifMaliklerQuery request)
    {
        var response = await _httpService.GetAsync<List<GetirListePasifMaliklerQueryResponseModel>>("BasvuruWeb/GetirListePasifMalikler", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }    

    [HttpPost("BasvuruAdaParselGuncelle")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> BasvuruAdaParselGuncelle([FromForm] BasvuruAdaParselGuncelleCommandModel request)
    {
        var response = await _httpService.PostAsync<string, BasvuruAdaParselGuncelleCommandModel>("BasvuruWeb/BasvuruAdaParselGuncelle", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
}