using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BasvuruImzaVerenDosyaTur
{
    public long BasvuruImzaVerenDosyaTurId { get; set; }

    public string Ad { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual ICollection<BasvuruImzaVerenDosya> BasvuruImzaVerenDosyas { get; set; } = new List<BasvuruImzaVerenDosya>();
}
