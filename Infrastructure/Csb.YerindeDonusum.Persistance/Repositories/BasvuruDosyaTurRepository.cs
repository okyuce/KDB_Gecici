using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Persistance.Context;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Persistance.Repositories;

public class BasvuruDosyaTurRepository : GenericRepositoryAsync<BasvuruDosyaTur>, IBasvuruDosyaTurRepository
{
    public BasvuruDosyaTurRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
    {
    }

    public IQueryable<BasvuruDosyaTur> GetAll()
        => GetWhere(x => x.AktifMi == true && x.SilindiMi == false, true);
}
