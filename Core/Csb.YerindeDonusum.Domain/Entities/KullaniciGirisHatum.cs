using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class KullaniciGirisHatum
{
    public long KullaniciGirisHataId { get; set; }

    public string? KullaniciAdi { get; set; }

    public long? KullaniciId { get; set; }

    public string? Sifre { get; set; }

    public string IpAdres { get; set; } = null!;

    public long? OlusturanKullaniciId { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public bool SilindiMi { get; set; }

    public string? Aciklama { get; set; }

    public string? RequestHeader { get; set; }

    public string? Code { get; set; }

    public string GirisGuid { get; set; } = null!;

    public virtual Kullanici? Kullanici { get; set; }
}
