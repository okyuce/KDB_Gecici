using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Dtos;

public class KullaniciDto
{
	public long KullaniciId { get; set; }

	public string KullaniciAdi { get; set; } = null!;

	public long KullaniciHesapTipId { get; set; }

	public long? BirimId { get; set; }

	public bool? AktifMi { get; set; }

	public long? TcKimlikNo { get; set; }

	public string? Eposta { get; set; }
    public string? Ad { get; set; }

    public string? Soyad { get; set; }
    public DateTime? SonSifreDegisimTarihi { get; set; }
    public bool? SifreZamanAsimiMi { get; set; }

    public long[] SecilenRolIdList { get; set; }


}