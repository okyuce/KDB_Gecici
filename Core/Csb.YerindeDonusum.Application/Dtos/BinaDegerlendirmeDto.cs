using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeDetay;
using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.Application.Dtos;

public class BinaDegerlendirmeDto
{
    public long BinaDegerlendirmeId { get; set; }
    public string? UavtIlAdi { get; set; }
    public string? UavtIlceAdi { get; set; }
    public string? UavtMahalleAdi { get; set; }
    public string? BinaDisKapiNo { get; set; }
    public string? Ada { get; set; }
    public string? Parsel { get; set; }
    public long? YapiKimlikNo { get; set; }
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? HasarTespitIlAdi { get; set; }
    public string? HasarTespitIlceAdi { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public double? Oran { get; set; } = 0;
    public string? SonHasarDurumu { get; set; }
    public string? SutunRenk { get; set; }
    public string? HasarDurumu { get; set; }
    public string? ItirazSonucu { get; set; }
    public string? BasvuruDegerlendirmeDurumAd { get; set; }
    public long? BinaDegerlendirmeDurumId { get; set; }
    public string? OdemeDurumAd { get; set; }
    public int? ToplamKatSayisi { get; set; }
    public bool MuteahhitAtaGosterilsinMi { get; set; }
    public bool OranDegistirGosterilsinMi { get; set; }
    public bool OnayaGonderGosterilsinMi { get; set; }
    public bool OnaylaReddetGosterilsinMi { get; set; }
    public long? BasvuruMuteahhitDurumuEnum { get; set; }
    public bool DegerlendirmeButonuAktifMi { get; set; }
    public long? BultenNo { get; set; }
    public long? YibfNo { get; set; }
    public int? ImzalayanKisiSayisi { get; set; }
    public decimal? YapiInsaatAlan { get; set; }
    public int? BagimsizBolumSayisi { get; set; }
    public int? KotAltKatSayisi { get; set; }
    public int? KotUstKatSayisi { get; set; }
    public DateTime? RuhsatOnayTarihi { get; set; }
    public string? FenniMesulTc { get; set; }
    public DateTime? IzinBelgesiTarih { get; set; }
    public long? IzinBelgesiSayi { get; set; }
    public long? AdaParselGuncellemeTipId { get; set; }
    public string? EskiTapuAda { get; set; }
    public string? EskiTapuParsel { get; set; }
    public long? AdaParselGuncellemeTipiId { get; set; }
    public string? AdaParselGuncellemeTipiAdi { get; set; }
    public Guid? AdaParselGuncelleDosyaGuid { get; set; }
    public int TapuIlNo { get; set; }
    public int TapuIlceNo { get; set; }
    public string? TapuMahalleNo { get; set; }
    public string? TapuIlAdi { get; set; }
    public string? TapuIlceAdi { get; set; }
    public string? TapuMahalleAdi { get; set; }
    public List<string> VergiKimlikNo { get; set; }
    public List<string> IbanNo { get; set; }
    public List<string> YetkiBelgeNo { get; set; }
    public long? OlusturanKullaniciId { get; set; }
    public long? GuncelleyenKullaniciId { get; set; }
    public GetirBinaDegerlendirmeDetayMuteahhitModel? Muteahhit { get; set; }
    public GetirBinaDegerlendirmeDetayBinaDegerlendirmeDosyaModel? BinaDegerlendirmeDosya { get; set; }
    public GetirBinaDegerlendirmeDetayBinaYapiRuhsatIzinDosyaModel? BinaYapiRuhsatIzinDosya { get; set; }
    public GetirBinaDegerlendirmeDetayBinaYapiDenetimSeviyeTespitModel? BinaYapiDenetimSeviyeTespit { get; set; }
}