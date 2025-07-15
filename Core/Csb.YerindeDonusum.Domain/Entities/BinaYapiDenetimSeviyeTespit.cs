using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaYapiDenetimSeviyeTespit
{
    public long BinaYapiDenetimSeviyeTespitId { get; set; }

    public long BinaDegerlendirmeId { get; set; }

    public int IlerlemeYuzdesi { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public long OlusturanKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual BinaDegerlendirme BinaDegerlendirme { get; set; } = null!;

    public virtual ICollection<BinaYapiDenetimSeviyeTespitDosya> BinaYapiDenetimSeviyeTespitDosyas { get; set; } = new List<BinaYapiDenetimSeviyeTespitDosya>();
}
