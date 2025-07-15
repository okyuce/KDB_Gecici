using Csb.YerindeDonusum.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Application.Interfaces
{
    public interface IIstisnaAskiKoduRepository
    {
        IQueryable<IstisnaAskiKodu> GetAll();
        Task<IEnumerable<IstisnaAskiKodu>> GetAllAsync();
        Task<IstisnaAskiKodu> GetByIdAsync(long id);
        Task AddAsync(IstisnaAskiKodu entity);
        Task UpdateAsync(IstisnaAskiKodu entity);
        Task SoftDeleteAsync(long id);
    }
}