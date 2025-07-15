namespace Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Queries;

public class GetirOfisKonumDetayResponseModel
{
    public long? OfisKonumId { get; set; }

    public string? IlAdi { get; set; }

    public string? IlceAdi { get; set; }

    public string? Adres { get; set; }

    public string? HaritaUrl { get; set; }

    public bool? AktifMi { get; set; }

    public double Enlem { get; set; }

    public double Boylam { get; set; }
}