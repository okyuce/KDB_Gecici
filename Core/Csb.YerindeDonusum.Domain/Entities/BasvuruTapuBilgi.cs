using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BasvuruTapuBilgi
{
    public long BasvuruTapuBilgiId { get; set; }

    public long BasvuruId { get; set; }

    public long HissePay { get; set; }

    public long HissePayda { get; set; }

    /// <summary>
    /// iştiraklı vb.
    /// </summary>
    public string? HisseTuru { get; set; }

    public int? IstirakNo { get; set; }

    public string? TapuMudurlugu { get; set; }

    public string? IslemTanim { get; set; }

    public int? YevmiyeNo { get; set; }

    public DateTime? YevmiheTarihi { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual Basvuru Basvuru { get; set; } = null!;
}
