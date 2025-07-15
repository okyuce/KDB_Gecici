using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class KullaniciHesapTip
{
    public long KullaniciHesapTipId { get; set; }

    public string Ad { get; set; } = null!;

    public virtual ICollection<Kullanici> Kullanicis { get; set; } = new List<Kullanici>();
}
