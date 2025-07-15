using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.DosyaTurCQRS;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.WebApi.BasicAuthFiles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme)]
public class BasvuruDosyaTurController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BasvuruDosyaTurController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
  
    [HttpGet("GetirBasvuruDosyaTurListe")]
    public async Task<ResultModel<List<BasvuruDosyaTurDto>>> GetirBasvuruDosyaTurListe()
    {
        var query = new GetirBasvuruDosyaTurListeQuery();
        return await _mediator.Send(query);
    }
}