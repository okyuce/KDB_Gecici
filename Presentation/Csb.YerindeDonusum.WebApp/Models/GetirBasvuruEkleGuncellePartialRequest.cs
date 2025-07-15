using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruDetayById;

namespace Csb.YerindeDonusum.WebApp.Models;

public class GetirBasvuruEkleGuncellePartialRequest : GetirBasvuruDetayByIdQueryModel
{
    public int? Tip { get; set; }
}
