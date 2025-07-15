using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class Kurum
{
    public long KurumId { get; set; }

    public string Ad { get; set; } = null!;

    public virtual ICollection<Birim> Birims { get; set; } = new List<Birim>();
}
