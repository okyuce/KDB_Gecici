using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class TakbisIl
{
    public int TakbisIlId { get; set; }

    public int TakbisIlKod { get; set; }

    public string? Ad { get; set; }

    public int? IlKod { get; set; }

    public bool? Aktif { get; set; }

    public DateTime? EklenmeTarihi { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }
}
