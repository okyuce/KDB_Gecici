using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface ITakbisIlRepository : IGenericRepositoryAsync<TakbisIl>
{
    IQueryable<TakbisIl> GetAll();
}