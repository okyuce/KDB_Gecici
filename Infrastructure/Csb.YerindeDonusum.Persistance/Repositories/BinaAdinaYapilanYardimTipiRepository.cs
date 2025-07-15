using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Persistance.Context;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Persistance.Repositories;

public class BinaAdinaYapilanYardimTipiRepository : GenericRepositoryAsync<BinaAdinaYapilanYardimTipi>, IBinaAdinaYapilanYardimTipiRepository
{
    public BinaAdinaYapilanYardimTipiRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
    {
    }

    public IQueryable<BinaAdinaYapilanYardimTipi> GetAll()
        => GetWhere(x => x.AktifMi == true && x.SilindiMi == false, true);
}