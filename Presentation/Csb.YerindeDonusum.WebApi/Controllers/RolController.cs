using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.RolCQRS;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RolController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RolController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
  
    [HttpGet("GetirRolListe")]
    public async Task<ResultModel<List<RolDto>>> GetirRolListe()
    {
        var query = new GetirRolListeQuery();
        return await _mediator.Send(query);
    }
}