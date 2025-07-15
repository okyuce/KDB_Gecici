using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities.Kds;

public partial class TblIlce
{
    /// <summary>
    /// tablodaki anahtar alan, otomatik artan degerdedir.
    /// </summary>
    public int IlceId { get; set; }

    /// <summary>
    /// aks sistemindaki ilce degeri. #IlceKod
    /// </summary>
    public int IlceKod { get; set; }

    /// <summary>
    /// ilcenin ad bilgisini tutmaktadir. #IlceAd
    /// </summary>
    public string Ad { get; set; } = null!;

    /// <summary>
    /// il tablosundaki kod bilgisi degerini tutmaktadir #IlceKod
    /// </summary>
    public int IlKod { get; set; }

    /// <summary>
    /// tuik tarafinda ilcenin kod degerini tutmaktadir.
    /// </summary>
    public string? TuikKod { get; set; }

    /// <summary>
    /// tapu sisteminde o ilceye karsilik gelen id degeridir. #TapuIlceKod
    /// </summary>
    public int? TapuKod { get; set; }

    /// <summary>
    /// acikalma verisini tutmaktadir. #Aciklama
    /// </summary>
    public string? Aciklama { get; set; }

    /// <summary>
    /// verinin gecerli kullanilabilir veri olma durumu #Aktif
    /// </summary>
    public bool? Aktif { get; set; }

    /// <summary>
    /// ilce verisi ile ilgili son resmi gazate tarihi #ResmiTarih
    /// </summary>
    public DateOnly? ResmiGazeteTarihi { get; set; }

    /// <summary>
    /// guncel versiyon numarasi #Versiyon
    /// </summary>
    public long Versiyon { get; set; }

    /// <summary>
    /// İlçe merkezi ilçemi değil mi belirtmek için kullanılmakatdır.
    /// </summary>
    public bool? MerkezIlce { get; set; }

    public virtual TblIl IlKodNavigation { get; set; } = null!;

    public virtual ICollection<TblCsbm> TblCsbms { get; set; } = new List<TblCsbm>();

    public virtual ICollection<TblMahalle> TblMahalles { get; set; } = new List<TblMahalle>();

    public virtual ICollection<TblYetkiliIdare> TblYetkiliIdares { get; set; } = new List<TblYetkiliIdare>();
}
