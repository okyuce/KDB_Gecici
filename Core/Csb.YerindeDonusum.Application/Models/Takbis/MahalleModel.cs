using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.Application.Models.Takbis;

public class MahalleModel
{
    public decimal Id { get; set; }
    public string Ad { get; set; }
    public DurumEnum Durum { get; set; }
    public string Tip { get; set; }
    public int KurumId  { get; set; }
}
