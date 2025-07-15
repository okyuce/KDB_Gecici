
namespace Csb.YerindeDonusum.Application.Dtos;

public class KullaniciListeDto
{
    public long KullaniciId { get; set; }

    public string KullaniciAdi { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public string? TcKimlikNo { get; set; }

    public string? Eposta { get; set; }
    public string? CepTelefonu { get; set; }
    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public string AdSoyad => Ad + " " + Soyad;
    public List<string>? KullaniciRolAdListe { get; set; }

    public string? BirimAdi { get; set; }
    public string? BirimId { get; set; }

    public string KullaniciHesapTipAdi { get; set; }
}