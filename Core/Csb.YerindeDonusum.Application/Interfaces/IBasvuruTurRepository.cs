using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IBasvuruTurRepository : IGenericRepositoryAsync<BasvuruTur>
{
    IQueryable<BasvuruTur> GetAll();
}