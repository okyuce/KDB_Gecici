
namespace Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Commands.TebligatGonder;

public class TebligatGonderCommandModel
{
    public string? GonderimId { get; set; }
    public long? TcKimlikNoRaw { get; set; }
    public long? TcKimlikNo { get; set; }
    public string? TapuTasinmazId { get; set; }
    public string? AskiKodu { get; set; }
    public string? HasarTespitHasarDurumu { get; set; }
    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }
    public string? TapuIlAdi { get; set; }
    public string? TapuIlceAdi { get; set; }
    public string? TapuMahalleAdi { get; set; }
    public DateTime TebligTarihi { get; set; }
    public int TuzelKisiTipId { get; set; }
    public string? TuzelKisiVergiNo { get; set; }
}
