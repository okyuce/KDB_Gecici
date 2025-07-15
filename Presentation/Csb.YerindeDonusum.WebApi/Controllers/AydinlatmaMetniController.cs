using Csb.YerindeDonusum.Application.CQRS.AydinlatmaMetniCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.WebApi.BasicAuthFiles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
[Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme)]
public class AydinlatmaMetniController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public AydinlatmaMetniController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetirAydinlatmaMetni")]
    public async Task<ResultModel<AydinlatmaMetniDto>> GetirAydinlatmaMetni()
    {
        return await _mediator.Send(new GetirAydinlatmaMetniQuery());
    }
    
    //[HttpGet("GetirAydinlatmaMetinListe")]
    //public async Task<ResultModel<List<AydinlatmaMetniDto>>> GetirAydinlatmaMetinListe()
    //{
    //    return await _mediator.Send(new GetirAydinlatmaMetinListeQuery());
    //}
}