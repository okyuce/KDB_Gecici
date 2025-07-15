using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaDegerlendirmeDurum
{
    public long BinaDegerlendirmeDurumId { get; set; }

    public string Ad { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual ICollection<BinaDegerlendirme> BinaDegerlendirmes { get; set; } = new List<BinaDegerlendirme>();
}
