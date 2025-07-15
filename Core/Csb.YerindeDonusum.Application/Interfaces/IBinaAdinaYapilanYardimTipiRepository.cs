using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IBinaAdinaYapilanYardimTipiRepository : IGenericRepositoryAsync<BinaAdinaYapilanYardimTipi>
{
    IQueryable<BinaAdinaYapilanYardimTipi> GetAll();
}