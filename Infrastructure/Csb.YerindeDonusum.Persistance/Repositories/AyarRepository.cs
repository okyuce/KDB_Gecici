using System.Linq.Expressions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Common;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Persistance.Context;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Persistance.Repositories;

public class AyarRepository : GenericRepositoryAsync<Ayar>, IAyarRepository
{
    public AyarRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
    {
    }

    public IQueryable<Ayar> GetAll()
        => GetAllQueryable(x => x.AktifMi == true && x.SilindiMi == false);
}