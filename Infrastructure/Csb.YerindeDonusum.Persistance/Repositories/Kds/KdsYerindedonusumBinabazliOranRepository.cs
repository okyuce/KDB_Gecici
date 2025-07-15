using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Kds;

public class KdsYerindedonusumBinabazliOranRepository : GenericRepositoryGeneralAsync<KdsYerindedonusumBinabazliOran, KdsDbContext>, IKdsYerindedonusumBinabazliOranRepository
{
    public KdsYerindedonusumBinabazliOranRepository(KdsDbContext dbContext) : base(dbContext)
    {
    }
}