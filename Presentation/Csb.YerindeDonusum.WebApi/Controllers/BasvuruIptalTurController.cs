using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BasvuruIptalTurCQRS.Queries;
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
public class BasvuruIptalTurController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BasvuruIptalTurController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
  
    [HttpGet("GetirBasvuruIptalTurListe")]
    public async Task<ResultModel<List<BasvuruIptalTurDto>>> GetirBasvuruIptalTurListe()
    {
        var result = await _mediator.Send(new GetirBasvuruIptalTurListeQuery());
        return _mapper.Map<ResultModel<List<BasvuruIptalTurDto>>>(result);
    }
}