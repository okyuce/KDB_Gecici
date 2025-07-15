using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Kds;

public class KdsBoyutTblYapiRepository : GenericRepositoryGeneralAsync<TblYapi, KdsDbContext>, IKdsBoyutTblYapiRepository
{
    public KdsBoyutTblYapiRepository(KdsDbContext dbContext) : base(dbContext)
    {
    }
}