using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class KullaniciGirisKodDeneme
{
    public long KullaniciGirisKodDenemeId { get; set; }

    public long? KullaniciId { get; set; }

    public string? Code { get; set; }

    public string? IpAdres { get; set; }

    public long? OlusturanKullaniciId { get; set; }

    public DateTime? OlusturmaTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public string? GirisGuid { get; set; }

    public bool? SilindiMi { get; set; }

    public virtual Kullanici? Kullanici { get; set; }
}
