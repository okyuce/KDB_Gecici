using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Models.PagedListExtension;
public class PagedList<T>
{
    /// <summary>.
    /// Sayfa numarası
    /// </summary>
    public int SayfaNo { get; set; }

    /// <summary>
    /// Bir sayfadaki max. kayıt sayısı
    /// </summary>
    public int SayfaBoyutu { get; set; }

    /// <summary>
    /// Toplam kayıt sayısı.
    /// </summary>
    public int ToplamKayitSayisi { get; set; }

    /// <summary>
    /// Toplam sayfa sayısı.
    /// </summary>
    public int ToplamSayfaSayisi { get; set; }

    /// <summary>
    /// Şuan ki sayfanın kayıtları.
    /// </summary>
    public IList<T> VeriListesi { get; set; }

    /// <summary>
    /// Önceki bir sayfa olup olmadığı bilgisi.
    /// </summary>
    public bool OncekiSayfaVarmi => SayfaNo > 1;

    /// <summary>
    /// Sonraki bir sayfa olup olmadığı bilgisi.
    /// </summary>
    public bool SonrakiSayfaVarMi => SayfaNo < ToplamSayfaSayisi;

    /// <summary>
    /// <see cref="PagedList{T}" /> sınıfının yeni bir örneğini oluşturur.
    /// </summary>
    /// <param name="kaynak">Kaynak.</param>
    /// <param name="sayfaSayisi">Sayfa numarası.</param>
    /// <param name="sayfaBoyutu">Sayfa büyüklüğü.</param>
    public PagedList(IEnumerable<T> kaynak, int sayfaSayisi, int sayfaBoyutu)
    {
        if (sayfaSayisi <= 0)
        {
            throw new ArgumentException($"sayfaSayisi: {sayfaSayisi} <= 0 , sayfaSayisi > 0 olmalı.");
        }

        if (kaynak is IQueryable<T> querable)
        {
            SayfaNo = sayfaSayisi;
            SayfaBoyutu = sayfaBoyutu;
            ToplamKayitSayisi = querable.Count();
            ToplamSayfaSayisi = (int)Math.Ceiling(ToplamKayitSayisi / (double)SayfaBoyutu);

            VeriListesi = querable.Skip((SayfaNo - 1) * SayfaBoyutu).Take(SayfaBoyutu).ToList();
        }
        else
        {
            SayfaNo = sayfaSayisi;
            SayfaBoyutu = sayfaBoyutu;
            ToplamKayitSayisi = kaynak.Count();
            ToplamSayfaSayisi = (int)Math.Ceiling(ToplamKayitSayisi / (double)SayfaBoyutu);

            VeriListesi = kaynak.Skip((SayfaNo - 1) * SayfaBoyutu).Take(SayfaBoyutu).ToList();
        }
    }

    /// <summary>
    /// <see cref="PagedList{T}" /> sınıfının yeni bir örneğini oluşturur.
    /// </summary>
    public PagedList() => VeriListesi = new T[0];
}
