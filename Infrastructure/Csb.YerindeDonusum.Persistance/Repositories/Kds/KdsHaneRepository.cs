using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Kds;

public class KdsHaneRepository : GenericRepositoryGeneralAsync<Hane, KdsDbContext>, IKdsHaneRepository
{
    public KdsHaneRepository(KdsDbContext dbContext) : base(dbContext)
    {
    }
}