using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BirimCQRS;
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
public class BirimController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BirimController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
  
    [HttpGet("GetirBirimListe")]
    public async Task<ResultModel<List<BirimDto>>> GetirBirimListe()
    {
        var query = new GetirBirimListeQuery();
        return await _mediator.Send(query);
    }
}