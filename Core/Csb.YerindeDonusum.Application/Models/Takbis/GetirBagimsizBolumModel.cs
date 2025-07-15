using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.Models.Takbis;

public class GetirBagimsizBolumModel
{
    public decimal[]? MahalleIds { get; set; }
    public string? AdaNo { get; set; }
    public string? ParselNo { get; set; }
    public TapuBolumDurumEnum? TapuBolumDurum { get; set; } = TapuBolumDurumEnum.Hepsi;
    public string? Bbno { get; set; }
    public string? Kat { get; set; }
    public string? Blok { get; set; }
    public string? Giris { get; set; }
}
