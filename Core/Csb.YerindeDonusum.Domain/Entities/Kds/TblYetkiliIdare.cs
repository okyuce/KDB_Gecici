using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities.Kds;

public partial class TblYetkiliIdare
{
    /// <summary>
    /// primary key
    /// </summary>
    public int TblYetkiliIdareId { get; set; }

    /// <summary>
    /// #YerelIdareKod
    /// </summary>
    public int YetkiliIdareKod { get; set; }

    /// <summary>
    /// #TanimKod
    /// </summary>
    public string KurumTurKod { get; set; } = null!;

    /// <summary>
    /// #YerelIdareAd
    /// </summary>
    public string Ad { get; set; } = null!;

    /// <summary>
    /// #KoyKod
    /// </summary>
    public int? KoyKod { get; set; }

    /// <summary>
    /// #IlceKod
    /// </summary>
    public int? IlceKod { get; set; }

    /// <summary>
    /// #IlKod
    /// </summary>
    public int? IlKod { get; set; }

    /// <summary>
    /// #Aktif
    /// </summary>
    public bool? Aktif { get; set; }

    /// <summary>
    /// #VersiyonNo
    /// </summary>
    public long Versiyon { get; set; }

    public virtual TblIl? IlKodNavigation { get; set; }

    public virtual TblIlce? IlceKodNavigation { get; set; }

    public virtual TblKoy? KoyKodNavigation { get; set; }

    public virtual TblTanim KurumTurKodNavigation { get; set; } = null!;

    public virtual ICollection<TblCsbm> TblCsbms { get; set; } = new List<TblCsbm>();

    public virtual ICollection<TblMahalle> TblMahalles { get; set; } = new List<TblMahalle>();
}
