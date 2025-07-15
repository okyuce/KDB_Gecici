using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class HizmetAciklama
{
    public long HizmetAciklamaId { get; set; }

    public string Icerik { get; set; } = null!;

    /// <summary>
    /// tüzel veya gerçek kişi açıklaması
    /// </summary>
    public bool TuzelMi { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }
}
