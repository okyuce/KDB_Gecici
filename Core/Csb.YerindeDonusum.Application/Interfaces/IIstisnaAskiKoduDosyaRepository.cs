using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces
{
    public interface IIstisnaAskiKoduDosyaRepository
    {
        Task<IEnumerable<IstisnaAskiKoduDosya>> GetByKoduIdAsync(long koduId);
        Task AddAsync(IstisnaAskiKoduDosya entity);
        Task UpdateAsync(IstisnaAskiKoduDosya entity);
        Task SoftDeleteAsync(long id);
    }
}
