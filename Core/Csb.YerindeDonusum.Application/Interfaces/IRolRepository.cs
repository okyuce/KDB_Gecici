using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IRolRepository : IGenericRepositoryAsync<Rol>
{
    IQueryable<Rol> GetAll();
}