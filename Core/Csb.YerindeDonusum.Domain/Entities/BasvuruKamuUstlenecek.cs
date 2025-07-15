using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BasvuruKamuUstlenecek
{
    public long BasvuruKamuUstlenecekId { get; set; }

    public Guid BasvuruKamuUstlenecekGuid { get; set; }

    public long BasvuruDestekTurId { get; set; }

    public long BasvuruTurId { get; set; }

    public long BasvuruDurumId { get; set; }

    public string TcKimlikNo { get; set; } = null!;

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

    public int? UavtIlNo { get; set; }

    public int? UavtIlceNo { get; set; }

    public int? UavtMahalleNo { get; set; }

    public string? UavtIlAdi { get; set; }

    public string? UavtIlKodu { get; set; }

    public string? UavtIlceAdi { get; set; }

    public string? UavtIlceKodu { get; set; }

    public string? UavtMahalleAdi { get; set; }

    public string? UavtMahalleKodu { get; set; }

    public string? TuzelKisiVergiNo { get; set; }

    public string? TuzelKisiAdi { get; set; }

    public string? TuzelKisiMersisNo { get; set; }

    public string? TuzelKisiAdres { get; set; }

    public string? TuzelKisiYetkiTuru { get; set; }

    public int? TuzelKisiTipId { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public long OlusturanKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public DateTime? BasvuruAfadDurumGuncellemeTarihi { get; set; }

    public long? BasvuruAfadDurumId { get; set; }

    public long? BinaDegerlendirmeId { get; set; }

    public long? TapuArsaPay { get; set; }

    public long? TapuArsaPayda { get; set; }

    public string? SonuclandirmaAciklamasi { get; set; }

    public int? EskiTapuMahalleId { get; set; }

    public string? EskiTapuMahalleAdi { get; set; }

    public string? EskiTapuAda { get; set; }

    public string? EskiTapuParsel { get; set; }

    public DateTime? EskiTapuGuncellemeTarihi { get; set; }

    public long? EskiTapuGuncelleyenKullaniciId { get; set; }

    public int? EskiTapuIlceId { get; set; }

    public string? EskiTapuIlceAdi { get; set; }

    public long? BasvuruIptalTurId { get; set; }

    public bool? PasifMaliyeHazinesiMi { get; set; }

    public virtual BasvuruAfadDurum? BasvuruAfadDurum { get; set; }

    public virtual BasvuruDestekTur BasvuruDestekTur { get; set; } = null!;

    public virtual BasvuruDurum BasvuruDurum { get; set; } = null!;

    public virtual ICollection<BasvuruImzaVeren> BasvuruImzaVerens { get; set; } = new List<BasvuruImzaVeren>();

    public virtual BasvuruTur BasvuruTur { get; set; } = null!;

    public virtual BinaDegerlendirme? BinaDegerlendirme { get; set; }
}
