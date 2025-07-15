namespace Csb.YerindeDonusum.Application.Dtos;

public class OfisKonumDto
{
    public string IlAdi { get; set; } = null!;

    public string IlceAdi { get; set; } = null!;

    public string Adres { get; set; } = null!;

    public string? HaritaUrl { get; set; }

    public double Enlem { get; set; }

    public double Boylam { get; set; }
}