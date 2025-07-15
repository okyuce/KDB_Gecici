using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaMuteahhitTapuTur
{
    public long BinaMuteahhitTapuTurId { get; set; }

    public string Ad { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual ICollection<BinaMuteahhit> BinaMuteahhits { get; set; } = new List<BinaMuteahhit>();
}
