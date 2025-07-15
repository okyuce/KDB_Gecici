namespace Csb.YerindeDonusum.Application.CQRS.YapiBelgeCQRS.Queries.GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNo;

public class GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQueryResponseModel
{
    public long? YapiKimlikNo { get; set; }
    public decimal? ToplamYapiInsaatAlan { get; set; }
    public int? ToplamBBSayi { get; set; }
    public int? ToplamKatSayi { get; set; }
    public int? YolKotUstKatSayi { get; set; }
    public int? YolKotAltKatSayi { get; set; }
    public string? AcikAdres { get; set; }
    public DateTime? RuhsatOnayTarihi { get; set; }
}