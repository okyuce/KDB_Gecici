using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirListeTebligatMalikler;
using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Commands.TebligatGonder;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirTebligatGonderimDetayDosyaByDetayId;
using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirTebligatGonderimDetayDosyaByGuid;
using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Commands.KaydetTebligatDonderimDetayDosya;
using System.Net;
using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirTebligatGonderimDetayById;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class TebligatController : Controller
{
    public readonly IHttpService _httpService;

    public TebligatController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}, {RoleEnum.Tebligat}")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("GetirListeTebligatMalikler")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}, {RoleEnum.Tebligat}")]
    public async Task<IActionResult> GetirListeTebligatMalikler(GetirListeTebligatMaliklerQuery request)
    {
        var response = await _httpService.PostAsync<List<GetirListeTebligatMaliklerQueryResponseModel>, GetirListeTebligatMaliklerQuery>("Tebligat/GetirListeTebligatMalikler", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost("TebligatGonder")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}, {RoleEnum.Tebligat}")]
    public async Task<IActionResult> TebligatGonder([FromForm] TebligatGonderCommand request)
    {
        request.TebligatYapilacaklar = request.TebligatYapilacaklar.Select(m => { m.TebligTarihi = DateTime.Now; return m; }).ToList();
        var response = await _httpService.PostAsync<string, TebligatGonderCommand>("Tebligat/TebligatGonder", request);
        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirTebligatGonderimDetayDosyaByDetayId")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}, {RoleEnum.Tebligat}")]
    public async Task<IActionResult> GetirTebligatGonderimDetayDosyaByDetayId(GetirTebligatGonderimDetayDosyaByDetayIdQuery request)
    {
        var response = await _httpService.GetAsync<TebligatGonderimDetayDosyaDto>("Tebligat/GetirTebligatGonderimDetayDosyaByDetayId", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [Route("BelgeIndirTebligatGonderimDetayDosya/{dosyaGuid}")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}, {RoleEnum.Tebligat}")]
    public async Task<IActionResult> BelgeIndirTebligatGonderimDetayDosya(Guid dosyaGuid)
    {
        var response = await _httpService.GetAsync<GetirTebligatGonderimDetayDosyaByGuidQueryResponseModel>("Tebligat/GetirTebligatGonderimDetayDosyaByGuid", new GetirTebligatGonderimDetayDosyaByGuidQuery
        {
            DosyaGuid = dosyaGuid
        });

        if (!response.ResultModel.IsError)
            return File(response.ResultModel.Result.Icerik, response.ResultModel.Result.Tur, response.ResultModel.Result.Ad);

        return BadRequest(response.ResultModel.ErrorMessageContent);
    }
    
    [Route("AnlasmayaIstirakTebligatTaslakSozlesme")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}, {RoleEnum.Tebligat}")]
    public async Task<IActionResult> AnlasmayaIstirakTebligatTaslakSozlesme(GetirTebligatGonderimDetayByIdQueryResponseModel tebligatGonderimResponseModel)
    {
        tebligatGonderimResponseModel.TebligTarihi = DateTime.Now;
        return View(tebligatGonderimResponseModel);

        //GetirTebligatGonderimDetayByIdQueryResponseModel? data = null;
        //var response = await _httpService.GetAsync<GetirTebligatGonderimDetayByIdQueryResponseModel>("Tebligat/GetirTebligatGonderimDetayById", new GetirTebligatGonderimDetayByIdQuery { TebligatGonderimDetayId = tebligatGonderimDetayId });

        //if (!response.ResultModel.IsError)
        //    data = response.ResultModel.Result;
        //else
        //    ViewBag.HataMesaji = response.StatusCode == (int)HttpStatusCode.BadRequest ? response.ResultModel.ErrorMessageContent : "Bilinmeyen bir hata oluştu!";

        //return View(data);
    }

    [Route("AnahtarTeslimTebligatTaslakSozlesme")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}, {RoleEnum.Tebligat}")]
    public async Task<IActionResult> AnahtarTeslimTebligatTaslakSozlesme(GetirTebligatGonderimDetayByIdQueryResponseModel tebligatGonderimResponseModel)
    {
        tebligatGonderimResponseModel.TebligTarihi = DateTime.Now;
        return View(tebligatGonderimResponseModel);

        //GetirTebligatGonderimDetayByIdQueryResponseModel? data = null;
        //var response = await _httpService.GetAsync<GetirTebligatGonderimDetayByIdQueryResponseModel>("Tebligat/GetirTebligatGonderimDetayById", new GetirTebligatGonderimDetayByIdQuery { TebligatGonderimDetayId = tebligatGonderimDetayId });

        //if (!response.ResultModel.IsError)
        //    data = response.ResultModel.Result;
        //else
        //    ViewBag.HataMesaji = response.StatusCode == (int)HttpStatusCode.BadRequest ? response.ResultModel.ErrorMessageContent : "Bilinmeyen bir hata oluştu!";

        //return View(data);
    }

    [HttpPost]
    [Route("KaydetTebligatGonderimDetayDosya")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}, {RoleEnum.Tebligat}")]
    public async Task<IActionResult> KaydetTebligatGonderimDetayDosya([FromForm] KaydetTebligatGonderimDetayDosyaCommand request)
    {
        var response = await _httpService.PostAsync<TebligatGonderimDetayDosyaDto, KaydetTebligatGonderimDetayDosyaCommand>("Tebligat/KaydetTebligatGonderimDetayDosya", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
}