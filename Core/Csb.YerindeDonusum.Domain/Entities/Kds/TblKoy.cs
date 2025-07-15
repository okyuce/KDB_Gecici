using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities.Kds;

public partial class TblKoy
{
    /// <summary>
    /// primary key
    /// </summary>
    public int TblKoyId { get; set; }

    /// <summary>
    /// #KoyKod
    /// </summary>
    public int KoyKod { get; set; }

    /// <summary>
    /// #KoyAd
    /// </summary>
    public string Ad { get; set; } = null!;

    /// <summary>
    /// #BucakKod
    /// </summary>
    public int BucakKod { get; set; }

    /// <summary>
    /// #Aktif
    /// </summary>
    public bool? Aktif { get; set; }

    /// <summary>
    /// #versiyonNo
    /// </summary>
    public long Versiyon { get; set; }

    public virtual ICollection<TblMahalle> TblMahalles { get; set; } = new List<TblMahalle>();

    public virtual ICollection<TblYetkiliIdare> TblYetkiliIdares { get; set; } = new List<TblYetkiliIdare>();
}
