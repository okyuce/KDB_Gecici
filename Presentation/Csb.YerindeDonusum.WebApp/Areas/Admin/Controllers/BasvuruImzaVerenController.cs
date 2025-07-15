using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVeren;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVerenAfadBelge;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVerenSozlesme;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruKendimKarsilamakIstiyorum;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Queries.GetirBasvuruImzaByBasvuruId;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Queries.GetirBasvuruImzaSozlesmeDosyaByGuid;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirDetayHibeTaahhutnameSozlesme;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirDetaySozlesmeGeriOdemeBilgileri;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirDetayYerindeYapimKrediSozlesme;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirBasvuruImzaSozlesmeDosyaByGuid;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class BasvuruImzaVerenController : Controller
{
    public readonly IHttpService _httpService;
    string apiControllerName = nameof(BasvuruImzaVerenController).Replace(nameof(Controller), "");

    public BasvuruImzaVerenController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    [Route("HibeTaahhutnameSozlesme/{basvuruGuid}")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> HibeTaahhutnameSozlesme(Guid basvuruGuid)
    {
        GetirDetayHibeTaahhutnameSozlesmeQueryResponseModel? data = null;
        var response = await _httpService.GetAsync<GetirDetayHibeTaahhutnameSozlesmeQueryResponseModel>("BasvuruImzaVeren/GetirDetayHibeTaahhutnameSozlesme", new GetirDetayHibeTaahhutnameSozlesmeQuery { BasvuruGuid = basvuruGuid });

        if (!response.ResultModel.IsError)
            data = response.ResultModel.Result;
        else
            ViewBag.HataMesaji = response.StatusCode == (int)HttpStatusCode.BadRequest ? response.ResultModel.ErrorMessageContent : "Bilinmeyen bir hata oluştu!";

        return View(data);
    }

    [Route("YerindeYapimKrediSozlesme/{basvuruGuid}")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> YerindeYapimKrediSozlesme(Guid basvuruGuid)
    {
        GetirDetayYerindeYapimKrediSozlesmeQueryResponseModel? data = null;
        var response = await _httpService.GetAsync<GetirDetayYerindeYapimKrediSozlesmeQueryResponseModel>("BasvuruImzaVeren/GetirDetayYerindeYapimKrediSozlesme", new GetirDetayYerindeYapimKrediSozlesmeQuery { BasvuruGuid = basvuruGuid });

        if (!response.ResultModel.IsError)
            data = response.ResultModel.Result;
        else
            ViewBag.HataMesaji = response.StatusCode == (int)HttpStatusCode.BadRequest ? response.ResultModel.ErrorMessageContent : "Bilinmeyen bir hata oluştu!";

        return View(data);
    }

    [HttpGet]
    [Route(nameof(GetirBasvuruImzaByBasvuruId))]
    public async Task<IActionResult> GetirBasvuruImzaByBasvuruId([FromQuery] GetirBasvuruImzaByBasvuruIdQuery request)
    {
        var response = await _httpService.GetAsync<BasvuruImzaVerenDto>($"{apiControllerName}/{nameof(GetirBasvuruImzaByBasvuruId)}", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost]
    [Route(nameof(KaydetBasvuruImzaVeren))]
    public async Task<IActionResult> KaydetBasvuruImzaVeren([FromForm] KaydetBasvuruImzaVerenCommand request)
    {
        var response = await _httpService.PostAsync<BasvuruImzaVerenDto, KaydetBasvuruImzaVerenCommand>($"{apiControllerName}/{nameof(KaydetBasvuruImzaVeren)}", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }    
    
    [HttpPost]
    [Route(nameof(KaydetBasvuruKendimKarsilamakIstiyorum))]
    public async Task<IActionResult> KaydetBasvuruKendimKarsilamakIstiyorum([FromForm] KaydetBasvuruKendimKarsilamakIstiyorumCommand request)
    {
        var response = await _httpService.PostAsync<BasvuruImzaVerenDto, KaydetBasvuruKendimKarsilamakIstiyorumCommand>($"{apiControllerName}/{nameof(KaydetBasvuruKendimKarsilamakIstiyorum)}", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost]
    [Route(nameof(KaydetBasvuruImzaVerenSozlesme))]
    public async Task<IActionResult> KaydetBasvuruImzaVerenSozlesme([FromForm] KaydetBasvuruImzaVerenSozlesmeCommand request)
    {
        var response = await _httpService.PostAsync<BasvuruImzaVerenDto, KaydetBasvuruImzaVerenSozlesmeCommand>($"{apiControllerName}/{nameof(KaydetBasvuruImzaVerenSozlesme)}", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost]
    [Route(nameof(KaydetBasvuruImzaVerenAfadBelge))]
    public async Task<IActionResult> KaydetBasvuruImzaVerenAfadBelge([FromForm] KaydetBasvuruImzaVerenAfadBelgeCommand request)
    {
        var response = await _httpService.PostAsync<string, KaydetBasvuruImzaVerenAfadBelgeCommand>($"{apiControllerName}/{nameof(KaydetBasvuruImzaVerenAfadBelge)}", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [Route("SozlesmeGeriOdemeBilgileri/{basvuruGuid}")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> SozlesmeGeriOdemeBilgileri(Guid basvuruGuid)
    {
        GetirDetaySozlesmeGeriOdemeBilgileriQueryResponseModel? data = null;
        var response = await _httpService.GetAsync<GetirDetaySozlesmeGeriOdemeBilgileriQueryResponseModel>("BasvuruImzaVeren/GetirDetaySozlesmeGeriOdemeBilgileri", new GetirDetaySozlesmeGeriOdemeBilgileriQuery { BasvuruGuid = basvuruGuid });

        if (!response.ResultModel.IsError)
            data = response.ResultModel.Result;
        else
            ViewBag.HataMesaji = response.StatusCode == (int)HttpStatusCode.BadRequest ? response.ResultModel.ErrorMessageContent : "Bilinmeyen bir hata oluştu!";

        return View(data);
    }

    [Route("BelgeIndirKrediSozlesme/{dosyaGuid}")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> BelgeIndirKrediSozlesme(Guid dosyaGuid)
    {
        var response = await _httpService.GetAsync<GetirBasvuruImzaSozlesmeDosyaByGuidResponseModel>("BasvuruImzaVeren/GetirBasvuruImzaSozlesmeDosyaByGuid", new GetirBasvuruImzaSozlesmeDosyaByGuidQuery
        {
            DosyaTurId = (long)BasvuruImzaVerenDosyaTurEnum.KrediSozlesmesi,
            DosyaGuid = dosyaGuid
        });

        if (!response.ResultModel.IsError)
            return File(response.ResultModel.Result.Icerik, response.ResultModel.Result.Tur, response.ResultModel.Result.Ad);

        return BadRequest(response.ResultModel.ErrorMessageContent);
    }

    [Route("BelgeIndirHibeSozlesme/{dosyaGuid}")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> BelgeIndirHibeSozlesme(Guid dosyaGuid)
    {
        var response = await _httpService.GetAsync<GetirBasvuruImzaSozlesmeDosyaByGuidResponseModel>("BasvuruImzaVeren/GetirBasvuruImzaSozlesmeDosyaByGuid", new GetirBasvuruImzaSozlesmeDosyaByGuidQuery
        {
            DosyaTurId = (long)BasvuruImzaVerenDosyaTurEnum.HibeOnayi,
            DosyaGuid = dosyaGuid
        });

        if (!response.ResultModel.IsError)
            return File(response.ResultModel.Result.Icerik, response.ResultModel.Result.Tur, response.ResultModel.Result.Ad);

        return BadRequest(response.ResultModel.ErrorMessageContent);
    }
    
    [Route("BelgeIndirFeragatname/{dosyaGuid}")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> BelgeIndirFeragatname(Guid dosyaGuid)
    {
        var response = await _httpService.GetAsync<GetirBasvuruImzaSozlesmeDosyaByGuidResponseModel>("BasvuruImzaVeren/GetirBasvuruImzaSozlesmeDosyaByGuid", new GetirBasvuruImzaSozlesmeDosyaByGuidQuery
        {
            DosyaTurId = (long)BasvuruImzaVerenDosyaTurEnum.TaahhutnameBelgesi,
            DosyaGuid = dosyaGuid
        });

        if (!response.ResultModel.IsError)
            return File(response.ResultModel.Result.Icerik, response.ResultModel.Result.Tur, response.ResultModel.Result.Ad);

        return BadRequest(response.ResultModel.ErrorMessageContent);
    }
}