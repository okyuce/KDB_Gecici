using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities.Kds;

public partial class TblCsbm
{
    /// <summary>
    /// primary key
    /// </summary>
    public long TblCsbmId { get; set; }

    /// <summary>
    /// #YolKod
    /// </summary>
    public long CsbmKod { get; set; }

    /// <summary>
    /// #YolAd
    /// </summary>
    public string? Ad { get; set; }

    /// <summary>
    /// #MahalleKod
    /// </summary>
    public int MahalleKod { get; set; }

    /// <summary>
    /// yol tipinin tanim kodu #TanimKod
    /// </summary>
    public string? TipKod { get; set; }

    /// <summary>
    /// yol gelismislik tanim kodu #TanimKod
    /// </summary>
    public string? GelismislikDurumKod { get; set; }

    /// <summary>
    /// #YerelIdareKod
    /// </summary>
    public int? YetkiliIdareKod { get; set; }

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
    /// #Versiyon
    /// </summary>
    public long Versiyon { get; set; }

    public virtual TblTanim? GelismislikDurumKodNavigation { get; set; }

    public virtual TblIl? IlKodNavigation { get; set; }

    public virtual TblIlce? IlceKodNavigation { get; set; }

    public virtual ICollection<TblNumarataj> TblNumaratajs { get; set; } = new List<TblNumarataj>();

    public virtual TblTanim? TipKodNavigation { get; set; }

    public virtual TblYetkiliIdare? YetkiliIdareKodNavigation { get; set; }
}
