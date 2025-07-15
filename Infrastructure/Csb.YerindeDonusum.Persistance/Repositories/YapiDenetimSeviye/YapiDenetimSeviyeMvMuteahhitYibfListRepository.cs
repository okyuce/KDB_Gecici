using Csb.YerindeDonusum.Application.Interfaces.YapiDenetimSeviye;
using Csb.YerindeDonusum.Domain.Entities.YapiDenetimSeviye;
using Csb.YerindeDonusum.Persistance.Context;

namespace Csb.YerindeDonusum.Persistance.Repositories.YapiDenetimSeviye;

internal class YapiDenetimSeviyeMvMuteahhitYibfListRepository : GenericRepositoryGeneralAsync<MvMuteahhitYibfList, YapiDenetimSeviyeDbContext>, IYapiDenetimSeviyeMvMuteahhitYibfListRepository
{
    public YapiDenetimSeviyeMvMuteahhitYibfListRepository(YapiDenetimSeviyeDbContext dbContext) : base(dbContext)
    {
    }
}