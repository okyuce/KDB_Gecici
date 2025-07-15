using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaOdemeDetay
{
    public long BinaOdemeDetayId { get; set; }

    public long BinaOdemeId { get; set; }

    public string Ad { get; set; } = null!;

    public string Iban { get; set; } = null!;

    public bool MuteahhitMi { get; set; }

    public decimal OdemeTutari { get; set; }

    public decimal? HibeOdemeTutari { get; set; }

    public decimal? KrediOdemeTutari { get; set; }

    public decimal? DigerHibeOdemeTutari { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public long OlusturanKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual BinaOdeme BinaOdeme { get; set; } = null!;

    public virtual Kullanici? GuncelleyenKullanici { get; set; }

    public virtual Kullanici OlusturanKullanici { get; set; } = null!;
}
