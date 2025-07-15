using Csb.YerindeDonusum.Application.CQRS.BirimCQRS;
using Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS;
using Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS.Commands.EkleKullanici;
using Csb.YerindeDonusum.Application.CQRS.KullaniciHesapTipCQRS;
using Csb.YerindeDonusum.Application.CQRS.RolCQRS;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Domain.Cryptography;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/kullanici/")]
[Authorize]
public class KullaniciController : Controller
{
    public readonly IHttpService _httpService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public KullaniciController(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IHttpService httpService)
    {
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _httpService = httpService;
    }

    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.KullanicilarYoneticisi}")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("getiridile")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.KullanicilarYoneticisi}")]
    public async Task<IActionResult> GetirIdIle([FromQuery] GetirKullaniciIdIleQuery request)
    {
        var response = await _httpService.GetAsync<KullaniciDto>("Kullanici/GetirIdIle", request);
        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);
        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("getirListe")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.KullanicilarYoneticisi}")]
    public async Task<IActionResult> GetirListe([FromQuery] GetirListeKullaniciQueryModel request)
    {
        var response = await _httpService.GetAsync<List<KullaniciListeDto>>("Kullanici/GetirListe", request);
        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);
        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost("ekle")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.KullanicilarYoneticisi}")]
    public async Task<IActionResult> Ekle([FromForm] EkleKullaniciCommand request)
    {
        var res = await _httpService.PostAsync<EkleKullaniciCommandResponseModel, EkleKullaniciCommand>("Kullanici/EkleKullanici", request);
        if (!res.ResultModel.IsError)
            return Ok(res.ResultModel.Result);

        return BadRequest(res.ResultModel.ErrorMessageContent);
    }

    [HttpPost("guncelle")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.KullanicilarYoneticisi}")]
    public async Task<IActionResult> Guncelle([FromForm] GuncelleKullaniciCommand request)
    {
        var res = await _httpService.PostAsync<GuncelleKullaniciCommandResponseModel, GuncelleKullaniciCommand>("Kullanici/GuncelleKullanici", request);
        if (!res.ResultModel.IsError)
            return Ok(res.ResultModel.Result);

        return BadRequest(res.ResultModel.ErrorMessageContent);
    }
    
    [HttpPost("sifremiUnuttum")]
    [AllowAnonymous]
    public async Task<IActionResult> SifremiUnuttum([FromForm] SifremiUnuttumKullaniciCommand request)
    {
        try
        {
            if (!_webHostEnvironment.IsDevelopment())
            {
                if (string.IsNullOrWhiteSpace(request?.GuvenlikKodu))
                    return BadRequest("Lütfen güvenlik kodunu girin!");
                else if (string.IsNullOrWhiteSpace(request?.GuvenlikKoduKey))
                    return BadRequest("Hatalı istek yapıldı!");

                var cozulmusKey = StringCipher.Decrypt(request.GuvenlikKoduKey).Split(";");

                var guvenlikKoduDogruMu = request.GuvenlikKodu.ToUpper().Trim() == cozulmusKey[0];
                if (!guvenlikKoduDogruMu)
                    return BadRequest("Doğrulama kodu hatalı girildi!");

                var keyTarihi = DateTime.ParseExact(cozulmusKey[1], "yyMMddHHmm", System.Globalization.CultureInfo.InvariantCulture);
                if ((DateTime.Now - keyTarihi).Minutes > 5)
                    return BadRequest("Güvenlik kodu eski, lütfen tekrar deneyin.");
            }
        }
        catch (Exception)
        {
            return BadRequest("Doğrulama kodu kontrol edilirken hata oluştu!");
        }
        var res = await _httpService.PostAsync<SifremiUnuttumKullaniciCommandResponseModel, SifremiUnuttumKullaniciCommand>("Kullanici/SifremiUnuttum", request);
        if (!res.ResultModel.IsError)
            return Ok(res.ResultModel.Result);

        return BadRequest(res.ResultModel.ErrorMessageContent);
    }

    [HttpPost("sifreDegistir")]
    public async Task<IActionResult> SifreDegistir([FromForm] SifreDegistirKullaniciCommand request)
    {
        var res = await _httpService.PostAsync<SifreDegistirKullaniciCommandResponseModel, SifreDegistirKullaniciCommand>("Kullanici/SifreDegistir", request);
        if (res.ResultModel.IsError)
            return BadRequest(res.ResultModel.ErrorMessageContent);

        #region Local Kullanıcı İse Son Şifre Değiştirme Tarihini Setle
        if (_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "KullaniciHesapTipId")?.Value == KullaniciHesapTipEnum.Local.GetHashCode().ToString())
        {
            var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity!;
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst("SonSifreDegisimTarihi"));
            claimsIdentity.AddClaim(new Claim("SonSifreDegisimTarihi", DateTime.Now.ToString("yyyy-MM-dd")));
        }
        #endregion

        return Ok(res.ResultModel.Result);
    }

    [HttpPost("sil")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.KullanicilarYoneticisi}")]
    public async Task<IActionResult> Sil([FromForm] SilKullaniciCommand request)
    {
        var res = await _httpService.PostAsync<SilKullaniciCommandResponseModel, SilKullaniciCommand>("Kullanici/SilKullanici", request);
        if (!res.ResultModel.IsError)
            return Ok(res.ResultModel.Result);

        return BadRequest(res.ResultModel.ErrorMessageContent);
    }

    [HttpGet("getirlisteKullaniciHesapTip")]
    public async Task<IActionResult> GetirlisteKullaniciHesapTip([FromQuery] GetirKullaniciHesapTipListeQuery request)
    {
        var response = await _httpService.GetAsync<List<KullaniciHesapTipDto>>("KullaniciHesapTip/GetirKullaniciHesapTipListe", request);
        //response.ResultModel.Result = response.ResultModel.Result.Where(x => x.KullaniciHesapTipId == 1).ToList();// Sadece LDAP Gelsin diye
        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("getirlisteBirim")]
    public async Task<IActionResult> GetirlisteBirim([FromQuery] GetirBirimListeQuery request)
    {
        var response = await _httpService.GetAsync<List<BirimDto>>("Birim/GetirBirimListe", request);
        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);
        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("getirlisteRol")]
    public async Task<IActionResult> GetirlisteRol([FromQuery] GetirRolListeQuery request)
    {
        var response = await _httpService.GetAsync<List<RolDto>>("Rol/GetirRolListe", request);
        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
}