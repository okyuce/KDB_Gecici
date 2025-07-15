namespace Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirListeAfadBasvuruServerSide;

public class GetirListeAfadBasvuruServerSideQueryResponseModel
{
    public Guid CsbId { get; set; }

    public string? Tckn { get; set; }

    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public string? Telefon { get; set; }

    public string? AskiKodu { get; set; }

    public string? Il { get; set; }

    public string? Ilce { get; set; }

    public string? Mahalle { get; set; }

    public string? Ada { get; set; }

    public string? Parsel { get; set; }

    public string? ItirazDegerlendirmeSonucu { get; set; }

    public string? DegerlendirmeDurumu { get; set; }

    public bool? KuraIsabetEttiMi { get; set; }

    public string? Huid { get; set; }
}