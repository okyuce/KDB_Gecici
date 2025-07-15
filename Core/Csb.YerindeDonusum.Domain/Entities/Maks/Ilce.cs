using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Csb.YerindeDonusum.Domain.Entities.Maks;

public partial class Ilce
{
    public int? Objectid { get; set; }

    public string? Id { get; set; }

    public string? Ad { get; set; }

    public int? Kimlikno { get; set; }

    public string? Ilid { get; set; }

    public string? Globalid { get; set; }

    public DateTime? Olusturmatarihi { get; set; }

    public DateTime? Degistirmetarihi { get; set; }

    public DateTime? Gecerliliktarihi { get; set; }

    public int? Ihtilafdurumu { get; set; }

    public string? Ihtilafnedeni { get; set; }

    public string? Kuruluskararsayisi { get; set; }

    public DateTime? Kuruluskarartarihi { get; set; }

    public short? Merkezilcemi { get; set; }

    public double? Olusturan { get; set; }

    public double? Degistiren { get; set; }

    public int? Operationnumber { get; set; }

    public string? Operationdescription { get; set; }

    public int? Uavtmerkezbucakkodu { get; set; }

    public int? Uavtmerkezkoykodu { get; set; }

    public int? Verikaynagi { get; set; }

    public string? SeAnnoCadData { get; set; }

    public double? ShapeLength { get; set; }

    public double? ShapeArea { get; set; }

    public Geometry? Geom { get; set; }

    public char? Islem { get; set; }

    public DateTime? Islemtarih { get; set; }

    public long? Versiyon { get; set; }

    public long Seqid { get; set; }
}
