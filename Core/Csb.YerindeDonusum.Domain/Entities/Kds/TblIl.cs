using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities.Kds;

/// <summary>
/// Aks sistemindeki il kayıtlarının tutulduğu tablodur. #Il
/// </summary>
public partial class TblIl
{
    /// <summary>
    /// tablodaki anahtar alan, otomatik artan degerdedir.
    /// </summary>
    public int IlId { get; set; }

    /// <summary>
    /// aks sistemindaki il degeri = plaka kodu, tabloda essizdir. #IlKod
    /// </summary>
    public int IlKod { get; set; }

    /// <summary>
    /// ilin ad bilgisini tutmaktadir. #IlAd
    /// </summary>
    public string Ad { get; set; } = null!;

    /// <summary>
    /// tuik tarafinda ilin kod degerini tutmaktadir.
    /// </summary>
    public string? TuikKod { get; set; }

    /// <summary>
    /// tapu sisteminde o ile ait karsilik gelen id degeridir. #TapuIlKod
    /// </summary>
    public int TapuKod { get; set; }

    /// <summary>
    /// acikalma verisini tutmaktadir. #Aciklama
    /// </summary>
    public string? Aciklama { get; set; }

    /// <summary>
    /// verinin gecerli kullanilabilir veri olma durumu #Aktif
    /// </summary>
    public bool? Aktif { get; set; }

    /// <summary>
    /// il verisi ile ilgili son resmi gazate tarihi #ResmiTarih
    /// </summary>
    public DateOnly? ResmiGazeteTarihi { get; set; }

    /// <summary>
    /// guncel versiyon numarasi #Versiyon
    /// </summary>
    public long Versiyon { get; set; }

    public bool Buyuksehir { get; set; }

    public virtual ICollection<TblCsbm> TblCsbms { get; set; } = new List<TblCsbm>();

    public virtual ICollection<TblIlce> TblIlces { get; set; } = new List<TblIlce>();

    public virtual ICollection<TblMahalle> TblMahalles { get; set; } = new List<TblMahalle>();

    public virtual ICollection<TblYetkiliIdare> TblYetkiliIdares { get; set; } = new List<TblYetkiliIdare>();
}
