using Csb.YerindeDonusum.Application.CQRS.BilgilendirmeMesajCQRS.Queries.GetirBilgilendirmeMesajByAnahtar;
using Csb.YerindeDonusum.Application.CQRS.BilgilendirmeMesajCQRS.Queries.GetirBilgilendirmeMesajById;
using Csb.YerindeDonusum.Application.CQRS.BilgilendirmeMesajCQRS.Queries.GetirListeBilgilendirmeMesaj;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.WebApi.BasicAuthFiles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csb.YerindeDonusum.WebApi.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
[Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme)]
public class BilgilendirmeMesajController : ControllerBase
{
    private readonly IMediator _mediator;

    public BilgilendirmeMesajController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Id bilgisine gore ilgili bilgilendirme mesaj bilgisini dondurmektedir.
    /// </summary>
    /// <param name="id">Bilgilendirme Mesaj Id bilgisi</param>
    /// <returns></returns>
    [HttpGet(nameof(GetirBilgilendirmeMesajById))]
    public async Task<ResultModel<GetirBilgilendirmeMesajByIdQueryResponseModel>> GetirBilgilendirmeMesajById(int id)
    {
        return await _mediator.Send(new GetirBilgilendirmeMesajByIdQuery { Id = id });
    }

    /// <summary>
    /// Anahtar bilgisine gore ilgili bilgilendirme mesaj bilgisini dondurmektedir.
    /// </summary>
    /// <param name="anahtar">Bilgilendirme mesaj anahtar bilgisi</param>
    /// <returns></returns>
    [HttpGet(nameof(GetirBilgilendirmeMesajByAnahtar))]
    public async Task<ResultModel<GetirBilgilendirmeMesajByIdQueryResponseModel>> GetirBilgilendirmeMesajByAnahtar(string anahtar)
    {
        return await _mediator.Send(new GetirBilgilendirmeMesajByAnahtarQuery { Anahtar = anahtar });
    }

    /// <summary>
    /// Anahtar bilgisine gore ilgili bilgilendirme mesaj bilgisini dondurmektedir.
    /// </summary>
    /// <param name="anahtar">Bilgilendirme mesaj anahtar bilgisi</param>
    /// <returns></returns>
    [HttpGet(nameof(GetirListeBilgilendirmeMesaj))]
    public async Task<ResultModel<List<GetirBilgilendirmeMesajByIdQueryResponseModel>>> GetirListeBilgilendirmeMesaj()
    {
        return await _mediator.Send(new GetirListeBilgilendirmeMesajQuery { });
    }
}
