using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirParselMahalleIdAdaIdDenQuery;

public class GetirListeParselByTakbisMahalleIdAdaIdDenQueryModel
{
    public int MahalleId { get; set; }
    public string AdaNo { get; set; }
}
