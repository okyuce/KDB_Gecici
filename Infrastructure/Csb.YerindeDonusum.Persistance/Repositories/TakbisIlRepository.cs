using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Persistance.Context;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Persistance.Repositories;

public class TakbisIlRepository : GenericRepositoryAsync<TakbisIl>, ITakbisIlRepository
{
    public TakbisIlRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
    {
    }

    public IQueryable<TakbisIl> GetAll()
        => GetWhere(x => x.Aktif == true, true);
}