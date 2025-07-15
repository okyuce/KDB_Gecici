namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeOdemeBekleyenDegerlendirmeServerSide;

public class GetirListeOdemeBekleyenDegerlendirmeServerSideResponseModel
{
    public long BinaDegerlendirmeId { get; set; }
    public string? HasarTespitIlAdi { get; set; }
    public string? UavtIlAdi { get; set; }
    public string? UavtIlceAdi { get; set; }
    public string? UavtMahalleAdi { get; set; }
    public string? HasarTespitIlceAdi { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public string? HasarTespitMahalleAdi { get; set; }
    public string? YapiKimlikNo { get; set; }
    public string? Seviye { get; set; }
    public string? Ada { get; set; }
    public string? Parsel { get; set; }
    public string? BinaDisKapiNo { get; set; }
    public long? BinaOdemeDurumId { get; set; }
    public long? BultenNo { get; set; }
    public string? BinaOdemeDurumAd { get; set; }
    public decimal? OdemeTutari { get; set; }
    public decimal? HibeOdemeTutari { get; set; }
    public decimal? KrediOdemeTutari { get; set; }
    public decimal? DigerHibeOdemeTutari { get; set; }
    public DateTime OlusturmaTarihi { get; set; }
    public DateTime? GuncellemeTarihi { get; set; }
    public bool OdemeListesiButonuAktifMi { get; set; }
}