using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities.Kds;

public partial class Basvuru
{
    public long? BasvuruId { get; set; }

    public string? BasvuruGuid { get; set; }

    public string? BasvuruKodu { get; set; }

    public long? BasvuruDestekTurId { get; set; }

    public long? BasvuruTurId { get; set; }

    public long? BasvuruDurumId { get; set; }

    public short? VatandaslikDurumu { get; set; }

    public string? TcKimlikNo { get; set; }

    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public string? CepTelefonu { get; set; }

    public string? Eposta { get; set; }

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

    public string? TapuRehinDurumu { get; set; }

    public int? TapuIstirakNo { get; set; }

    public string? TapuBeyanAciklama { get; set; }

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

    public string? HasarTespitUid { get; set; }

    public string? HasarTespitAskiKodu { get; set; }

    public string? HasarTespitHasarDurumu { get; set; }

    public string? HasarTespitItirazSonucu { get; set; }

    public string? HasarTespitIlAdi { get; set; }

    public string? HasarTespitIlceAdi { get; set; }

    public string? HasarTespitMahalleAdi { get; set; }

    public string? HasarTespitAda { get; set; }

    public string? HasarTespitParsel { get; set; }

    public string? HasarTespitDisKapiNo { get; set; }

    public long? BasvuruKanalId { get; set; }

    public long? AydinlatmaMetniId { get; set; }

    public bool? KacakYapiMi { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime? OlusturmaTarihi { get; set; }

    public long? OlusturanKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }

    public bool? AktifMi { get; set; }

    public bool? SilindiMi { get; set; }

    public long? TapuArsaPay { get; set; }

    public long? TapuArsaPayda { get; set; }

    public bool? TapuHazineArazisiMi { get; set; }

    public string? SonuclandirmaAciklamasi { get; set; }

    public int? TapuToplamBagimsizBolumSayisi { get; set; }

    public decimal? TapuToplamKisiHisseOrani { get; set; }

    public int? TapuToplamKisiBagimsizBolumSayisi { get; set; }

    public DateTime? BasvuruAfadDurumGuncellemeTarihi { get; set; }

    public long? BasvuruAfadDurumId { get; set; }

    public long? BinaDegerlendirmeId { get; set; }

    public DateTime? BasvuruKamuUstlenecekGuncellemeTarihi { get; set; }

    public bool? UavtBeyanMi { get; set; }

    public int? EskiTapuMahalleId { get; set; }

    public string? EskiTapuMahalleAdi { get; set; }

    public string? EskiTapuAda { get; set; }

    public string? EskiTapuParsel { get; set; }

    public DateTime? EskiTapuGuncellemeTarihi { get; set; }

    public long? EskiTapuGuncelleyenKullaniciId { get; set; }

    public int? EskiTapuIlceId { get; set; }

    public string? EskiTapuIlceAdi { get; set; }

    public string? BasvuruIptalAciklamasi { get; set; }
}
