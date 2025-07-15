using Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciGiris;
using Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciGirisDogrula;
using Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciGirisServis;
using Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciTokenYenile;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models;
using CSB.Core.LogHandler.Attr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class HesapController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _contextAccessor;

    public HesapController(IMediator mediator, IHttpContextAccessor contextAccessor)
    {
        _mediator = mediator;
        _contextAccessor = contextAccessor;
    }
    
    [HttpPost("Giris")]
    [AddInfoLog]
    public async Task<ResultModel<KullaniciGirisKodDto>> Giris([FromBody] KullaniciGirisCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("GirisServis")]
    [AddInfoLog]
    public async Task<ResultModel<TokenDto>> GirisServis([FromBody] KullaniciGirisServisCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("GirisDogrula")]
    [AddInfoLog]
    public async Task<ResultModel<TokenDto>> GirisDogrula([FromBody] KullaniciGirisDogrulaCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("Yenile")]
    [AddInfoLog]
    public async Task<ResultModel<TokenDto>> Yenile([FromBody] KullaniciTokenYenileCommand request)
    {
        return await _mediator.Send(request);
    }
}