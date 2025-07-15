namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruListeServerSide;

public class GetirBasvuruListeServerSideResponseModel
{
    public long? BasvuruId { get; set; }
    public Guid? BasvuruGuid { get; set; }
    public string? BasvuruKodu { get; set; }
    public string? TcKimlikNo { get; set; }
    public string? TcKimlikNoMasked { get; set; }
    public string? ListeAd { get; set; }
    public string? Ad { get; set; }
    public string? Soyad { get; set; }
    public string? TuzelKisiAdi { get; set; }
    public string? TuzelKisiVergiNo { get; set; }
    public string? TuzelKisiMersisNo { get; set; }
    public string? CepTelefonu { get; set; }
    public string? Eposta { get; set; }
    public string? BasvuruKanalAd { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public string? BasvuruTurAd { get; set; }
    public string? BasvuruDestekTurAd { get; set; }
    public string? TapuArsaPay { get; set; }
    public string? TapuArsaPayda { get; set; }
    //public string? TapuArsaPayPaydaText { get; set; }
    public string? UavtIlAdi { get; set; }
    public string? UavtIlceAdi { get; set; }
    public string? UavtMahalleAdi { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? UavtMahalleKodu { get; set; }
    public string? HasarTespitHasarDurumu { get; set; }
    public string? HasarTespitItirazSonucu { get; set; }
    public string? HasarTespitGuclendirmeMahkemeSonucu { get; set; }
    public string? BasvuruDurumAd { get; set; }
    public string? BasvuruDegerlendirmeDurumAd { get; set; }
    public string? BasvuruAfadDurumAd { get; set; }
    public string? Color { get; set; }
    public DateTime? OlusturmaTarihi { get; set; } // { 01/01/2023 00:00:00}
    public bool? IptalEdilebilirMi { get; set; }
    public bool? SonuclandirilabilirMi { get; set; }
    public string? SonuclandirmaAciklamasi { get; set; }
    public string? BasvuruIptalAciklamasi { get; set; }
    public string? TuzelKisiYetkiTuru { get; set; }
    public long OlusturanKullaniciId { get; set; }
    public long? GuncelleyenKullaniciId { get; set; }
    public string? OlusturanKullaniciAdi { get; set; }
    public string? GuncelleyenKullaniciAdi { get; set; }

    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }
    public string? UavtBinaAda { get; set; }
    public string? UavtBinaParsel { get; set; }
    public string? HasarTespitAda { get; set; }
    public string? HasarTespitParsel { get; set; }
    public bool? UavtBeyanMi { get; set; }
}