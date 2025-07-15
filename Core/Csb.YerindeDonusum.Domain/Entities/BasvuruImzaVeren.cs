using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BasvuruImzaVeren
{
    public long BasvuruImzaVerenId { get; set; }

    public long? BasvuruId { get; set; }

    public long BasvuruDestekTurId { get; set; }

    public long BasvuruTurId { get; set; }

    public int? HibeOdemeTutar { get; set; }

    public int? KrediOdemeTutar { get; set; }

    /// <summary>
    /// metre kare
    /// </summary>
    public double BagimsizBolumAlani { get; set; }

    public string? NotBilgi { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public long OlusturanKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public string? BagimsizBolumNo { get; set; }

    public DateOnly? SozlesmeTarihi { get; set; }

    public long? BasvuruKamuUstlenecekId { get; set; }

    public string? IbanNo { get; set; }

    public bool IbanGirildiMi { get; set; }

    public int? HissePay { get; set; }

    public int? HissePayda { get; set; }

    public virtual Basvuru? Basvuru { get; set; }

    public virtual BasvuruDestekTur BasvuruDestekTur { get; set; } = null!;

    public virtual ICollection<BasvuruImzaVerenDosya> BasvuruImzaVerenDosyas { get; set; } = new List<BasvuruImzaVerenDosya>();

    public virtual BasvuruKamuUstlenecek? BasvuruKamuUstlenecek { get; set; }

    public virtual BasvuruTur BasvuruTur { get; set; } = null!;
}
