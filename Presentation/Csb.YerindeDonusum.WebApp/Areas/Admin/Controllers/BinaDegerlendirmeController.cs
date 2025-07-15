using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.AdaParselGuncelle;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetBelge;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetBinaDigerYardimlar;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetMuteahhitBilgileri;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetNakdiYardim;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetYapiDenetim;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetYapiRuhsat;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeDetay;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumu;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaIcinYapilanDigerYardimlar;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeBinaDegerlendirmeServerSide;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeOdemeBekleyenDegerlendirmeServerSide;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirNakdiYardimTaksitler;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirYapiDenetimSeviyeTespitBilgiler;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.IndirBinaNakdiYardimTaksitDosya;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.IndirBinaYapiRuhsatDosya;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.IndirYapiDenetimDosya;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeDosyaCQRS.Commands.YapiDenetimBelgeGuncelle;
using Csb.YerindeDonusum.Application.CQRS.MuteahhitCQRS.Queries.GetirYetkiBelgeNoIle;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class BinaDegerlendirmeController : Controller
{

    #region ...: Constructor Injection & Global Variables :...

    public readonly IHttpService _httpService;

    string apiControllerName = nameof(BinaDegerlendirmeController).Replace(nameof(Controller), "");

    public BinaDegerlendirmeController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion

    #region ...: Home Page :...

    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public IActionResult Index()
    {
        return Redirect("/admin/");
        return View();
    }

    #endregion

    #region ...: Commands :...

    [HttpPost(nameof(KaydetBelge))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> KaydetBelge([FromForm] KaydetBelgeCommand request)
    {
        var response = await _httpService.PostAsync<KaydetBelgeCommandResponseModel, KaydetBelgeCommand>
            ($"{apiControllerName}/{nameof(KaydetBelge)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost(nameof(KaydetYapiRuhsat))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> KaydetYapiRuhsat([FromForm] KaydetYapiRuhsatCommand request)
    {
        var response = await _httpService.PostAsync<KaydetYapiRuhsatCommandResponseModel, KaydetYapiRuhsatCommand>($"{apiControllerName}/{nameof(KaydetYapiRuhsat)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost(nameof(KaydetMuteahhitBilgileri))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> KaydetMuteahhitBilgileri([FromForm] KaydetMuteahhitBilgileriCommand request)
    {
        var response = await _httpService.PostAsync<string, KaydetMuteahhitBilgileriCommand>
                                                ($"{apiControllerName}/{nameof(KaydetMuteahhitBilgileri)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost(nameof(KaydetBinaDigerYardimlar))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> KaydetBinaDigerYardimlar([FromForm] KaydetBinaDigerYardimlarCommand request)
    {
        var response = await _httpService.PostAsync<string, KaydetBinaDigerYardimlarCommand>
            ($"{apiControllerName}/{nameof(KaydetBinaDigerYardimlar)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost(nameof(KaydetYapiDenetim))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> KaydetYapiDenetim([FromForm] KaydetYapiDenetimCommand request)
    {
        var response = await _httpService.PostAsync<string, KaydetYapiDenetimCommand>
            ($"{apiControllerName}/{nameof(KaydetYapiDenetim)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost(nameof(KaydetNakdiYardim))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    public async Task<IActionResult> KaydetNakdiYardim([FromForm] KaydetNakdiYardimCommand request)
    {
        var response = await _httpService.PostAsync<string, KaydetNakdiYardimCommand>
            ($"{apiControllerName}/{nameof(KaydetNakdiYardim)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
    #endregion

    #region ...: Queries :...


    [HttpGet(nameof(GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumu))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumu([FromQuery] GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQuery request)
    {
        var response = await _httpService.GetAsync<GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQueryResponseModel>($"{apiControllerName}/{nameof(GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumu)}", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet(nameof(GetirDetay))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> GetirDetay([FromQuery] GetirBinaDegerlendirmeDetayQuery request)
    {
        var response = await _httpService.GetAsync<GetirBinaDegerlendirmeDetayQueryResponseModel>($"{apiControllerName}/{nameof(GetirDetay)}", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet(nameof(GetirBinaIcinYapilanDigerYardimlar))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> GetirBinaIcinYapilanDigerYardimlar(GetirBinaIcinYapilanDigerYardimlarQuery request)
    {
        var response = await _httpService.GetAsync<List<GetirBinaIcinYapilanDigerYardimlarQueryResponseModel>>
            ($"{apiControllerName}/{nameof(GetirBinaIcinYapilanDigerYardimlar)}", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet(nameof(GetirYapiDenetimSeviyeTespitBilgiler))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> GetirYapiDenetimSeviyeTespitBilgiler(GetirYapiDenetimSeviyeTespitBilgilerQuery request)
    {
        var response = await _httpService.GetAsync<List<GetirYapiDenetimSeviyeTespitBilgilerQueryResponseModel>>
            ($"{apiControllerName}/{nameof(GetirYapiDenetimSeviyeTespitBilgiler)}", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet(nameof(GetirNakdiYardimTaksitler))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> GetirNakdiYardimTaksitler(GetirNakdiYardimTaksitlerQuery request)
    {
        var response = await _httpService.GetAsync<List<GetirNakdiYardimTaksitlerQueryResponseModel>>
            ($"{apiControllerName}/{nameof(GetirNakdiYardimTaksitler)}", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
    #endregion ...: Queries :...

    #region ...: List Methods

    [HttpPost(nameof(GetirListeOdemeBekleyenDegerlendirmeServerSide))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> GetirListeOdemeBekleyenDegerlendirmeServerSide([FromForm] GetirListeOdemeBekleyenDegerlendirmeServerSideQuery request)
    {
        var response = await _httpService.PostAsync<DataTableResponseModel<List<GetirListeOdemeBekleyenDegerlendirmeServerSideResponseModel>>, GetirListeOdemeBekleyenDegerlendirmeServerSideQuery>($"{apiControllerName}/{nameof(GetirListeOdemeBekleyenDegerlendirmeServerSide)}", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    } 
  

    [HttpPost(nameof(GetirListeBinaDegerlendirmeServerSide))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> GetirListeBinaDegerlendirmeServerSide([FromForm] GetirListeBinaDegerlendirmeServerSideQuery request)
    {
        var response = await _httpService.PostAsync<DataTableResponseModel<List<GetirListeBinaDegerlendirmeServerSideQueryResponseModel>>,
            GetirListeBinaDegerlendirmeServerSideQuery>
            ($"{apiControllerName}/{nameof(GetirListeBinaDegerlendirmeServerSide)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    #endregion

    #region ...: External Service Methods :...

    [HttpGet(nameof(GetirYetkiBelgeNoIle))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<IActionResult> GetirYetkiBelgeNoIle([FromQuery] GetirYetkiBelgeNoIleQuery request)
    {
        var response = await _httpService.GetAsync<GetirYetkiBelgeNoIleQueryResponseModel>
            ($"{apiControllerName}/{nameof(GetirYetkiBelgeNoIle)}", request);

        if (!response.ResultModel.IsError)
        {
            return Ok(response.ResultModel.Result);
        }

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    #endregion

    #region ...: Document Downloand :...

    [HttpPost(nameof(BelgeIndir))]
    [Route(nameof(BelgeIndir) + "/{binaDosyaGuid}")]
    public async Task<IActionResult> BelgeIndir(string binaDosyaGuid)
    {
        var belgeBilgileriResult = await _httpService
                    .GetAsync<IndirBinaYapiRuhsatDosyaQueryResponseModel>($"{apiControllerName}/GetirBelgeIndirBilgiler",
                     new IndirBinaYapiRuhsatDosyaQueryModel { BinaDosyaGuid = binaDosyaGuid });

        if (!belgeBilgileriResult.ResultModel.IsError)
        {
            return File(belgeBilgileriResult.ResultModel.Result?.File,
                        belgeBilgileriResult.ResultModel.Result?.MimeType,
                        belgeBilgileriResult.ResultModel.Result.DosyaAdi);
        }

        return StatusCode(belgeBilgileriResult.StatusCode, belgeBilgileriResult.ResultModel.ErrorMessageContent);
    }

    [HttpPost(nameof(BelgeIndirYapiRuhsat))]
    [Route(nameof(BelgeIndirYapiRuhsat) + "/{binaDosyaGuid}")]
    public async Task<IActionResult> BelgeIndirYapiRuhsat(string binaDosyaGuid)
    {
        var belgeBilgileriResult = await _httpService
                    .GetAsync<IndirBinaYapiRuhsatDosyaQueryResponseModel>($"{apiControllerName}/GetirBelgeIndirBilgilerYapiRuhsat",
                     new IndirBinaYapiRuhsatDosyaQueryModel { BinaDosyaGuid = binaDosyaGuid });

        if (!belgeBilgileriResult.ResultModel.IsError)
        {
            return File(belgeBilgileriResult.ResultModel.Result?.File,
                        belgeBilgileriResult.ResultModel.Result?.MimeType,
                        belgeBilgileriResult.ResultModel.Result.DosyaAdi);
        }

        return StatusCode(belgeBilgileriResult.StatusCode, belgeBilgileriResult.ResultModel.ErrorMessageContent);
    }

    [HttpPost(nameof(YapiDenetimBelgeIndir))]
    [Route(nameof(YapiDenetimBelgeIndir) + "/{binaYapiDenetimDosyaGuid}")]
    public async Task<IActionResult> YapiDenetimBelgeIndir(string binaYapiDenetimDosyaGuid)
    {
        var belgeBilgileriResult = await _httpService
                             .GetAsync<IndirYapiDenetimDosyaQueryResponseModel>($"{apiControllerName}/GetirYapiDenetimBelgeIndirBilgiler",
                             new IndirYapiDenetimDosyaQueryModel { BinaYapiDenetimDosyaGuid = binaYapiDenetimDosyaGuid });

        if (!belgeBilgileriResult.ResultModel.IsError)
        {
            return File(belgeBilgileriResult.ResultModel.Result?.File,
                        belgeBilgileriResult.ResultModel.Result?.MimeType,
                        belgeBilgileriResult.ResultModel.Result.DosyaAdi);
        }

        return StatusCode(belgeBilgileriResult.StatusCode, belgeBilgileriResult.ResultModel.ErrorMessageContent);
    }
    
    [HttpPost(nameof(YapiDenetimBelgeGuncelle))]
    [Route(nameof(YapiDenetimBelgeGuncelle))]
    public async Task<IActionResult> YapiDenetimBelgeGuncelle(YapiDenetimBelgeGuncelleCommand yapiDenetimBelgeGuncelleCommand)
    {
        var belgeBilgileriResult = await _httpService
                             .PostAsync<YapiDenetimBelgeGuncelleCommandResponseModel,YapiDenetimBelgeGuncelleCommand>($"{apiControllerName}/YapiDenetimBelgeGuncelle",
                             yapiDenetimBelgeGuncelleCommand);

        if (!belgeBilgileriResult.ResultModel.IsError)
        {
            return Ok(belgeBilgileriResult);
        }

        return StatusCode(belgeBilgileriResult.StatusCode, belgeBilgileriResult.ResultModel.ErrorMessageContent);
    }


    [HttpPost(nameof(BinaNakdiYardimTaksitDosyaIndir))]
    [Route(nameof(BinaNakdiYardimTaksitDosyaIndir) + "/{binaNakdiYardimTaksitDosyaGuid}")]
    public async Task<IActionResult> BinaNakdiYardimTaksitDosyaIndir(string binaNakdiYardimTaksitDosyaGuid)
    {
        var belgeBilgileriResult = await _httpService
                      .GetAsync<IndirBinaNakdiYardimTaksitDosyaQueryResponseModel>($"{apiControllerName}/GetirBinaNakdiYardimTaksitDosyaIndirBilgiler",
                      new IndirBinaNakdiYardimTaksitDosyaQueryModel { BinaNakdiYardimTaksitDosyaGuid = binaNakdiYardimTaksitDosyaGuid });

        if (!belgeBilgileriResult.ResultModel.IsError)
        {
            return File(belgeBilgileriResult.ResultModel.Result?.File,
                        belgeBilgileriResult.ResultModel.Result?.MimeType,
                        belgeBilgileriResult.ResultModel.Result.DosyaAdi);
        }

        return StatusCode(belgeBilgileriResult.StatusCode, belgeBilgileriResult.ResultModel.ErrorMessageContent);
    }

    #endregion ...: Document Downloand :...

}