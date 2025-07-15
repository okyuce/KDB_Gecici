using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaKotUstuSayi
{
    public long BinaKotUstuSayiId { get; set; }

    public int UavtIlId { get; set; }

    public int? UavtIlceId { get; set; }

    public short KotUstuKatSayisi { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public int? UavtMahalleId { get; set; }

    public string? Ada { get; set; }

    public string? Parsel { get; set; }
}
