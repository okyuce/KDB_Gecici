namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeDetay;

public class GetirBinaDegerlendirmeDetayQueryResponseModel
{
    public long BinaDegerlendirmeId { get; set; }
    public string? Ada { get; set; }
    public string? Parsel { get; set; }
    public long? BultenNo { get; set; }
    public long? YibfNo { get; set; }
    public int? ImzalayanKisiSayisi { get; set; }
    public long? YapiKimlikNo { get; set; }
    public decimal? YapiInsaatAlan { get; set; }
    public int? BagimsizBolumSayisi { get; set; }
    public int? ToplamKatSayisi { get; set; }
    public int? KotAltKatSayisi { get; set; }
    public int? KotUstKatSayisi { get; set; }
    public DateTime? RuhsatOnayTarihi { get; set; }
    public string? FenniMesulTc { get; set; }
    public bool? FenniMesulSeciliMi => FenniMesulTc != null && YibfNo == null;
    public DateTime? IzinBelgesiTarih { get; set; }
    public long? IzinBelgesiSayi { get; set; }
    public bool? IzinBelgesiSeciliMi => (BultenNo == null || BultenNo == 0) && IzinBelgesiTarih != null && IzinBelgesiSayi != null && IzinBelgesiSayi != 0;
    public GetirBinaDegerlendirmeDetayMuteahhitModel? Muteahhit { get; set; }
    public GetirBinaDegerlendirmeDetayBinaDegerlendirmeDosyaModel? BinaDegerlendirmeDosya { get; set; }
    public GetirBinaDegerlendirmeDetayBinaYapiRuhsatIzinDosyaModel? BinaYapiRuhsatIzinDosya { get; set; }
}