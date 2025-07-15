using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaAdinaYapilanYardimTipi
{
    public long BinaAdinaYapilanYardimTipiId { get; set; }

    public string Adi { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public virtual ICollection<BinaAdinaYapilanYardim> BinaAdinaYapilanYardims { get; set; } = new List<BinaAdinaYapilanYardim>();
}
