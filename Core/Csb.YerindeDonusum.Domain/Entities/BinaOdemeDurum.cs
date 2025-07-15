using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaOdemeDurum
{
    public long BinaOdemeDurumId { get; set; }

    public string Ad { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual ICollection<BinaOdeme> BinaOdemes { get; set; } = new List<BinaOdeme>();
}
