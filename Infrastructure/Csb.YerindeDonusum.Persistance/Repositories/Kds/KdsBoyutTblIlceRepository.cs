using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Kds;

public class KdsBoyutTblIlceRepository : GenericRepositoryGeneralAsync<TblIlce, KdsDbContext>, IKdsBoyutTblIlceRepository
{
    public KdsBoyutTblIlceRepository(KdsDbContext dbContext) : base(dbContext)
    {
    }
}