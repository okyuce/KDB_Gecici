using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IBasvuruIptalTurRepository : IGenericRepositoryAsync<BasvuruIptalTur>
{
    IQueryable<BasvuruIptalTur> GetAll();
}