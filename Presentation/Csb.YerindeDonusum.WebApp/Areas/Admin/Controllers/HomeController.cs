using Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciGiris;
using Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciGirisDogrula;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Cryptography;
using Csb.YerindeDonusum.WebApp.Services;
using Csb.YerindeDonusum.WebApp.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/")]
[Authorize]
public class HomeController : Controller
{
    public readonly IHttpService _httpService;
    public readonly IAuthService _authService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public HomeController(IHttpService httpService, IAuthService authService, IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
        _httpService = httpService;
        _authService = authService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Route("cikis")]
    public async Task<IActionResult> Cikis()
    {
        await _authService.SignOutAsync();

        return RedirectToAction("giris");
    }

    [AllowAnonymous]
    [Route("erisimYetkisiYok")]
    public IActionResult ErisimYetkisiYok()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("getirGuvenlikKodu")]
    public IActionResult GetirGuvenlikKodu()
    {
        using (var stream = new MemoryStream())
        {
            string captchaText = CapthaHelper.GenerateRandomCode();
            CapthaHelper.GenerateImage(captchaText: captchaText).Save(stream, ImageFormat.Jpeg);
            return Ok(new
            {
                Base64 = Convert.ToBase64String(stream.ToArray()),
                Key = StringCipher.Encrypt(string.Concat(captchaText, ";", DateTime.Now.ToString("yyMMddHHmm")))
            });
        }
    }

    [AllowAnonymous]
    [Route("giris")]
    public IActionResult Giris()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("giris")]
    public async Task<IActionResult> Giris([FromForm] KullaniciGirisCommand request)
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

        var response = await _httpService.PostAsync<KullaniciGirisKodDto, KullaniciGirisCommand>("Hesap/Giris", request);
        if (!response.ResultModel.IsError)
        {
            //return Ok("Giriş başarılı");
            return Ok(response.ResultModel.Result.GirisGuid);
        }
        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }


    [AllowAnonymous]
    [HttpPost]
    [Route("girisDogrula")]
    public async Task<IActionResult> GirisDogrula([FromForm] KullaniciGirisDogrulaCommand request)
    {
        var response = await _httpService.PostAsync<TokenDto, KullaniciGirisDogrulaCommand>("Hesap/GirisDogrula", request);
        if (!response.ResultModel.IsError)
        {
            await _authService.SignInAsync(response.ResultModel.Result);
            return Ok(response.ResultModel.Result.AccessToken);
        }
        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
}