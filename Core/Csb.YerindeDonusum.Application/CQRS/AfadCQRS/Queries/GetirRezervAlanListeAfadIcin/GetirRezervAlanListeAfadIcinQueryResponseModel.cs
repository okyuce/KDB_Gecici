namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirRezervAlanListeAfadIcin;

public class GetirRezervAlanListeAfadIcinQueryResponseModel
{
    public long ToplamRezervAlanSayisi { get; set; }
    public List<GetirRezervAlanListeAfadIcinQueryResponseModelDetay>? RezervAlanListe { get; set; }
}