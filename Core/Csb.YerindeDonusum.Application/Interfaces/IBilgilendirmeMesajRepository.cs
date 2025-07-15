using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IBilgilendirmeMesajRepository : IGenericRepositoryAsync<BilgilendirmeMesaj>
{
    IQueryable<BilgilendirmeMesaj> GetAll();
}