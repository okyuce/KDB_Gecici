using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Kds;

public class KdsBoyutTblMahalleRepository : GenericRepositoryGeneralAsync<TblMahalle, KdsDbContext>, IKdsBoyutTblMahalleRepository
{
    public KdsBoyutTblMahalleRepository(KdsDbContext dbContext) : base(dbContext)
    {
    }
}