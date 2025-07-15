namespace Csb.YerindeDonusum.Application.Models.Takbis;

public class DaimiMustakilHakModel
{
    public decimal Id { get; set; }
    public int TesisBicim { get; set; }
    public DateTime BaslamaTarihi { get; set; }
    public DateTime BitisTarihi { get; set; }
    public string SureAciklama { get; set; }
    public string Cumle { get; set; }
}