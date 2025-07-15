namespace Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirTebligatGonderimDetayById;

public class GetirTebligatGonderimDetayByIdQueryResponseModel
{
    public string? TapuIlAdi { get; set; }
    public string? TapuIlceAdi { get; set; }
    public string? TapuMahalleAdi { get; set; }
    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }
    public int? TapuTasinmazId { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public string? HasarTespitHasarDurumu { get; set; }
    public DateTime? TebligTarihi { get; set; }
}
