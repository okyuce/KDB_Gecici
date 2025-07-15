namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeDetay;

public class GetirBinaDegerlendirmeDetayMuteahhitModel
{
    public string AdSoyadUnvan { get; set; } = null!;
    public string? Adres { get; set; }
    public string? CepTelefonu { get; set; }
    public string? Telefon { get; set; }
    public string? Eposta { get; set; }
    public string VergiKimlikNo { get; set; } = null!;
    public string? YetkiBelgeNo { get; set; }
    public string? Aciklama { get; set; }
    public string? IbanNo { get; set; }
    public long BinaMuteahhitTapuTurId { get; set; }
}