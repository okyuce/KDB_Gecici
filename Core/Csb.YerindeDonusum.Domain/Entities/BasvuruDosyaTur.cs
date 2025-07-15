using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BasvuruDosyaTur
{
    public long BasvuruDosyaTurId { get; set; }

    public Guid BasvuruDosyaTurGuid { get; set; }

    public string Ad { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual ICollection<BasvuruDosya> BasvuruDosyas { get; set; } = new List<BasvuruDosya>();
}
