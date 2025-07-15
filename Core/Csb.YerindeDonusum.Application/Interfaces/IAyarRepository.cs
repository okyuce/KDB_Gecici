using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IAyarRepository : IGenericRepositoryAsync<Ayar>
{
    IQueryable<Ayar> GetAll();
}