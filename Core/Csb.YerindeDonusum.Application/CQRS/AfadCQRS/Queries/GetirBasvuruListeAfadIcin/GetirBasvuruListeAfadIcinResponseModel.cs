namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirBasvuruListeAfadIcin;

public class GetirBasvuruListeAfadIcinResponseModel
{
    public long ToplamBasvuruSayisi { get; set; }
    public List<GetirAfadIcinBasvuruDetayDto>? BasvuruListe { get; set; }
}
