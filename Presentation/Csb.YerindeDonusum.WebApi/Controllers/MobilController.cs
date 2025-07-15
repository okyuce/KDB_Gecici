using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuranKisiSayisi;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruSayisi;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MobilController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public MobilController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;

        _mediator = mediator;
    }

    [HttpGet("GetirBasvuruSayisi")]
    public async Task<ResultModel<GetirBasvuruSayisiQueryResponseModel>> GetirBasvuruSayisi()
    {
        return await _mediator.Send(new GetirBasvuruSayisiQuery());
    }

    [HttpGet("GetirBasvuranKisiSayisi")]
    public async Task<ResultModel<GetirBasvuranKisiSayisiQueryResponseModel>> GetirBasvuranKisiSayisi()
    {
        return await _mediator.Send(new GetirBasvuranKisiSayisiQuery());
    }
}