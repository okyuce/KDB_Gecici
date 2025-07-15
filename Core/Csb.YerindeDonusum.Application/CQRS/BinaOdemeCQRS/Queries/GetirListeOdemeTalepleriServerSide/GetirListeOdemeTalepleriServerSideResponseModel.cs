using Csb.YerindeDonusum.Application.Dtos;

namespace Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeBinaOdemeServerSide;

public class GetirListeOdemeTalepleriServerSideResponseModel : BinaOdemeDto
{
    public List<BinaOdemeDto>? BinaOdemeList { get; set; }
}