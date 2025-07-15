using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.KullaniciHesapTipCQRS;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.WebApi.BasicAuthFiles;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class KullaniciHesapTipController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public KullaniciHesapTipController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
  
    [HttpGet("GetirKullaniciHesapTipListe")]
    public async Task<ResultModel<List<KullaniciHesapTipDto>>> GetirKullaniciHesapTipListe()
    {
        var query = new GetirKullaniciHesapTipListeQuery();
        return await _mediator.Send(query);
    }
}