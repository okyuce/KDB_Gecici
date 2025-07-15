using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeMahalleByTakbisIlceIdQuery;

public class GetirMahalleIlceIdDenQueryModel
{
    public decimal IlceId { get; set; }
    public DurumEnum Durum { get; set; }
}
