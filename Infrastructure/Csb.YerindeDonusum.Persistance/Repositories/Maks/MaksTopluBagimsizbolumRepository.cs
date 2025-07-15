using Csb.YerindeDonusum.Application.Interfaces.Maks;
using Csb.YerindeDonusum.Domain.Entities.Maks;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Maks;

public class MaksTopluBagimsizbolumRepository : GenericRepositoryGeneralAsync<TopluBagimsizbolum, MaksDbContext>, IMaksTopluBagimsizbolumRepository
{
    public MaksTopluBagimsizbolumRepository(MaksDbContext dbContext) : base(dbContext)
    {
    }
}