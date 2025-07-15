using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.Kds;

public class KdsHasartespitTespitVeriRepository : GenericRepositoryGeneralAsync<HasartespitTespitVeri, KdsDbContext>, IKdsHasartespitTespitVeriRepository
{
    public KdsHasartespitTespitVeriRepository(KdsDbContext dbContext) : base(dbContext)
    {
    }
}