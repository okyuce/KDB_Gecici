using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Persistance.Context;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Persistance.Repositories;

public class KullaniciRolRepository : GenericRepositoryAsync<KullaniciRol>, IKullaniciRolRepository
{
    public KullaniciRolRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
    {
    }

    public IQueryable<KullaniciRol> GetAll()
        => GetAllQueryable();
}