namespace Csb.YerindeDonusum.Application.Dtos;

public class BinaOdemeDto
{
    public long? BinaOdemeId { get; set; }
    public long? BinaDegerlendirmeId { get; set; }
    public int? Seviye { get; set; }
    public decimal? OdemeTutari { get; set; }
    public decimal? HibeOdemeTutari { get; set; }
    public decimal? KrediOdemeTutari { get; set; }
    public decimal? DigerHibeOdemeTutari { get; set; }
    public long? BinaOdemeDurumId { get; set; }
    public string? BinaOdemeDurumAd { get; set; }
    public DateTime? OlusturmaTarihi { get; set; }
    public bool? AktifMi { get; set; }
    public bool? SilindiMi { get; set; }
    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }
    public string? TalepNumarasi { get; set; }
    public string? TalepDurumu { get; set; }
    public DateTime? TalepKapatmaTarihi { get; set; }
    public DateTime? GuncellemeTarihi { get; set; }

    // bina degerlendirme bilgileri:
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? UavtIlAdi { get; set; }
    public string? UavtIlceAdi { get; set; }
    public string? UavtMahalleAdi { get; set; } 
    public string? BinaDisKapiNo { get; set; } 
    public bool? OdemeIslemleriButonGoster { get; set; }
    public bool? IlMudurluguneAktarButonGoster { get; set; }
    public string HasarTespitAskiKodu { get; set; }
    public List<string> VergiKimlikNo { get; set; }
    public List<string> IbanNo { get; set; }
    public List<string> YetkiBelgeNo { get; set; }
    public string? OlusturanKullaniciAdi { get; set; }
    public long? OlusturanKullaniciId { get; set; }
    public string? GuncelleyenKullaniciAdi { get; set; }
    public long? GuncelleyenKullaniciId { get; set; }
}