using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class SikcaSorulanSoru
{
    public long SikcaSorulanSoruId { get; set; }

    public string Soru { get; set; } = null!;

    public string Cevap { get; set; } = null!;

    public int? SiraNo { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public long OlusturanKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }
}
