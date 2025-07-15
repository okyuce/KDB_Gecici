using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaAdinaYapilanYardim
{
    public long BinaAdinaYapilanYardimId { get; set; }

    public long BinaDegerlendirmeId { get; set; }

    public long BinaAdinaYapilanYardimTipiId { get; set; }

    public DateTime Tarih { get; set; }

    public long Tutar { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public long OlusturanKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual BinaAdinaYapilanYardimTipi BinaAdinaYapilanYardimTipi { get; set; } = null!;

    public virtual BinaDegerlendirme BinaDegerlendirme { get; set; } = null!;
}
