using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Application.Dtos;

public partial class KullaniciRolDto
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

    public virtual KullaniciDto Kullanici { get; set; } = null!;

    public virtual RolDto Rol { get; set; } = null!;
}
