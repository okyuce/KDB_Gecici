using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class Kullanici
{
    public long KullaniciId { get; set; }

    public Guid KullaniciGuid { get; set; }

    public string KullaniciAdi { get; set; } = null!;

    public string? Sifre { get; set; }

    public long KullaniciHesapTipId { get; set; }

    public long? BirimId { get; set; }

    public string? SonGirisYapilanIp { get; set; }

    public DateTime? SonGirisYapilanTarih { get; set; }

    /// <summary>
    /// webservis kullanıcısı ise true olacak, bu kullanıcılar admin panelinden görünmez
    /// </summary>
    public bool? SistemKullanicisiMi { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public long OlusturanKullaniciId { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public long? TcKimlikNo { get; set; }

    public string? Eposta { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? SonSifreDegisimTarihi { get; set; }

    public string? CepTelefonu { get; set; }

    public virtual ICollection<BinaOdemeDetay> BinaOdemeDetayGuncelleyenKullanicis { get; set; } = new List<BinaOdemeDetay>();

    public virtual ICollection<BinaOdemeDetay> BinaOdemeDetayOlusturanKullanicis { get; set; } = new List<BinaOdemeDetay>();

    public virtual ICollection<BinaOdeme> BinaOdemeGuncelleyenKullanicis { get; set; } = new List<BinaOdeme>();

    public virtual ICollection<BinaOdeme> BinaOdemeOlusturanKullanicis { get; set; } = new List<BinaOdeme>();

    public virtual Birim? Birim { get; set; }

    public virtual ICollection<KullaniciGirisBasarili> KullaniciGirisBasarilis { get; set; } = new List<KullaniciGirisBasarili>();

    public virtual ICollection<KullaniciGirisHatum> KullaniciGirisHata { get; set; } = new List<KullaniciGirisHatum>();

    public virtual ICollection<KullaniciGirisKodDeneme> KullaniciGirisKodDenemes { get; set; } = new List<KullaniciGirisKodDeneme>();

    public virtual ICollection<KullaniciGirisKod> KullaniciGirisKods { get; set; } = new List<KullaniciGirisKod>();

    public virtual KullaniciHesapTip KullaniciHesapTip { get; set; } = null!;

    public virtual ICollection<KullaniciRol> KullaniciRols { get; set; } = new List<KullaniciRol>();

    public virtual ICollection<TebligatGonderim> TebligatGonderims { get; set; } = new List<TebligatGonderim>();
}
