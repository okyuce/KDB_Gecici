namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirAfadBasvuruListeByTcNo;

public class GetirAfadBasvuruListeByTcNoResponseModel
{
    public string TcKimlikNo { get; set; } = null!;
    public string BasvuruKodu { get; set; }
    public string BasvuruKanali { get; set; }
    public string HasarTespitUid { get; set; } = null!;
    public string? HasarTespitAskiKodu { get; set; }
    public string BasvuruDurumu { get; set; }
    public string? IlAdi { get; set; }
    public string? IlceAdi { get; set; }
    public string? MahalleAdi { get; set; }
    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }
}