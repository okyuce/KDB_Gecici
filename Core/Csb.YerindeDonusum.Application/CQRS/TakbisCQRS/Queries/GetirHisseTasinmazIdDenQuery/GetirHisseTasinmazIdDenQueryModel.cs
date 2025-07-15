using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazByTakbisTasinmazId;

public class GetirHisseTasinmazIdDenQueryModel
{
    public int TakbisTasinmazId { get; set; }
    public string TapuBolumDurum { get; set; }
}
