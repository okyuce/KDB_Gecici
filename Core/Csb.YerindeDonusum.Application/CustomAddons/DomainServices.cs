using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeMalikler;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using CSB.Core.Extensions;
using MediatR;
using System.Net.Mail;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace Csb.YerindeDonusum.Application.CustomAddons;

public static class DomainServices
{

    public static async Task<ResultModel<string>> ButunMaliklerImzaVerdiMi(IMediator _mediator, BinaDegerlendirme binaDegerlendirme)
    {
        ResultModel<string> result = new();
        var malikListeResult = await _mediator.Send(new GetirListeMaliklerQuery()
        {
            BinaDegerlendirmeId = binaDegerlendirme.BinaDegerlendirmeId,
            UavtMahalleNo = binaDegerlendirme.UavtMahalleNo,
            HasarTespitAskiKodu = binaDegerlendirme.HasarTespitAskiKodu,
            BagimsizBolumlerAlinmasin = true,
            IptalEdilenlerAlinmasin = true,
        });

        if (malikListeResult.Result?.Any() != true)
        {
            result.ErrorMessage("Aktif başvuru bulunamadı.");
            return await Task.FromResult(result);
        }
        if (malikListeResult.Result.Any(x => x.BasvuruAfadDurumId == (long)BasvuruAfadDurumEnum.Kabul) == true)
        {
            result.ErrorMessage("AFAD durumu Kabul olan başvurular bulunduğundan bu işleme devam edilemez. Lütfen önce bu başvuruları iptal ediniz.");
            return await Task.FromResult(result);
        }
        if (!malikListeResult.Result.Any(x => x.BagimsizBolumNo!=null))
        {    
            result.IsError = true;
            result.ErrorMessage("En Az Bir Malik imza vermeden bu işleme devam edilemez.");          
        }

        return result;
    }
    
}