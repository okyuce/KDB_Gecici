using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Persistance.Context;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Persistance.Repositories;

public class BasvuruImzaVerenDosyaRepository : GenericRepositoryAsync<BasvuruImzaVerenDosya>, IBasvuruImzaVerenDosyaRepository
{
    public BasvuruImzaVerenDosyaRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
    {
    }
}