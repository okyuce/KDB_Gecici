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
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBelediyeBasvuruListeServerSide;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class BelediyeController : Controller
{
    public readonly IHttpService _httpService;

    public BelediyeController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}")]
    public IActionResult Index()
    {
        return View();
    }

	[HttpPost("GetirBelediyeBasvuruListeServerSide")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BelediyeKullanicisi}")]
    public async Task<IActionResult> GetirBelediyeBasvuruListeServerSide([FromForm] GetirBelediyeBasvuruListeServerSideQuery request)
	{
        var response = await _httpService.PostAsync<DataTableResponseModel<List<GetirBelediyeBasvuruListeServerSideResponseModel>>, GetirBelediyeBasvuruListeServerSideQuery>("Belediye/GetirBelediyeBasvuruListeServerSide", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    
}