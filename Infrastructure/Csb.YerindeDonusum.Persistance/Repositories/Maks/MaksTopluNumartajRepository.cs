using Csb.YerindeDonusum.Application.Interfaces.Maks;
using Csb.YerindeDonusum.Domain.Entities.Maks;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Maks;

public class MaksTopluNumartajRepository : GenericRepositoryGeneralAsync<TopluNumarataj, MaksDbContext>, IMaksTopluNumartajRepository
{
    public MaksTopluNumartajRepository(MaksDbContext dbContext) : base(dbContext)
    {
    }
}