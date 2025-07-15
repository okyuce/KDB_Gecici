using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities.Kds;

public partial class KdsYerindedonusumRezervAlanlar
{
    public long Id { get; set; }

    public int? IlId { get; set; }

    public string? IlAdi { get; set; }

    public int? IlceId { get; set; }

    public string? IlceAdi { get; set; }

    public int? MahalleId { get; set; }

    public string? MahalleAdi { get; set; }

    public string? RezevrAlanAdi { get; set; }

    public string? HasarTespitUid { get; set; }

    public string? HasarTespitAskiKodu { get; set; }

    public string? ItirazSonucuHasarDurum { get; set; }

    public string? KentselKırsal { get; set; }

    public int? AfetId { get; set; }

    public DateTime? Tarih { get; set; }

    public bool? Aktifmi { get; set; }

    public DateTime? EtlDate { get; set; }
}
