namespace Csb.YerindeDonusum.Application.Dtos;

public class TebligatGonderimDetayDosyaDto
{
    public long TebligatGonderimDetayDosyaId { get; set; }

    public long? TebligatGonderimDetayId { get; set; }

    public Guid TebligatGonderimDetayDosyaGuid { get; set; }

    public string DosyaAdi { get; set; } = null!;

    public string? DosyaYolu { get; set; }

    public string DosyaTuru { get; set; } = null!;

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }
}
