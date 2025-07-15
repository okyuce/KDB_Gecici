using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities.Kds;

public partial class TblTanim
{
    /// <summary>
    /// tablodaki anahtar alan, otomatik artan degerdedir.
    /// </summary>
    public int TanimId { get; set; }

    /// <summary>
    /// Tanim grup kodu, bu tablo ile join olacak tablolar bu alan ile eşleşmelidir.
    /// </summary>
    public string TanimKod { get; set; } = null!;

    /// <summary>
    /// ad bilgisini tutmaktadir.
    /// </summary>
    public string Ad { get; set; } = null!;

    /// <summary>
    /// bagli oldugu tanim grup tablosundaki kaydin kod degeri.
    /// </summary>
    public string TanimGrupKod { get; set; } = null!;

    /// <summary>
    /// Kendi icersinde topoloji bilgisini tutmaktadir.
    /// </summary>
    public string? BagliTanimKod { get; set; }

    /// <summary>
    /// ek aciklama bilgileri burada tutulmaktadir.
    /// </summary>
    public string? Aciklama { get; set; }

    /// <summary>
    /// verinin gecerli kullanilabilir veri olma durumu
    /// </summary>
    public bool? Aktif { get; set; }

    /// <summary>
    /// Tabloya verinin eklendiği tarih
    /// </summary>
    public DateTime EklenmeTarihi { get; set; }

    /// <summary>
    /// İlgili satırın en son ne zaman update edildiği bilgisini tutmaktadır.
    /// </summary>
    public DateTime? GuncellemeTarihi { get; set; }

    /// <summary>
    /// guncel versiyon numarasi
    /// </summary>
    public long Versiyon { get; set; }

    /// <summary>
    /// il tablosundaki il kod alani eslesmketedir. Bazi tanimlarda il gerektigi icin eklendi
    /// </summary>
    public int? IlKod { get; set; }

    /// <summary>
    /// tbl_ilce ilce_kod alani ile eslemektedir. Bazi enumlarda ilce bilgisi oldugu icin eklendi
    /// </summary>
    public int? IlceKod { get; set; }

    /// <summary>
    /// Kaynak Sistemdeki Kodu Ornek  MernisKodu
    /// </summary>
    public string? KaynakTanimKod { get; set; }

    public virtual TblTanim? BagliTanimKodNavigation { get; set; }

    public virtual ICollection<TblTanim> InverseBagliTanimKodNavigation { get; set; } = new List<TblTanim>();

    public virtual ICollection<TblCsbm> TblCsbmGelismislikDurumKodNavigations { get; set; } = new List<TblCsbm>();

    public virtual ICollection<TblCsbm> TblCsbmTipKodNavigations { get; set; } = new List<TblCsbm>();

    public virtual ICollection<TblMahalle> TblMahalles { get; set; } = new List<TblMahalle>();

    public virtual ICollection<TblYapi> TblYapiYapiDurumTanimKodNavigations { get; set; } = new List<TblYapi>();

    public virtual ICollection<TblYapi> TblYapiYapiTipTanimKodNavigations { get; set; } = new List<TblYapi>();

    public virtual ICollection<TblYetkiliIdare> TblYetkiliIdares { get; set; } = new List<TblYetkiliIdare>();
}
