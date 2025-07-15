using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class Ayar
{
    public long AyarId { get; set; }

    public string Deger { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }
}
