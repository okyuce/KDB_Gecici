using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BasvuruTurCQRS;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.WebApi.BasicAuthFiles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasvuruTurController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BasvuruTurController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme)]
    [HttpGet("GetirBasvuruTurListe")]
    public async Task<ResultModel<List<BasvuruTurDto>>> GetirBasvuruTurListe()
    {
        var result = await _mediator.Send(new GetirBasvuruTurListeQuery());
        return _mapper.Map<ResultModel<List<BasvuruTurDto>>>(result);
    }
}