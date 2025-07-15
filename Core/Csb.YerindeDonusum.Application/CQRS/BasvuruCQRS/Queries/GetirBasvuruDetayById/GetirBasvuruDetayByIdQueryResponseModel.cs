using Csb.YerindeDonusum.Application.Dtos;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruDetayById;

public class GetirBasvuruDetayByIdQueryResponseModel
{
    public Guid Id { get; set; }
    public string BasvuruKodu { get; set; }
    public string BasvuruKanali { get; set; }
    public string TcKimlikNo { get; set; } = null!;
    public string? Ad { get; set; }
    public string? Soyad { get; set; }
    public string? CepTelefonu { get; set; }
    public string? Eposta { get; set; }
    public string? BasvuruDurumu { get; set; }
    public long? BasvuruDurumId { get; set; }
    public string? BasvuruTuru { get; set; }
    public string? BasvuruDestekTuru { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public string? HasarTespitGuclendirmeMahkemeSonucu { get; set; }
    public string? HasarTespitHasarDurumu { get; set; }
    public string? HasarTespitItirazSonucu { get; set; }
    public string? HasarTespitIlAdi { get; set; }
    public string? HasarTespitIlceAdi { get; set; }
    public string? HasarTespitMahalleAdi { get; set; }
    public string? HasarTespitAda { get; set; }
    public string? HasarTespitParsel { get; set; }
    public string? HasarTespitDisKapiNo { get; set; }
    public int? TapuAnaTasinmazId { get; set; }
    public int? TapuTasinmazId { get; set; }
    public string? TapuTasinmazTipi { get; set; }
    public string? TapuBagimsizBolumNo { get; set; }
    public string? TapuKat { get; set; }
    public string? TapuBeyanAciklama { get; set; }
    public string? TapuBlok { get; set; }
    public string? TapuGirisBilgisi { get; set; }
    public string? TapuNitelik { get; set; }
    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }
    public bool? TapuHazineArazisiMi { get; set; }
    public string? UavtAdresNo {get; set; }
    public string? UavtIlAdi { get; set; }
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public int? UavtCaddeNo { get; set; }
    public int? UavtMeskenBinaNo { get; set; }
    public string? UavtIlceAdi { get; set; }
    public string? UavtMahalleAdi { get; set; }
    public string? UavtCsbm { get; set; }
    public string? UavtDisKapiNo { get; set; }
    public string? UavtIcKapiNo { get; set; }
    public string? OlusturmaTarihi { get; set; }
    public bool IptalEdilebilirMi { get; set; } = false;
    public string? BasvuruIptalAciklamasi { get; set; }
    public string? TuzelKisiVergiNumarasi { get; set; }
    public string? TuzelKisiAdi { get; set; }
    public int? TuzelKisiTipId { get; set; }
    public bool? UavtBeyanMi { get; set; }
    public List<DosyaIceriksizDto> DosyaBasvuruListe { get; set; }
}