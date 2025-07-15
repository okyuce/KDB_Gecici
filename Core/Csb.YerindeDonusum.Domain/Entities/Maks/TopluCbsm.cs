using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Csb.YerindeDonusum.Domain.Entities.Maks;

public partial class TopluCsbm
{
    public string? Iladi { get; set; }

    public short? IlId { get; set; }

    public string? Ilceadi { get; set; }

    public int? IlceId { get; set; }

    public string? Mahalleadi { get; set; }

    public int? MahId { get; set; }

    public string? Yoladi { get; set; }

    public int? YohId { get; set; }

    public int? Tip { get; set; }

    public int? Uavtkodu { get; set; }

    public Geometry? Geom { get; set; }
}
