using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.Application.Models.Takbis;

public class BagimsizBolumModel
{
    public decimal Id { get; set; }
    public string Blok { get; set; }
    public string Giris { get; set; }
    public string Kat { get; set; }
    public string No { get; set; }
    public decimal ArsaPay { get; set; }
    public decimal ArsaPayda { get; set; }
    public BagimsizBolumTipEnum Tip { get; set; }
}
