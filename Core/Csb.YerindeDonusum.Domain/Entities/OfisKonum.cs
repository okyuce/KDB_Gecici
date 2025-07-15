using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class OfisKonum
{
    public long OfisKonumId { get; set; }

    public string IlAdi { get; set; } = null!;

    public string IlceAdi { get; set; } = null!;

    public string Adres { get; set; } = null!;

    public string? HaritaUrl { get; set; }

    public Geometry Konum { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public long OlusturanKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }
}
