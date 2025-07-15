using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeDetay;
using Csb.YerindeDonusum.Application.Dtos;

namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiServerSideGroupped;

public class GetirListeYeniYapiServerSideGrouppedResponseModel
{
    public long BinaDegerlendirmeId { get; set; }
    public string? UavtIlAdi { get; set; }
    public string? UavtIlceAdi { get; set; }
    public string? UavtMahalleAdi { get; set; }
    public string? BinaDisKapiNo { get; set; }
    public long? YapiKimlikNo { get; set; }
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public string? Ada { get; set; }
    public string? Parsel { get; set; }
    public string? BasvuruDegerlendirmeDurumAd { get; set; }
    public long? BinaDegerlendirmeDurumId { get; set; }
    public int? ToplamKatSayisi { get; set; }
    public long? BasvuruMuteahhitDurumuEnum { get; set; }
    public List<BinaDegerlendirmeDto>? YeniYapiList { get; set; }

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
    public bool AdaParselGuncellemeButonuAktifMi { get; set; }
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
    public string? OlusturanKullaniciAdi { get; set; }
    public string? GuncelleyenKullaniciAdi { get; set; }

    public GetirBinaDegerlendirmeDetayMuteahhitModel? Muteahhit { get; set; }
    public GetirBinaDegerlendirmeDetayBinaDegerlendirmeDosyaModel? BinaDegerlendirmeDosya { get; set; }
    public GetirBinaDegerlendirmeDetayBinaYapiRuhsatIzinDosyaModel? BinaYapiRuhsatIzinDosya { get; set; }
    public GetirBinaDegerlendirmeDetayBinaYapiDenetimSeviyeTespitModel? BinaYapiDenetimSeviyeTespit { get; set; }

}