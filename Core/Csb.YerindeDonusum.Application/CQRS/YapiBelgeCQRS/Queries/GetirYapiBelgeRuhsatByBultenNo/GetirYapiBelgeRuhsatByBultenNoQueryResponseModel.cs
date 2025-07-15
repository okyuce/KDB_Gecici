using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.YapiBelgeCQRS.Queries.GetirYapiBelgeByYapiKimlikNo;

public class GetirYapiBelgeRuhsatByBultenNoQueryResponseModel
{
    public long? YapiKimlikNo { get; set; }
    public decimal? ToplamYapiInsaatAlan { get; set; }
    public int? ToplamBBSayi { get; set; }
    public int? ToplamKatSayi { get; set; }
    public int? YolKotUstKatSayi { get; set; }
    public int? YolKotAltKatSayi { get; set; }
    public string? AcikAdres { get; set; }
    public string? AdaNo { get; set; }
    public string? ParselNo { get; set; }
    public DateTime? RuhsatOnayTarihi { get; set; }
    public string? IlAdi { get; set; }
    public long? IlKimlikNo { get; set; }
    public string? IlceAdi { get; set; }
    public long? IlceKimlikNo { get; set; }
    public string? CsbmAdi { get; set; }
    public long? CsbmKimlikNo { get; set; }
    public string? MahalleAdi { get; set; }
    public long? MahalleKimlikNo { get; set; }
    public string? KoyAdi { get; set; }
    public long? KoyKimlikNo { get; set; }
    public long? NumaratajKimlikNo { get; set; }
    public string? DisKapiNo { get; set; }
}