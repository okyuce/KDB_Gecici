using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities.Kds;

/// <summary>
/// mahalle ve koy kayitlari bu tabloda toplanmaktadir. Hizli search islemeri icin tum ustr veri iliskisi taboya eklenmistir. #Mahalle
/// </summary>
public partial class TblMahalle
{
    /// <summary>
    /// primary key
    /// </summary>
    public long MahalleId { get; set; }

    /// <summary>
    /// Mahalle  kaydinin aks kodu #MahalleKod
    /// </summary>
    public int MahalleKod { get; set; }

    /// <summary>
    /// mahalle adi #MahalleAd
    /// </summary>
    public string Ad { get; set; } = null!;

    /// <summary>
    /// ilcenin uavt kodu #TanimKod
    /// </summary>
    public string MahalleTanimKod { get; set; } = null!;

    /// <summary>
    /// #KoyKod
    /// </summary>
    public int? KoyKod { get; set; }

    /// <summary>
    /// #YetkiliIdareKod
    /// </summary>
    public int? YetkiliIdareKod { get; set; }

    /// <summary>
    /// #BucakKod
    /// </summary>
    public int? BucakKod { get; set; }

    /// <summary>
    /// #IlceKod
    /// </summary>
    public int? IlceKod { get; set; }

    /// <summary>
    /// #IlKod
    /// </summary>
    public int? IlKod { get; set; }

    /// <summary>
    /// #TapuMahalleKod
    /// </summary>
    public int? TakbisMahalleKod { get; set; }

    /// <summary>
    /// #Aciklama
    /// </summary>
    public string? Aciklama { get; set; }

    /// <summary>
    /// #ResmiTarih
    /// </summary>
    public DateOnly? ResmiGazeteTarihi { get; set; }

    /// <summary>
    /// #Aktif
    /// </summary>
    public bool? Aktif { get; set; }

    /// <summary>
    /// guncel versiyon numarasi
    /// </summary>
    public long? Versiyon { get; set; }

    public virtual TblIl? IlKodNavigation { get; set; }

    public virtual TblIlce? IlceKodNavigation { get; set; }

    public virtual TblKoy? KoyKodNavigation { get; set; }

    public virtual TblTanim MahalleTanimKodNavigation { get; set; } = null!;

    public virtual TblYetkiliIdare? YetkiliIdareKodNavigation { get; set; }
}
