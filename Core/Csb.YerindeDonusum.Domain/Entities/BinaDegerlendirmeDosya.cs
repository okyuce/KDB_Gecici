using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaDegerlendirmeDosya
{
    public long BinaDegerlendirmeDosyaId { get; set; }

    public long BinaDegerlendirmeId { get; set; }

    public Guid BinaDosyaGuid { get; set; }

    public string DosyaAdi { get; set; } = null!;

    public string? DosyaYolu { get; set; }

    public string DosyaTuru { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public long? BinaDegerlendirmeDosyaTurId { get; set; }

    public string? YeniYapiSilAciklama { get; set; }

    public virtual BinaDegerlendirme BinaDegerlendirme { get; set; } = null!;

    public virtual BinaDegerlendirmeDosyaTur? BinaDegerlendirmeDosyaTur { get; set; }
}
