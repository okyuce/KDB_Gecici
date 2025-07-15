using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BasvuruDestekTur
{
    public long BasvuruDestekTurId { get; set; }

    public Guid BasvuruDestekTurGuid { get; set; }

    public string Ad { get; set; } = null!;

    public string? Aciklama { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual ICollection<BasvuruImzaVeren> BasvuruImzaVerens { get; set; } = new List<BasvuruImzaVeren>();

    public virtual ICollection<BasvuruKamuUstlenecek> BasvuruKamuUstleneceks { get; set; } = new List<BasvuruKamuUstlenecek>();

    public virtual ICollection<Basvuru> Basvurus { get; set; } = new List<Basvuru>();
}
