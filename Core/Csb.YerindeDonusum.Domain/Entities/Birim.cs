using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class Birim
{
    public long BirimId { get; set; }

    public string Ad { get; set; } = null!;

    public long KurumId { get; set; }

    public int? IlId { get; set; }

    public virtual ICollection<Kullanici> Kullanicis { get; set; } = new List<Kullanici>();

    public virtual Kurum Kurum { get; set; } = null!;
}
