using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.EkleBasvuru;

public class EkleBasvuruCommandModel
{
    public string? BasvuruDestekTurId { get; set; }

    public string? BasvuruTurId { get; set; }

    /// <summary>
    /// 1: tc vatandaşı, 2: yabancı
    /// </summary>
    public short? VatandaslikDurumu { get; set; }

    public string? TcKimlikNo { get; set; }

    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public string? CepTelefonu { get; set; }

    public string? Eposta { get; set; }
    public string? HasarTespitUid { get; set; }

    public int? TapuAnaTasinmazId { get; set; }

    public int? TapuTasinmazId { get; set; }

    public int? TapuIlId { get; set; }

    public string? TapuIlAdi { get; set; }

    public int? TapuIlceId { get; set; }

    public string? TapuIlceAdi { get; set; }

    public int? TapuMahalleId { get; set; }

    public string? TapuMahalleAdi { get; set; }

    public string? TapuAda { get; set; }

    public string? TapuParsel { get; set; }

    public string? TapuTasinmazTipi { get; set; }

    public string? TapuBagimsizBolumNo { get; set; }

    public int? TapuKat { get; set; }

    public string? TapuBlok { get; set; }

    public string? TapuGirisBilgisi { get; set; }

    public string? TapuNitelik { get; set; }

    public string? TapuBeyanAciklama { get; set; }

    public string? TapuRehinDurumu { get; set; }

    public bool? TapuHazineArazisiMi { get; set; }

    public int? UavtIlNo { get; set; }

    public int? UavtIlceNo { get; set; }

    public int? UavtMahalleNo { get; set; }

    public int? UavtCaddeNo { get; set; }

    public int? UavtMeskenBinaNo { get; set; }

    public string? UavtAcikAdres { get; set; }

    public string? UavtAdresNo { get; set; }

    public string? UavtBinaAda { get; set; }

    public string? UavtBinaBlokAdi { get; set; }

    public string? UavtBinaKodu { get; set; }

    public string? UavtBinaNo { get; set; }

    public string? UavtBinaPafta { get; set; }

    public string? UavtBinaParsel { get; set; }

    public string? UavtBinaSiteAdi { get; set; }

    public string? UavtCsbm { get; set; }

    public string? UavtCsbmKodu { get; set; }

    public string? UavtDisKapiNo { get; set; }

    public string? UavtIcKapiNo { get; set; }

    public string? UavtIlAdi { get; set; }

    public string? UavtIlKodu { get; set; }

    public string? UavtIlceAdi { get; set; }

    public string? UavtIlceKodu { get; set; }

    public string? UavtMahalleAdi { get; set; }

    public string? UavtMahalleKodu { get; set; }

    public string? UavtNitelik { get; set; }

    public string? TuzelKisiVergiNo { get; set; }

    public string? TuzelKisiAdi { get; set; }

    public string? TuzelKisiMersisNo { get; set; }

    public string? TuzelKisiAdres { get; set; }

    public string? TuzelKisiYetkiTuru { get; set; }

    public int? TuzelKisiTipId { get; set; }

    public string? AydinlatmaMetniId { get; set; }

    public bool? KacakYapiMi { get; set; }

    public string? BasvuruKanalId { get; set; }
    public bool? UavtBeyanMi { get; set; }

    public List<BasvuruTapuBilgiDto>? BasvuruTapuBilgiListe { get; set; } = new List<BasvuruTapuBilgiDto>();

    public List<DosyaDto>? BasvuruDosyaListe { get; set; }
    public List<DosyaDto>? BasvuruTuzelYetkiDosyaListe { get; set; }
}