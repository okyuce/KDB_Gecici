using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.EkleBasvuru;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.IptalBasvuruById;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruDetayById;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruListeByTcNo;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.WebApi.BasicAuthFiles;
using CSB.Core.LogHandler.Attr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme)]
public class BasvuruController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BasvuruController(IMediator mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet("GetirGercekKisiBasvuruListe")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirBasvuruListeByTcNoResponseModel>> GetirGercekKisiBasvuruListe([FromQuery] string TcKimlikNo)
    {
        var query = new GetirBasvuruListeByTcNoQuery() { Model = new GetirBasvuruListeByTcNoQueryModel { TcKimlikNo = TcKimlikNo, TuzelMi = false } };
        return await _mediator.Send(query);
    }

    [HttpGet("GetirTuzelKisiBasvuruListe")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirBasvuruListeByTcNoResponseModel>> GetirTuzelKisiBasvuruListe([FromQuery] string TcKimlikNo, [FromQuery] string TuzelKisiMersisNo)
    {
        var query = new GetirBasvuruListeByTcNoQuery() {
            Model = new GetirBasvuruListeByTcNoQueryModel {
                TcKimlikNo = TcKimlikNo,
                TuzelKisiMersisNo = TuzelKisiMersisNo,
                TuzelMi = true
            }
        };
        return await _mediator.Send(query);
    }

    [HttpGet("GetirBasvuruDetay")]
    [AddInfoLog]
    public async Task<ResultModel<GetirBasvuruDetayByIdQueryResponseModel>> GetirBasvuruDetay([FromQuery] GetirBasvuruDetayByIdQueryModel model)
    {
        var query = new GetirBasvuruDetayByIdQuery() { Model = model };
        return await _mediator.Send(query);
    }

    [HttpPost("EkleGercekKisiBasvuru")]
    [AddInfoLog]
    public async Task<ResultModel<EkleBasvuruCommandResponseModel>> EkleGercekKisiBasvuru(EkleBasvuruCommandModel model)
    {
        model.TuzelKisiTipId = null;
        var appeal = new EkleBasvuruCommand() { Model = model };
        return await _mediator.Send(appeal);
    }

    [HttpPost("EkleTuzelKisiBasvuru")]
    [AddInfoLog]
    public async Task<ResultModel<EkleBasvuruCommandResponseModel>> EkleTuzelKisiBasvuru(EkleBasvuruCommandModel model)
    {
        model.TuzelKisiTipId ??= 1;
        var appeal = new EkleBasvuruCommand() { Model = model };
        return await _mediator.Send(appeal);
    }

    [HttpPost("IptalBasvuru")]
    [AddInfoLog]
    public async Task<ResultModel<string>> IptalBasvuru(IptalBasvuruByIdCommand request)
    {
        return await _mediator.Send(request);
    }
}