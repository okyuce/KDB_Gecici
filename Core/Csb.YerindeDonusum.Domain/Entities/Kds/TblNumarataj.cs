using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities.Kds;

public partial class TblNumarataj
{
    public long TblNumaratajId { get; set; }

    public long NumaratajKod { get; set; }

    public long NumaratajKimlikno { get; set; }

    public string? KapiNo { get; set; }

    public long? YapiKod { get; set; }

    public long? DigerYapiKod { get; set; }

    public long? AnaNumaratajKod { get; set; }

    public long CsbmKod { get; set; }

    public string? NumaratajTipTanimKod { get; set; }

    public bool? Aktif { get; set; }

    public long Versiyon { get; set; }

    public virtual TblCsbm CsbmKodNavigation { get; set; } = null!;
}
