using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaDegerlendirmeDosyaTur
{
    public long BinaDegerlendirmeDosyaTurId { get; set; }

    public Guid BinaDegerlendirmeDosyaTurGuid { get; set; }

    public string Ad { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual ICollection<BinaDegerlendirmeDosya> BinaDegerlendirmeDosyas { get; set; } = new List<BinaDegerlendirmeDosya>();
}
