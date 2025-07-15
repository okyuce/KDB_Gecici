using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Kds;

public class KdsBoyutTblCsbmRepository : GenericRepositoryGeneralAsync<TblCsbm, KdsDbContext>, IKdsBoyutTblCsbmRepository
{
    public KdsBoyutTblCsbmRepository(KdsDbContext dbContext) : base(dbContext)
    {
    }
}