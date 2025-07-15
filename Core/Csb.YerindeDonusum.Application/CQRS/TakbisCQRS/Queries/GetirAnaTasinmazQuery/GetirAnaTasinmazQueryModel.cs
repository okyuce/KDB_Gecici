using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeAnaTasinmaz;

public class GetirAnaTasinmazQueryModel
{
    public decimal[] MahalleIds { get; set; }
    public string AdaNo { get; set; }
    public string ParselNo { get; set; }
    public TapuBolumDurumEnum TapuBolumDurum { get; set; } = TapuBolumDurumEnum.Hepsi;
}
