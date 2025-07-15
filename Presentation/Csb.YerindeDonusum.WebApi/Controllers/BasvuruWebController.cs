using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.GuncelleBasvuruAfadDurum;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirAfadBasvuruListeByTcNo;
using Csb.YerindeDonusum.Application.CQRS.BasvuruAfadDurumCQRS;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruDetayById;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruGostergePaneliVeri;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruListeServerSide;
using Csb.YerindeDonusum.Application.CQRS.BasvuruDurumCQRS;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Dtos.Afad;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.BasvuruSonuclandir;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.EkleBasvuru;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.GuncelleBasvuru;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.BasvuruOnaylaReddet;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeMalikler;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.AktarKamuUstlenecek;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.AdaParselGuncelle;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.IptalBasvuruByIdFromWeb;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListePasifMalikler;
using CSB.Core.LogHandler.Attr;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BasvuruWebController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BasvuruWebController(IMediator mediator, IMapper mapper)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost("EkleGercekKisiBasvuru")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    [AddInfoLog]
    public async Task<ResultModel<EkleBasvuruCommandResponseModel>> EkleGercekKisiBasvuru([FromBody] EkleBasvuruCommandModel model)
    {
        model.TuzelKisiTipId = null;
        var appeal = new EkleBasvuruCommand() { Model = model };
        return await _mediator.Send(appeal);
    }

    [HttpPost("AktarKamuUstlenecek")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    [AddInfoLog]
    public async Task<ResultModel<string>> AktarKamuUstlenecek([FromBody] AktarKamuUstlenecekCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("BasvuruOnaylaReddet")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    [AddInfoLog]
    public async Task<ResultModel<string>> BasvuruOnaylaReddet([FromBody] BasvuruOnaylaReddetCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("EkleTuzelKisiBasvuru")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    [AddInfoLog]
    public async Task<ResultModel<EkleBasvuruCommandResponseModel>> EkleTuzelKisiBasvuru([FromBody] EkleBasvuruCommandModel model)
    {
        model.TuzelKisiTipId ??= 1;
        var appeal = new EkleBasvuruCommand() { Model = model };
        return await _mediator.Send(appeal);
    }

    [HttpPost("GuncelleBasvuruAfadDurum")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    [AddInfoLog]
    public async Task<ResultModel<string>> GuncelleBasvuruAfadDurum([FromBody] GuncelleBasvuruAfadDurumCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("GuncelleBasvuru")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruDuzenle}")]
    [AddInfoLog]
    public async Task<ResultModel<GuncelleBasvuruCommandResponseModel>> GuncelleBasvuru(GuncelleBasvuruCommandModel model)
    {
        var appeal = new GuncelleBasvuruCommand() { Model = model };
        return await _mediator.Send(appeal);
    }

    [HttpPost("IptalBasvuru")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruIptal}")]
    [AddInfoLog]
    public async Task<ResultModel<string>> IptalBasvuru([FromBody] IptalBasvuruByIdFromWebCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("BasvuruSonuclandir")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruDuzenle}, {RoleEnum.BasvuruIptal}")]
    [AddInfoLog]
    public async Task<ResultModel<string>> BasvuruSonuclandir([FromBody] BasvuruSonuclandirCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("GetirBasvuruListeServerSide")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<DataTableResponseModel<List<GetirBasvuruListeServerSideResponseModel>>>> GetirListGetirBasvuruListeServerSideeServerSide([FromBody] GetirBasvuruListeServerSideQuery request)
    {
        request.MaskelemeKapaliMi = false;
        return await _mediator.Send(request);
    }

    [HttpGet("GetirBasvuruDetay")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}, {RoleEnum.BasvuruDuzenle}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirBasvuruDetayByIdQueryResponseModel>> GetirBasvuruDetay([FromQuery] GetirBasvuruDetayByIdQueryModel model)
    {
        var query = new GetirBasvuruDetayByIdQuery() { Model = model };
        return await _mediator.Send(query);
    }

    [HttpGet("GetirAfadBasvuruListeByTcNo")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruEkle}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<List<AfadBasvuruDto>>> GetirAfadBasvuruListeByTcNo([FromQuery] GetirAfadBasvuruListeByTcNoQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirBasvuruGostergePaneliVeri")]
    public async Task<ResultModel<GetirBasvuruGostergePaneliVeriQueryResponseModel>> GetirBasvuruGostergePaneliVeri([FromQuery] GetirBasvuruGostergePaneliVeriQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirBasvuruDurumListe")]
    public async Task<ResultModel<List<BasvuruDurumDto>>> GetirBasvuruDurumListe([FromQuery] GetirBasvuruDurumListeQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirBasvuruAfadDurumListe")]
    public async Task<ResultModel<List<BasvuruDurumDto>>> GetirBasvuruAfadDurumListe([FromQuery] GetirBasvuruAfadDurumListeQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet("GetirListeMalikler")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog]
    public async Task<ResultModel<List<GetirListeMaliklerQueryResponseModel>>> GetirListeMalikler([FromQuery] GetirListeMaliklerQuery request)
    {
        return await _mediator.Send(request);
    }
    
    [HttpGet("GetirListePasifMalikler")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog]
    public async Task<ResultModel<List<GetirListePasifMaliklerQueryResponseModel>>> GetirListePasifMalikler([FromQuery] GetirListePasifMaliklerQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("BasvuruAdaParselGuncelle")]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruDuzenle}")]
    [AddInfoLog]
    public async Task<ResultModel<string>> AdaParselGuncelle(BasvuruAdaParselGuncelleCommandModel model)
    {
        var appeal = new BasvuruAdaParselGuncelleCommand() { Model = model };
        return await _mediator.Send(appeal);
    }
}