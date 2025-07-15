using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BasvuruIptalTur
{
    public long BasvuruIptalTurId { get; set; }

    public string Ad { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual ICollection<Basvuru> Basvurus { get; set; } = new List<Basvuru>();
}
