namespace Csb.YerindeDonusum.Application.CQRS.BasvuruDosyaCQRS.Queries;

public class GetirDosyaByIdQueryModel
{
    public string Id { get; set; }
    public int MaxWitdh { get; set; }
    public int MaxQuality { get; set; }
}