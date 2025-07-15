using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class KullaniciRol
{
    public long KullaniciRolId { get; set; }

    public long KullaniciId { get; set; }

    public long RolId { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public long OlusturanKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual Kullanici Kullanici { get; set; } = null!;

    public virtual Rol Rol { get; set; } = null!;
}
