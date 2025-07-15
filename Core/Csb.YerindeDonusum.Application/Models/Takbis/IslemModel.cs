using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.Application.Models.Takbis;

public class IslemModel
{
    public decimal? Id { get; set; }
    public string? IslemTanimAd { get; set; }
    public string? BaslamaSekilAd { get; set; }
    public string? IptalAciklama { get; set; }
    public IslemDurumEnum? IslemDurum { get; set; }
}
