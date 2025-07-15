namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruListeByTcNo;

public class GetirBasvuruListeByTcNoQueryModel
{
    public string? TcKimlikNo { get; set; }

    public string? TuzelKisiMersisNo { get; set; }

    public bool? TuzelMi { get; set; } = false;
}