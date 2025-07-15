using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.Application.Models.Takbis;

public class AlanModel
{
    public decimal Id { get; set; }
    public TapuBolumDurumEnum TapuBolumDurum { get; set; }
    public decimal YuzOlcum { get; set; }
    public IslemModel TesisIslem { get; set; }
    public IslemModel TerkinIslem { get; set; }
}
