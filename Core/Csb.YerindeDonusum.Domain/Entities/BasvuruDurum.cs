using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BasvuruDurum
{
    public long BasvuruDurumId { get; set; }

    public Guid BasvuruDurumGuid { get; set; }

    public string Ad { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual ICollection<BasvuruKamuUstlenecek> BasvuruKamuUstleneceks { get; set; } = new List<BasvuruKamuUstlenecek>();

    public virtual ICollection<Basvuru> Basvurus { get; set; } = new List<Basvuru>();
}
