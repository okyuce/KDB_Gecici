using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities.Kds;

public partial class TblBagimsizBolum
{
    public long TblBagimsizBolumId { get; set; }

    public long BagimsizBolumKod { get; set; }

    public long? AdresNo { get; set; }

    public int? DurumKod { get; set; }

    public string? DurumAck { get; set; }

    public int? TipKod { get; set; }

    public string? TipAck { get; set; }

    public long BinaNo { get; set; }

    public long YapiKod { get; set; }

    public string? IcKapiNo { get; set; }

    public string? Katno { get; set; }

    public string? Tapubagimsizbolumno { get; set; }

    public bool? Aktif { get; set; }

    public long Versiyon { get; set; }
}
