using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetBelge;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetBinaDigerYardimlar;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetMuteahhitBilgileri;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetNakdiYardim;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetYapiDenetim;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetYapiRuhsat;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeDetay;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumu;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaIcinYapilanDigerYardimlar;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeBinaDegerlendirmeServerSide;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeOdemeBekleyenDegerlendirmeServerSide;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirNakdiYardimTaksitler;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirYapiDenetimSeviyeTespitBilgiler;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.IndirBinaDegerlendirmeDosya;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.IndirBinaNakdiYardimTaksitDosya;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.IndirBinaYapiRuhsatDosya;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.IndirYapiDenetimDosya;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeDosyaCQRS.Commands.YapiDenetimBelgeGuncelle;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeBinaOdemeServerSide;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeOdemeTalepleriServerSide;
using Csb.YerindeDonusum.Application.CQRS.MuteahhitCQRS.Queries.GetirYetkiBelgeNoIle;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using CSB.Core.LogHandler.Attr;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BinaDegerlendirmeController : ControllerBase
{

    #region ...: Constructor Injection :...

    private readonly IMediator _mediator;

    public BinaDegerlendirmeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #endregion ...: Constructor Injection :...

    #region ...: Commands :...

    [HttpPost(nameof(KaydetBelge))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog]
    public async Task<ResultModel<KaydetBelgeCommandResponseModel>> KaydetBelge([FromBody] KaydetBelgeCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(KaydetYapiRuhsat))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog]
    public async Task<ResultModel<KaydetYapiRuhsatCommandResponseModel>> KaydetYapiRuhsat([FromBody] KaydetYapiRuhsatCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(KaydetMuteahhitBilgileri))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog]
    public async Task<ResultModel<string>> KaydetMuteahhitBilgileri([FromBody] KaydetMuteahhitBilgileriCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(KaydetBinaDigerYardimlar))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog]
    public async Task<ResultModel<string>> KaydetBinaDigerYardimlar([FromBody] KaydetBinaDigerYardimlarCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(KaydetYapiDenetim))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog]
    public async Task<ResultModel<string>> KaydetYapiDenetim([FromBody] KaydetYapiDenetimCommand request)
    {
        return await _mediator.Send(request);
    }
    [HttpPost(nameof(YapiDenetimBelgeGuncelle))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog]
    public async Task<ResultModel<YapiDenetimBelgeGuncelleCommandResponseModel>> YapiDenetimBelgeGuncelle([FromBody] YapiDenetimBelgeGuncelleCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost(nameof(KaydetNakdiYardim))]
    [AddInfoLog]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    public async Task<ResultModel<string>> KaydetNakdiYardim([FromBody] KaydetNakdiYardimCommand request)
    {
        return await _mediator.Send(request);
    }
    #endregion ...: Commands :...

    #region ...: Queries :...

    [HttpGet(nameof(GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumu))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQueryResponseModel>> GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumu([FromQuery] GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQuery model)
    {
        return await _mediator.Send(model);
    }
    
    [HttpGet(nameof(GetirDetay))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<GetirBinaDegerlendirmeDetayQueryResponseModel>> GetirDetay([FromQuery] GetirBinaDegerlendirmeDetayQuery model)
    {
        return await _mediator.Send(model);
    }

    [HttpGet(nameof(GetirBinaIcinYapilanDigerYardimlar))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<List<GetirBinaIcinYapilanDigerYardimlarQueryResponseModel>>> GetirBinaIcinYapilanDigerYardimlar([FromQuery] GetirBinaIcinYapilanDigerYardimlarQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet(nameof(GetirYapiDenetimSeviyeTespitBilgiler))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<List<GetirYapiDenetimSeviyeTespitBilgilerQueryResponseModel>>> GetirYapiDenetimSeviyeTespitBilgiler([FromQuery] GetirYapiDenetimSeviyeTespitBilgilerQuery request)
    {
        return await _mediator.Send(request);
    }

    [HttpGet(nameof(GetirNakdiYardimTaksitler))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<List<GetirNakdiYardimTaksitlerQueryResponseModel>>> GetirNakdiYardimTaksitler([FromQuery] GetirNakdiYardimTaksitlerQuery request)
    {
        return await _mediator.Send(request);
    }

    #endregion ...: Queries :...

    #region ...: List Methods :...

    [HttpPost(nameof(GetirListeOdemeBekleyenDegerlendirmeServerSide))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<DataTableResponseModel<List<GetirListeOdemeBekleyenDegerlendirmeServerSideResponseModel>>>> GetirListeOdemeBekleyenDegerlendirmeServerSide([FromBody] GetirListeOdemeBekleyenDegerlendirmeServerSideQuery request)
    {
        return await _mediator.Send(request);
    }   

    [HttpPost(nameof(GetirListeBinaDegerlendirmeServerSide))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<DataTableResponseModel<List<GetirListeBinaDegerlendirmeServerSideQueryResponseModel>>>> GetirListeBinaDegerlendirmeServerSide([FromBody] GetirListeBinaDegerlendirmeServerSideQuery request)
    {
        return await _mediator.Send(request);
    }

    #endregion ...: List Methods :...

    #region ...: SOAP Service Methods :...

    [HttpGet(nameof(GetirYetkiBelgeNoIle))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog]
    public async Task<ResultModel<GetirYetkiBelgeNoIleQueryResponseModel>> GetirYetkiBelgeNoIle([FromQuery] GetirYetkiBelgeNoIleQuery request)
    {
        return await _mediator.Send(request);
    }

    #endregion ...: SOAP Service Methods

    #region ...: Document Downloand :...
    [HttpGet(nameof(GetirBelgeIndirBilgiler))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<IndirBinaDegerlendirmeDosyaQueryResponseModel>> GetirBelgeIndirBilgiler([FromQuery] IndirBinaDegerlendirmeDosyaQueryModel model)
    {
        IndirBinaDegerlendirmeDosyaQuery query = new IndirBinaDegerlendirmeDosyaQuery() { Model = model };
        return await _mediator.Send(query);
    }

    [HttpGet(nameof(GetirBelgeIndirBilgilerYapiRuhsat))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<IndirBinaYapiRuhsatDosyaQueryResponseModel>> GetirBelgeIndirBilgilerYapiRuhsat([FromQuery] IndirBinaYapiRuhsatDosyaQueryModel model)
    {
        IndirBinaYapiRuhsatDosyaQuery query = new IndirBinaYapiRuhsatDosyaQuery() { Model = model };
        return await _mediator.Send(query);
    }

    [HttpGet(nameof(GetirYapiDenetimBelgeIndirBilgiler))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<IndirYapiDenetimDosyaQueryResponseModel>> GetirYapiDenetimBelgeIndirBilgiler([FromQuery] IndirYapiDenetimDosyaQueryModel model)
    {
        IndirYapiDenetimDosyaQuery query = new IndirYapiDenetimDosyaQuery() { Model = model };
        return await _mediator.Send(query);
    }

    [HttpGet(nameof(GetirBinaNakdiYardimTaksitDosyaIndirBilgiler))]
    [Authorize(Roles = $"{RoleEnum.Admin}, {RoleEnum.BasvuruYoneticisi}, {RoleEnum.BasvuruListele}")]
    [AddInfoLog(PassResponse = true)]
    public async Task<ResultModel<IndirBinaNakdiYardimTaksitDosyaQueryResponseModel>> GetirBinaNakdiYardimTaksitDosyaIndirBilgiler([FromQuery] IndirBinaNakdiYardimTaksitDosyaQueryModel model)
    {
        IndirBinaNakdiYardimTaksitDosyaQuery query = new IndirBinaNakdiYardimTaksitDosyaQuery() { Model = model };
        return await _mediator.Send(query);
    }
    #endregion ...: Belge İndirme Bilgiler :...

}