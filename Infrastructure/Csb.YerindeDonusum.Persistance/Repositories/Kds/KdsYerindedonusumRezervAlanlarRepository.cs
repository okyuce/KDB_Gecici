using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Kds;

public class KdsYerindedonusumRezervAlanlarRepository : GenericRepositoryGeneralAsync<KdsYerindedonusumRezervAlanlar, KdsDbContext>, IKdsYerindedonusumRezervAlanlarRepository
{
    public KdsYerindedonusumRezervAlanlarRepository(KdsDbContext dbContext) : base(dbContext)
    {
    }
}