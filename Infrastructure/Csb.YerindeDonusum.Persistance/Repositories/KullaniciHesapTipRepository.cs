using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Persistance.Context;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Persistance.Repositories;

public class KullaniciHesapTipRepository : GenericRepositoryAsync<KullaniciHesapTip>, IKullaniciHesapTipRepository
{
    public KullaniciHesapTipRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
    {
    }

    public IQueryable<KullaniciHesapTip> GetAll()
        => GetAllQueryable();
}