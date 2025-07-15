using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaYapiRuhsatIzinDosya
{
    public long BinaYapiRuhsatIzinDosyaId { get; set; }

    public long BinaDegerlendirmeId { get; set; }

    public Guid BinaYapiRuhsatIzinDosyaGuid { get; set; }

    public string DosyaAdi { get; set; } = null!;

    public string? DosyaYolu { get; set; }

    public string DosyaTuru { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual BinaDegerlendirme BinaDegerlendirme { get; set; } = null!;
}
