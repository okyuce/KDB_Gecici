using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Csb.YerindeDonusum.Domain.Entities.Maks;

public partial class TopluBagimsizbolum
{
    public string? Iladi { get; set; }

    public short? IlId { get; set; }

    public string? Ilceadi { get; set; }

    public int? IlceId { get; set; }

    public string? Mahalleadi { get; set; }

    public int? MahId { get; set; }

    public string? Yoladi { get; set; }

    public int? YohId { get; set; }

    public double? BagNo { get; set; }

    public int? YapiNo { get; set; }

    public int? NumaratajNo { get; set; }

    public int? Kullanimturu { get; set; }

    public int? Kullanimalttur { get; set; }

    public int? NumaratajTip { get; set; }

    public int? YapiTip { get; set; }

    public string? Bagimsizbolumno { get; set; }

    public int? Uavtkodu { get; set; }

    public char? Durum { get; set; }

    public Geometry? Geom { get; set; }
}
