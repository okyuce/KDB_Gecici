using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaDegerlendirme
{
    public long BinaDegerlendirmeId { get; set; }

    public string HasarTespitAskiKodu { get; set; } = null!;

    public int UavtMahalleNo { get; set; }

    public long BinaDegerlendirmeDurumId { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public long OlusturanKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public string? Ada { get; set; }

    public string? Parsel { get; set; }

    public long? BultenNo { get; set; }

    public long? YibfNo { get; set; }

    public int? ImzalayanKisiSayisi { get; set; }

    public long? YapiKimlikNo { get; set; }

    public decimal? YapiInsaatAlan { get; set; }

    public int? BagimsizBolumSayisi { get; set; }

    public int? ToplamKatSayisi { get; set; }

    public int? KotAltKatSayisi { get; set; }

    public int? KotUstKatSayisi { get; set; }

    public string? FenniMesulTc { get; set; }

    public int UavtIlNo { get; set; }

    public int UavtIlceNo { get; set; }

    public string? UavtIlAdi { get; set; }

    public string? UavtIlceAdi { get; set; }

    public string? UavtMahalleAdi { get; set; }

    public DateTime? IzinBelgesiTarih { get; set; }

    public long? IzinBelgesiSayi { get; set; }

    public string? BinaDisKapiNo { get; set; }

    public long? AdaParselGuncellemeTipiId { get; set; }

    public DateTime? RuhsatOnayTarihi { get; set; }

    public virtual ICollection<BasvuruKamuUstlenecek> BasvuruKamuUstleneceks { get; set; } = new List<BasvuruKamuUstlenecek>();

    public virtual ICollection<Basvuru> Basvurus { get; set; } = new List<Basvuru>();

    public virtual ICollection<BinaAdinaYapilanYardim> BinaAdinaYapilanYardims { get; set; } = new List<BinaAdinaYapilanYardim>();

    public virtual ICollection<BinaDegerlendirmeDosya> BinaDegerlendirmeDosyas { get; set; } = new List<BinaDegerlendirmeDosya>();

    public virtual BinaDegerlendirmeDurum BinaDegerlendirmeDurum { get; set; } = null!;

    public virtual ICollection<BinaMuteahhit> BinaMuteahhits { get; set; } = new List<BinaMuteahhit>();

    public virtual ICollection<BinaNakdiYardimTaksit> BinaNakdiYardimTaksits { get; set; } = new List<BinaNakdiYardimTaksit>();

    public virtual ICollection<BinaOdeme> BinaOdemes { get; set; } = new List<BinaOdeme>();

    public virtual ICollection<BinaYapiDenetimSeviyeTespit> BinaYapiDenetimSeviyeTespits { get; set; } = new List<BinaYapiDenetimSeviyeTespit>();

    public virtual ICollection<BinaYapiRuhsatIzinDosya> BinaYapiRuhsatIzinDosyas { get; set; } = new List<BinaYapiRuhsatIzinDosya>();
}
