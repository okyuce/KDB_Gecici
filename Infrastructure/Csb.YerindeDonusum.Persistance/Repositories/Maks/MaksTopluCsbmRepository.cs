using Csb.YerindeDonusum.Application.Interfaces.Maks;
using Csb.YerindeDonusum.Domain.Entities.Maks;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Maks;

public class MaksTopluCsbmRepository : GenericRepositoryGeneralAsync<TopluCsbm, MaksDbContext>, IMaksTopluCbsmRepository
{
    public MaksTopluCsbmRepository(MaksDbContext dbContext) : base(dbContext)
    {
    }
}