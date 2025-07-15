using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaYapiDenetimSeviyeTespitDosya
{
    public long BinaYapiDenetimSeviyeTespitDosyaId { get; set; }

    public long BinaYapiDenetimSeviyeTespitId { get; set; }

    public Guid BinaYapiDenetimSeviyeTespitDosyaGuid { get; set; }

    public string DosyaAdi { get; set; } = null!;

    public string? DosyaYolu { get; set; }

    public string DosyaTuru { get; set; } = null!;

    public string? OlusturanIp { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public long OlusturanKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual BinaYapiDenetimSeviyeTespit BinaYapiDenetimSeviyeTespit { get; set; } = null!;
}
