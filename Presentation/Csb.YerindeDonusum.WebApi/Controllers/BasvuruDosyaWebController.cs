using Csb.YerindeDonusum.Application.CQRS.BasvuruDosyaCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeDosyaCQRS.Commands;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models;
using CSB.Core.LogHandler.Attr;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BasvuruDosyaWebController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasvuruDosyaWebController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("BasvuruDosyaIndir")]
        [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}, {RoleEnum.BasvuruDuzenle}, {RoleEnum.BasvuruListele}")]
        [AddInfoLog(PassResponse = true)]
        public async Task<ResultModel<BasvuruDosyaIndirCommandResponseModel>> BasvuruDosyaIndir(BasvuruDosyaIndirCommand request)
        {
            return await _mediator.Send(request);
        } 
        
        [HttpPost("BinaDegerlendirmeDosyaIndir")]
        [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}, {RoleEnum.BasvuruDuzenle}, {RoleEnum.BasvuruListele}")]
        [AddInfoLog(PassResponse = true)]
        public async Task<ResultModel<BinaDegerlendirmeDosyaIndirCommandResponseModel>> BinaDegerlendirmeDosyaIndir(BinaDegerlendirmeDosyaIndirCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}