using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.DestekTurCQRS.Queries;
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
public class BasvuruDestekTurController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BasvuruDestekTurController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
  
    [HttpGet("GetirBasvuruDestekTurListe")]
    public async Task<ResultModel<List<BasvuruDestekTurDto>>> GetirBasvuruDestekTurListe()
    {
        var result = await _mediator.Send(new GetirBasvuruDestekTurListeQuery());
        return _mapper.Map<ResultModel<List<BasvuruDestekTurDto>>>(result);
    }
}