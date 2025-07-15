namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirBasvuruListeDegisenAfadIcin;

public class GetirBasvuruListeDegisenAfadIcinResponseModel
{
    public long ToplamBasvuruSayisi { get; set; }
    public List<GetirAfadIcinBasvuruDetayDto>? BasvuruListe { get; set; }
}
