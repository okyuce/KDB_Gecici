using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Persistance.Context;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Persistance.Repositories;

public class BasvuruIptalTurRepository : GenericRepositoryAsync<BasvuruIptalTur>, IBasvuruIptalTurRepository
{
    public BasvuruIptalTurRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
    {
    }

    public IQueryable<BasvuruIptalTur> GetAll()
        => GetWhere(x => x.AktifMi == true && x.SilindiMi == false, true);
}