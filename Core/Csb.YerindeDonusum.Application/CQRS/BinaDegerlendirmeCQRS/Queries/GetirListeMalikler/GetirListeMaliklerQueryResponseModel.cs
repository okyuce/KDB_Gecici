namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeMalikler;

public class GetirListeMaliklerQueryResponseModel
{
    public long? BasvuruId { get; set; }
    public long? BinaDegerlendirmeId { get; set; }
    public Guid? BasvuruGuid { get; set; }
    public long? BasvuruKamuUstlenecekId { get; set; }
    public Guid? BasvuruKamuUstlenecekGuid { get; set; }
    public string? BinaDisKapiNo { get; set; }
    public string? BasvuruKodu { get; set; }
    public string? TcKimlikNo { get; set; }
    public string? TcKimlikNoRaw { get; set; }
    public string? Ad { get; set; }
    public string? Soyad { get; set; }
    public string? SonuclandirmaAciklamasi { get; set; }
    public string? TuzelKisiAdi { get; set; }
    public string? TuzelKisiVergiNo { get; set; }
    public string? TuzelKisiMersisNo { get; set; }
    public string? CepTelefonu { get; set; }
    public string? Eposta { get; set; }
    public string? BasvuruKanalAd { get; set; }
    public string? Color { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public long? BasvuruTurId { get; set; }
    public string? BasvuruTurAd { get; set; }
    public long? BasvuruDestekTurId { get; set; }
    public string? BasvuruDestekTurAd { get; set; }
    public string? TapuBlok { get; set; }
    public int? TapuTasinmazId { get; set; }
    public long? TapuArsaPay { get; set; }
    public long? TapuArsaPayda { get; set; }
    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }
    public int? BasvuruDurumId { get; set; }
    public string? BasvuruDurumAd { get; set; }
    public string? BasvuruDegerlendirmeDurumAd { get; set; }
    public long? BinaDegerlendirmeDurumId { get; set; }
    public string? DurumClassName { get; set; }
    public long? BasvuruAfadDurumId { get; set; }
    public string? BasvuruAfadDurumAd { get; set; }
    public int? HibeOdemeTutar { get; set; }
    public int? KrediOdemeTutar { get; set; }
    public double? BagimsizBolumAlani { get; set; }
    public string? BagimsizBolumNo { get; set; }
    public DateTime? OlusturmaTarihi { get; set; }
    public bool? IptalEdilebilirMi { get; set; }
    public bool? ImzaVerilebilirMi { get; set; }
    public bool? AfadDurumDegisebilirMi { get; set; }
    public Guid? AfadDurumIptalDosyaId { get; set; }
    public Guid? FeragatnameDosyaId { get; set; }
    public int? HissePay { get; set; }
    public int? HissePayda { get; set; }
}