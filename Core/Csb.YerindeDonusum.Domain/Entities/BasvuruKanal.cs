using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BasvuruKanal
{
    public long BasvuruKanalId { get; set; }

    public string Ad { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public Guid BasvuruKanalGuid { get; set; }

    public virtual ICollection<Basvuru> Basvurus { get; set; } = new List<Basvuru>();
}
