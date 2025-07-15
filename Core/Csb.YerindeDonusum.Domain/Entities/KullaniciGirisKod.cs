using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class KullaniciGirisKod
{
    public long KullaniciGirisKodId { get; set; }

    public long KullaniciId { get; set; }

    public string Code { get; set; } = null!;

    public bool Tamamlandi { get; set; }

    public long? OlusturanKullaniciId { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public string? OlusturanIp { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public string? GuncelleyenIp { get; set; }

    public Guid GirisGuid { get; set; }

    public string Telefon { get; set; } = null!;

    public bool SmsGonderildiMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual Kullanici Kullanici { get; set; } = null!;
}
