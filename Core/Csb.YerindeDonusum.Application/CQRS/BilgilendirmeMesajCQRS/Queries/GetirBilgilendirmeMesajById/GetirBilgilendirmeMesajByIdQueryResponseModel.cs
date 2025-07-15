namespace Csb.YerindeDonusum.Application.CQRS.BilgilendirmeMesajCQRS.Queries.GetirBilgilendirmeMesajById;

public class GetirBilgilendirmeMesajByIdQueryResponseModel
{
    public int Id { get; set; }

    public string Anahtar { get; set; } = null!;

    public string Deger { get; set; } = null!;
}
