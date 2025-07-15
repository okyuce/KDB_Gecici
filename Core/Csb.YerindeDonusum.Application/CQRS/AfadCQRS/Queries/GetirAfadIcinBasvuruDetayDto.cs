namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries;

public class GetirAfadIcinBasvuruDetayDto
{
    public Guid BasvuruGuid { get; set; }
    public DateTime OlusturmaTarihi { get; set; }
    public DateTime? GuncellemeTarihi { get; set; }
    public string TcKimlikNo { get; set; } = null!;
    public string? Ad { get; set; }
    public string? Soyad { get; set; }
    public string BasvuruKodu { get; set; }
    public long BasvuruKanalId { get; set; }
    public string BasvuruKanali { get; set; }
    public string HasarTespitUid { get; set; } = null!;
    public string? HasarTespitAskiKodu { get; set; }
    public long BasvuruDurumId { get; set; }
    public string BasvuruDurumu { get; set; }
    public string? IlAdi { get; set; }
    public string? IlceAdi { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? MahalleAdi { get; set; }
    public string? UavtAdresNo { get; set; }
    public string? UavtBinaNo { get; set; }
    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }
    public long? BasvuruTurId { get; set; }
    public string? BasvuruTurAdi { get; set; }
    public long? BinaDegerlendirmeDurumId { get; set; }
    public string? BinaDegerlendirmeDurumAdi { get; set; }
    public bool? BinaDegerlendirmeYapiRuhsatGirildiMi { get; set; }
    public string? TuzelKisiVergiNo { get; set; }
    public string? TuzelKisiAdi { get; set; }
    public string? TuzelKisiMersisNo { get; set; }
    public int? TuzelKisiTipId { get; set; }
    public double? UzlasmaOrani { get; set; }
    public bool RezervAlanMi { get; set; }
    public int? TapuTasinmazId { get; set; }
    public bool MalikUstlenecekMi { get; set; }
}