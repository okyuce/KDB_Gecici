using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class KullaniciGirisBasarili
{
    public long KullaniciGirisBasariliId { get; set; }

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

    public virtual Kullanici? Kullanici { get; set; }
}
