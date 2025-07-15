using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Kds;

public class KdsBoyutTblNumaratajRepository : GenericRepositoryGeneralAsync<TblNumarataj, KdsDbContext>, IKdsBoyutTblNumaratajRepository
{
    public KdsBoyutTblNumaratajRepository(KdsDbContext dbContext) : base(dbContext)
    {
    }
}