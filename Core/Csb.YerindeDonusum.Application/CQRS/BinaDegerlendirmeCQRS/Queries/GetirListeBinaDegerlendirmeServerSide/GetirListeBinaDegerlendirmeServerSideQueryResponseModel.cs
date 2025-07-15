namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeBinaDegerlendirmeServerSide;

public class GetirListeBinaDegerlendirmeServerSideQueryResponseModel
{
    public long BinaDegerlendirmeId { get; set; }
    public int Id { get; set; }
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? HasarTespitIlAdi { get; set; }
    public string? HasarTespitIlceAdi { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public double? BasvuruSay { get; set; }
    public double? Oran { get; set; }
    public string? SutunRenk { get; set; }
    public string? HasarDurumu { get; set; }
    public string? ItirazSonucu { get; set; }
    public string? SonHasarDurumu { get; set; }
    public string? BasvuruDegerlendirmeDurumAd { get; set; }
    public long? BinaDegerlendirmeDurumId { get; set; }
    public string? OdemeDurumAd { get; set; }
    public string? HasarTespitMahalleAdi { get; set; }
    public int? KatAdedi { get; set; }
    public string? YapiKimlikNo { get; set; }
    public long? BasvuruMuteahhitDurumuEnum { get; set; }
}