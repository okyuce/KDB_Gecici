using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Kds;

public class KdsBoyutTblBagimsizBolumRepository : GenericRepositoryGeneralAsync<TblBagimsizBolum, KdsDbContext>, IKdsBoyutTblBagimsizBolumRepository
{
    public KdsBoyutTblBagimsizBolumRepository(KdsDbContext dbContext) : base(dbContext)
    {
    }
}