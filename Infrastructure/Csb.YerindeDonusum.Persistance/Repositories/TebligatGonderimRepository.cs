using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Persistance.Context;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Persistance.Repositories
{
    public class TebligatGonderimRepository : GenericRepositoryAsync<TebligatGonderim>, ITebligatGonderimRepository
    {
        public TebligatGonderimRepository(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
        }
    }
}
