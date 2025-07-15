namespace Csb.YerindeDonusum.Application.CQRS.MuteahhitCQRS.Queries.GetirYetkiBelgeNoIle;

public class GetirYetkiBelgeNoIleQueryResponseModel
{
    public string AdSoyadUnvan { get; set; }
    public string Aciklama { get; set; }
    public string? Adres { get; set; }
    public string? CepTelefonu { get; set; }
    public string? Eposta { get; set; }
    public string VergiKimlikNo { get; set; }
    public string? YetkiBelgeNo { get; set; }
    public string HasarTespitAskiKodu { get; set; }
    public int UavtMahalleNo { get; set; }
    public string? Telefon { get; set; }
    public string? IbanNo { get; set; }
    public bool YetkiBelgeNoKaydiMi { get; set; }
    public int MuteahhitBasvuruEklemeDurumu { get; set; }
}