using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Csb.YerindeDonusum.WebApp.Services;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Queries;
using Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Commands; // Upload/DeleteDosyaCommands
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.CQRS;
using Csb.YerindeDonusum.Application.CQRS.Base;

namespace Csb.YerindeDonusum.WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("[area]/[controller]/[action]")]
    [Authorize]
    public class IstisnaAskiKoduController : Controller
    {
        private readonly IHttpService _httpService;
        private readonly string _apiName = "IstisnaAskiKodu";

        public IstisnaAskiKoduController(IHttpService httpService)
        {
            _httpService = httpService;
        }

        // Liste ekranı
        [HttpGet]
        [Authorize(Roles = "Admin,NaTamamYapi")]
        public IActionResult Index()
        {
            return View();
        }

        // DataTable ile liste getir
        [HttpPost]
        [Authorize(Roles = "Admin,NaTamamYapiListele")]
        public async Task<IActionResult> GetList([FromForm] DataTableRequestModel request)
        {
            var response = await _httpService.PostAsync<
                DataTableResponseModel<List<IstisnaAskiKoduListItem>>,
                GetListIstisnaAskiKoduQuery>(
                    $"{_apiName}/{nameof(GetList)}",
                    new GetListIstisnaAskiKoduQuery
                    {
                        draw = request.draw,
                        start = request.start,
                        length = request.length
                    }
            );
            return Ok(response.ResultModel.Result);
        }

        // Yeni ekle / güncelle
        [HttpPost]
        [Authorize(Roles = "Admin,NaTamamYapiEkle")]
        public async Task<IActionResult> CreateOrUpdate([FromForm] CreateIstisnaAskiKoduCommand model)
        {
            var response = await _httpService.PostAsync<
                CreateIstisnaAskiKoduResponseModel,
                CreateIstisnaAskiKoduCommand>(
                    $"{_apiName}/{nameof(CreateOrUpdate)}", model
            );
            if (!response.ResultModel.IsError)
                return Ok(response.ResultModel.Result);

            return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
        }

        // Dosya yükleme
        [HttpPost]
        [Authorize(Roles = "Admin,NaTamamYapiEkle")]
        public async Task<IActionResult> UploadFile(int istisnaAskiKoduId, IFormFile dosya)
        {
            if (dosya == null || dosya.Length == 0)
                return BadRequest("Dosya yüklenemedi.");

            using var content = new MultipartFormDataContent();
            using var stream = dosya.OpenReadStream();
            content.Add(new StreamContent(stream)
            {
                Headers = { ContentLength = dosya.Length, ContentType = new MediaTypeHeaderValue(dosya.ContentType) }
            }, "Dosya", dosya.FileName);
            content.Add(new StringContent(istisnaAskiKoduId.ToString()), "IstisnaAskiKoduId");

            var response = await _httpService.PostAsync<
                FileUploadResponseModel,
                UploadIstisnaAskiKoduDosyaCommand>(
                    $"{_apiName}/{nameof(UploadFile)}",
                    new UploadIstisnaAskiKoduDosyaCommand { IstisnaAskiKoduId = istisnaAskiKoduId, Dosya = dosya }
            );
            if (!response.ResultModel.IsError)
                return Ok(response.ResultModel.Result);

            return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
        }

        // Dosya silme
        [HttpPost]
        [Authorize(Roles = "Admin,NaTamamYapiSil")]
        public async Task<IActionResult> DeleteFile(int dosyaId)
        {
            var response = await _httpService.PostAsync<
                BaseCommandResponse,
                DeleteIstisnaAskiKoduDosyaCommand>(
                    $"{_apiName}/{nameof(DeleteFile)}",
                    new DeleteIstisnaAskiKoduDosyaCommand { Id = dosyaId }
            );
            if (!response.ResultModel.IsError)
                return Ok(true);

            return StatusCode(response.StatusCode, response.ResultModel.ErrorMessageContent);
        }
    }
}