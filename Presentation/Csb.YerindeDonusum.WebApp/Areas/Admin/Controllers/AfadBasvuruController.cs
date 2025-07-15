using Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirAfadBasvuruById;
using Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirListeAfadBasvuruIl;
using Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirListeAfadBasvuruIlce;
using Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirListeAfadBasvuruMahalle;
using Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirListeAfadBasvuruServerSide;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers;

[Area("admin")]
[Route("[area]/[controller]/")]
[Authorize]
public class AfadBasvuruController : Controller
{
    public readonly IHttpService _httpService;

    public AfadBasvuruController(IHttpService httpService)
    {
        _httpService = httpService;
    }

    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}, {RoleEnum.AfadBasvuruYoneticisi}")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("GetirListeIl")]
    public async Task<IActionResult> GetirListeIl([FromQuery] GetirListeAfadBasvuruIlQuery request)
    {
        var response = await _httpService.GetAsync<List<SelectDto<int>>>("AfadBasvuru/GetirListeIl", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirListeIlce")]
    public async Task<IActionResult> GetirListeIlce([FromQuery] GetirListeAfadBasvuruIlceQuery request)
    {
        var response = await _httpService.GetAsync<List<SelectDto<int>>>("AfadBasvuru/GetirListeIlce", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirListeMahalle")]
    public async Task<IActionResult> GetirListeMahalle([FromQuery] GetirListeAfadBasvuruMahalleQuery request)
    {
        var response = await _httpService.GetAsync<List<SelectDto<int>>>("AfadBasvuru/GetirListeMahalle", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpGet("GetirById")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.AfadBasvuruYoneticisi}")]
    public async Task<IActionResult> GetirById([FromQuery] GetirAfadBasvuruByIdQuery request)
    {
        var response = await _httpService.GetAsync<GetirAfadBasvuruByIdQueryResponseModel>("AfadBasvuru/GetirById", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }

    [HttpPost("GetirListeServerSide")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.AfadBasvuruYoneticisi}")]
    public async Task<IActionResult> GetirListeServerSide([FromForm] GetirListeAfadBasvuruServerSideQuery request)
    {
        var response = await _httpService.PostAsync<DataTableResponseModel<List<GetirListeAfadBasvuruServerSideQueryResponseModel>>, GetirListeAfadBasvuruServerSideQuery>("AfadBasvuru/GetirListeServerSide", request);

        if (!response.ResultModel.IsError)
            return Ok(response.ResultModel.Result);

        return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
    }
}