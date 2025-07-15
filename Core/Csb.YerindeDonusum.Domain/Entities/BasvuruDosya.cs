using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BasvuruDosya
{
    public long BasvuruDosyaId { get; set; }

    public Guid BasvuruDosyaGuid { get; set; }

    public long BasvuruId { get; set; }

    public long BasvuruDosyaTurId { get; set; }

    public string DosyaAdi { get; set; } = null!;

    public string? DosyaYolu { get; set; }

    public string DosyaTuru { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime? OlusturmaTarihi { get; set; }

    public long? OlusturanKullaniciId { get; set; }

    public virtual Basvuru Basvuru { get; set; } = null!;

    public virtual BasvuruDosyaTur BasvuruDosyaTur { get; set; } = null!;
}
