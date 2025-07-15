using Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS;
using Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS.Commands.EkleKullanici;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using CSB.Core.LogHandler.Attr;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class KullaniciController : ControllerBase
{
    private readonly IMediator _mediator;

    public KullaniciController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetirKullaniciListe")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.KullanicilarYoneticisi}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<List<KullaniciDto>>> GetirKullaniciListe([FromQuery] GetirKullaniciListeQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirListe")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.KullanicilarYoneticisi}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<List<KullaniciListeDto>>> GetirListe([FromQuery] GetirListeKullaniciQueryModel request)
    {
        return await _mediator.Send(new GetirListeKullaniciQuery { Model = request });
    }

    [HttpGet("GetirIdIle")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.KullanicilarYoneticisi}")]
    [AddInfoLog]
    public async Task<ResultModel<KullaniciDto>> GetirIdIle([FromQuery] GetirKullaniciIdIleQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("EkleKullanici")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.KullanicilarYoneticisi}")]
    [AddInfoLog]
    public async Task<ResultModel<EkleKullaniciCommandResponseModel>> EkleKullanici([FromBody] EkleKullaniciCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("GuncelleKullanici")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.KullanicilarYoneticisi}")]
    [AddInfoLog]
    public async Task<ResultModel<GuncelleKullaniciCommandResponseModel>> GuncelleKullanici([FromBody] GuncelleKullaniciCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("sifremiUnuttum")]
    [AllowAnonymous]
    [AddInfoLog]
    public async Task<ResultModel<SifremiUnuttumKullaniciCommandResponseModel>> SifremiUnuttum([FromBody] SifremiUnuttumKullaniciCommand request)
    {
        return await _mediator.Send(request);
    }
    [HttpPost("sifreDegistir")]
    [AddInfoLog]
    public async Task<ResultModel<SifreDegistirKullaniciCommandResponseModel>> SifreDegistir([FromBody] SifreDegistirKullaniciCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("SilKullanici")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.KullanicilarYoneticisi}")]
    [AddInfoLog]
    public async Task<ResultModel<SilKullaniciCommandResponseModel>> SilKullanici([FromBody] SilKullaniciCommand request)
    {
        return await _mediator.Send(request);
    }

}