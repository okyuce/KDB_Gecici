using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IBasvuruDestekTurRepository : IGenericRepositoryAsync<BasvuruDestekTur>
{
    IQueryable<BasvuruDestekTur> GetAll();
}