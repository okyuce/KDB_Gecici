using Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.WebApp.Models;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Csb.YerindeDonusum.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly OptionsModel _options;
    private readonly IHttpService _httpService;

    public HomeController(IHttpService httpService, IOptions<OptionsModel> options)
    {
        _httpService = httpService;
        _options = options.Value;
    }

    public IActionResult Index()
    {
        var response = _httpService.GetAsync<List<OfisKonumDto>>("OfisKonum/GetirOfisKonumListe").Result;

        var groupped = response?.ResultModel?.Result?.OrderBy(x=> x.IlAdi)?.GroupBy(p => p.IlAdi)?
                              .Select(g => new IllerViewModel()
                              {
                                  IlAdi = g?.First()?.IlAdi,
                                  Ilceler = g?.OrderBy(y => y.IlceAdi)?.Select(y => new OfisKonumDto
                                  {
                                      IlceAdi = y.IlceAdi,
                                  })?.ToList(),
                              })?.ToList();

        return View(groupped);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Route("nasil-basvurulur")]
    public IActionResult NasilBasvurulur()
    {
        return View();
    } 

    [Route("get-locations")]

    public async Task<IActionResult> GetLocations()
    {
        var response = await _httpService.GetAsync<List<OfisKonumDto>>("OfisKonum/GetirOfisKonumListe");

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [Route("get-sss")]
    public async Task<IActionResult> GetSss(GetirSikcaSorulanSoruListeQuery request)
    {
        var response = await _httpService.GetAsync<List<SikcaSorulanSoruDto>>("SikcaSorulanSoru/GetirSikcaSorulanSoruListe", request);

        if(!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [Route("SSS")]
    public async Task<IActionResult> SSS()
    {
        // (await GetSss(new GetirSikcaSorulanSoruListeQuery())).Result
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}