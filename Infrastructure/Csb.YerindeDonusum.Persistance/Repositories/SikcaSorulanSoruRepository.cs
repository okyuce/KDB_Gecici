using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Persistance.Context;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Persistance.Repositories;

public class SikcaSorulanSoruRepository : GenericRepositoryAsync<SikcaSorulanSoru>, ISikcaSorulanSoruRepository
{
    public SikcaSorulanSoruRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
    {
    }

    public IQueryable<SikcaSorulanSoru> GetAll()
        => GetWhere(x => x.AktifMi == true && x.SilindiMi == false, true).OrderBy(o => o.SiraNo);
}