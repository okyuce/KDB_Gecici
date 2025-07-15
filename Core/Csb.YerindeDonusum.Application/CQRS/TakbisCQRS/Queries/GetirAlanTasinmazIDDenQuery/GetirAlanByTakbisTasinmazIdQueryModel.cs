using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirAlanByTakbisTasinmazIdQuery;

public class GetirAlanByTakbisTasinmazIdQueryModel
{
    public decimal TakbisTasinmazId { get; set; }

    public TapuBolumDurumEnum TapuBolumDurum { get; set; }
}
