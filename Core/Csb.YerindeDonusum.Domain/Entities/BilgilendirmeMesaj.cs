using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BilgilendirmeMesaj
{
    public int BilgilendirmeMesajId { get; set; }

    public string Anahtar { get; set; } = null!;

    public string Deger { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }
}
