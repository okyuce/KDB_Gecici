namespace Csb.YerindeDonusum.Application.Dtos;

public class SikcaSorulanSoruDto
{
    public string Soru { get; set; } = null!;
	public string Cevap { get; set; } = null!;

	public long SikcaSorulanSoruId { get; set; }

	public int? SiraNo { get; set; }

	public bool? AktifMi { get; set; }

	public bool SilindiMi { get; set; }
}