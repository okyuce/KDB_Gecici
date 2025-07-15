namespace Csb.YerindeDonusum.Application.Dtos;

public class SikcaSorulanSoruServerSideDto
{
    public long SikcaSorulanSoruId { get; set; }
    public string Soru { get; set; } = null!;
	public string Cevap { get; set; } = null!;
	public int? SiraNo { get; set; }
	public bool? AktifMi { get; set; }
}