using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities.YapiDenetimSeviye;

public partial class MvMuteahhitYibfList
{
    public long? YibfNo { get; set; }

    public long? IlId { get; set; }

    public string? IlKod { get; set; }

    public string? Il { get; set; }

    public long? IlceId { get; set; }

    public string? IlceKod { get; set; }

    public string? Ilce { get; set; }

    public long? MahalleId { get; set; }

    public string? MahalleKod { get; set; }

    public string? Mahalle { get; set; }

    public string? Ada { get; set; }

    public string? Parsel { get; set; }

    public DateTime? YapiRuhsatOnayTarihi { get; set; }

    public string? RuhsatNo { get; set; }

    public decimal? Seviye { get; set; }

    public string? SantiyeSefi { get; set; }

    public string? YapiMuteahhiti { get; set; }

    public string? MutTicKayitNo { get; set; }
}
