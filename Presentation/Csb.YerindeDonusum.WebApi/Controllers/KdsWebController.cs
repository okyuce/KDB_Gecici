using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries;
using Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.KdsHasarTespitVeriByAskiKodu;
using Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.KdsHasarTespitVeriByUid;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models;
using CSB.Core.LogHandler.Attr;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class KdsWebController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public KdsWebController(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
  
    [HttpGet("GetirHasarTespitByAskiKodu")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    [AddInfoLog]
    public async Task<ResultModel<KdsHaneModel>> GetirHasarTespitByAskiKodu([FromQuery] KdsHasarTespitVeriByAskiKoduQuery request)
    {
        return await _mediator.Send(request);
    }
  
    [HttpGet("GetirListeDepremIl")]
    public async Task<ResultModel<List<GetirListeIlKdsHasarTespitVeriQueryResponseModel>>> GetirListeDepremIl()
    {
        return await _mediator.Send(new GetirListeIlKdsHasarTespitVeriQuery());
    }
}