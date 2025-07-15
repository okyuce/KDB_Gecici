using Csb.YerindeDonusum.Application.CQRS.BasvuruAfadDurumCQRS;
using Csb.YerindeDonusum.Application.CQRS.BasvuruDurumCQRS;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeDurumCQRS.Queries.GetirBinaOdemeDurumListe;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiDisKapiNo;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.WebApp.Models;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class SelectController : Controller
{
	public readonly IHttpService _httpService;
	private readonly OptionsModel _options;

	public SelectController(IHttpService httpService, IOptions<OptionsModel> options)
	{
		_httpService = httpService;
		_options = options.Value;
	}

	public IActionResult Index()
	{
		return View();
    }

    [HttpGet("GetirBasvuruHasarDurumuListe")]
    public async Task<IActionResult> GetirBasvuruHasarDurumuListe()
    {
        var response = await _httpService.GetAsync<List<BasvuruHasarDurumuDto>>("Select/GetirBasvuruHasarDurumuListe");

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirBinaDegerlendirmeDurumListe")]
    public async Task<IActionResult> GetirBinaDegerlendirmeDurumListe()
    {
        var response = await _httpService.GetAsync<List<BasvuruDurumDto>>("Select/GetirBinaDegerlendirmeDurumListe");

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirBasvuruTurListe")]
	public async Task<IActionResult> GetirBasvuruTurListe()
	{
		var response = await _httpService.GetAsync<List<BasvuruTurCsbDto>> ("Select/GetirBasvuruTurListe");

		if (!response.ResultModel.IsError)
			return Ok(response.ResultModel.Result);

		return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirBasvuruDestekTurListe")]
	public async Task<IActionResult> GetirBasvuruDestekTurListe()
	{
		var response = await _httpService.GetAsync<List<BasvuruDestekTurCsbDto>> ("Select/GetirBasvuruDestekTurListe");

		if (!response.ResultModel.IsError)
			return Ok(response.ResultModel.Result);

		return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }



    [HttpGet("GetirBasvuruIptalTurListe")]
    public async Task<IActionResult> GetirBasvuruIptalTurListe()
    {
        var response = await _httpService.GetAsync<List<BasvuruIptalTurDto>>("Select/GetirBasvuruIptalTurListe");

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirBasvuruDurumListe")]
    public async Task<IActionResult> GetirBasvuruDurumListe()
    {
        var response = await _httpService.GetAsync<List<BasvuruDurumDto>>("BasvuruWeb/GetirBasvuruDurumListe", new GetirBasvuruDurumListeQuery());

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirBinaOdemeDurumListe")]
    public async Task<IActionResult> GetirBinaOdemeDurumListe()
    {
        var response = await _httpService.GetAsync<List<DurumDto>>("Select/GetirBinaOdemeDurumListe", new GetirBinaOdemeDurumListeQuery());

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirListeYeniYapiDisKapiNo")]
    public async Task<IActionResult> GetirListeYeniYapiDisKapiNo([FromQuery] GetirListeYeniYapiDisKapiNoQuery request)
    {
        var response = await _httpService.GetAsync<List<SelectDto<string>>>("Select/GetirListeYeniYapiDisKapiNo", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirBasvuruAfadDurumListe")]
    public async Task<IActionResult> GetirBasvuruAfadDurumListe()
    {
        var response = await _httpService.GetAsync<List<BasvuruDurumDto>>("BasvuruWeb/GetirBasvuruAfadDurumListe", new GetirBasvuruAfadDurumListeQuery());

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirBasvuruKanalListe")]
	public async Task<IActionResult> GetirBasvuruKanalListe()
	{
		var response = await _httpService.GetAsync<List<BasvuruKanalDto>> ("Select/GetirBasvuruKanalListe");

		if (!response.ResultModel.IsError)
			return Ok(response.ResultModel.Result);

		return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirTuzelKisilikTipiListe")]
    public async Task<IActionResult> GetirTuzelKisilikTipiListe()
    {
        var response = await _httpService.GetAsync<List<TuzelKisilikTipiDto>>("Select/GetirTuzelKisilikTipiListe");

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
}