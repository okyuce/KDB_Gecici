namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirDetayYerindeYapimKrediSozlesme;

public class GetirDetayYerindeYapimKrediSozlesmeQueryResponseModel
{
    public decimal? KrediOdemeTutar { get; set; }
    public DateOnly? IlkTaksitTarihi { get; set; }
    public string? UavtIlAdi { get; set; }
    public string? UavtIlceAdi { get; set; }
    public string? UavtMahalleAdi { get; set; }
    public string? TapuPafta { get; set; }
    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }
    public string? BagimsizBolumNo { get; set; }
    public string? TcKimlikNo { get; set; }
    public string? Ad { get; set; }
    public string? Soyad { get; set; }
    public string? CepTelefonu { get; set; }
    public string? Adres { get; set; }
    public string? Eposta { get; set; }
    public string? AskiKodu { get; set; }
}
