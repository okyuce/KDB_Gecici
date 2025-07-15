using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Kds;

public class KdsBasvuruRepository : GenericRepositoryGeneralAsync<Basvuru, KdsDbContext>, IKdsBasvuruRepository
{
    public KdsBasvuruRepository(KdsDbContext dbContext) : base(dbContext)
    {
    }
}