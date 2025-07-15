using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Kds;

public class KdsBoyutTblIlRepository : GenericRepositoryGeneralAsync<TblIl, KdsDbContext>, IKdsBoyutTblIlRepository
{
    public KdsBoyutTblIlRepository(KdsDbContext dbContext) : base(dbContext)
    {
    }
}