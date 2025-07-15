using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class AydinlatmaMetni
{
    public long AydinlatmaMetniId { get; set; }

    public Guid AydinlatmaMetniGuid { get; set; }

    public string Icerik { get; set; } = null!;

    public DateTime OlusturmaTarihi { get; set; }

    public long OlusturanKullaniciId { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual ICollection<Basvuru> Basvurus { get; set; } = new List<Basvuru>();
}
