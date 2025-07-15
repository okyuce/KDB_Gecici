using System.Linq.Expressions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Common;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Persistance.Context;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Persistance.Repositories;

public class AydinlatmaMetniRepository : GenericRepositoryAsync<AydinlatmaMetni>, IAydinlatmaMetniRepository
{
    public AydinlatmaMetniRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
    {
    }

    //public async Task<ClarificationText> GetById(Guid id) 
    //    =>  await GetByIdAsync(id);

    public IQueryable<AydinlatmaMetni> GetAll()
        => GetAllQueryable(x => x.AktifMi == true && x.SilindiMi == false);
}