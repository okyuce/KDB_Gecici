using Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS;
using Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS.Commands.EkleSikcaSorulanSoru;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.WebApp.Models;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class SSSController : Controller
{
	public readonly IHttpService _httpService;
	private readonly OptionsModel _options;

	public SSSController(IHttpService httpService, IOptions<OptionsModel> options)
	{
		_httpService = httpService;
		_options = options.Value;
	}

    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.SikcaSorulanSoruYoneticisi}")]
    public IActionResult Index()
	{
		return View();
    }

	[HttpGet("getiridile")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.SikcaSorulanSoruYoneticisi}")]
    public async Task<IActionResult> GetirIdIle([FromQuery] GetirSSSIdIleQuery request)
	{
		var response = await _httpService.GetAsync<SikcaSorulanSoruDto>("SikcaSorulanSoru/GetirIdIle", request);

		if (!response.ResultModel.IsError)
			return Ok(response.ResultModel.Result);

		return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
	}

	[HttpGet("getirliste")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.SikcaSorulanSoruYoneticisi}")]
    public async Task<IActionResult> GetirListe([FromQuery] GetirSikcaSorulanSoruListeQuery request)
	{
		var response = await _httpService.GetAsync<List<SikcaSorulanSoruDto>>("SikcaSorulanSoru/GetirSikcaSorulanSoruListe", request);
		
		if (!response.ResultModel.IsError)
			return Ok(response.ResultModel.Result);
		
		return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
	}

    [HttpPost("getirListeServerSide")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.SikcaSorulanSoruYoneticisi}")]
    public async Task<IActionResult> GetirListeServerSide([FromForm] GetirListeSikcaSorulanSoruListeServerSideQueryModel Model)
    {
        var response = await _httpService.PostAsync<DataTableResponseModel<List<SikcaSorulanSoruServerSideDto>>, GetirListeSikcaSorulanSoruListeServerSideQueryModel>("SikcaSorulanSoru/GetirListeSikcaSorulanSoruListeServerSide", Model);
            
		if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

		return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost("ekle")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.SikcaSorulanSoruYoneticisi}")]
    public async Task<IActionResult> Ekle([FromForm] EkleSikcaSorulanSoruCommand request)
	{
		var response = await _httpService.PostAsync<EkleSikcaSorulanSoruCommandResponseModel, EkleSikcaSorulanSoruCommand>("SikcaSorulanSoru/EkleSikcaSorulanSoru", request);

		if (!response.ResultModel.IsError)
			return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

	[HttpPost("guncelle")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.SikcaSorulanSoruYoneticisi}")]
    public async Task<IActionResult> Guncelle([FromForm] GuncelleSikcaSorulanSoruCommand request)
	{
		var response = await _httpService.PostAsync<GuncelleSikcaSorulanSoruCommandResponseModel,
										GuncelleSikcaSorulanSoruCommand>("SikcaSorulanSoru/GuncelleSikcaSorulanSoru", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

	[HttpPost("sil")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.SikcaSorulanSoruYoneticisi}")]
    public async Task<IActionResult> Sil([FromForm] SilSikcaSorulanSoruCommand request)
	{
		var response = await _httpService.PostAsync<SilSikcaSorulanSoruCommandResponseModel,
										SilSikcaSorulanSoruCommand>("SikcaSorulanSoru/SilSikcaSorulanSoru", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
}