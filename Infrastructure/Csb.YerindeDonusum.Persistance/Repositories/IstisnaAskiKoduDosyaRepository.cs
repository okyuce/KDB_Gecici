using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Persistence.Repositories
{
    public class IstisnaAskiKoduDosyaRepository : IIstisnaAskiKoduDosyaRepository
    {
        private readonly ApplicationDbContext _context;
        public IstisnaAskiKoduDosyaRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<IstisnaAskiKoduDosya>> GetByKoduIdAsync(long koduId) =>
            await _context.IstisnaAskiKoduDosyalar
                          .Where(d => d.IstisnaAskiKoduId == koduId && !d.SilindiMi)
                          .AsNoTracking()
                          .ToListAsync();

        public async Task AddAsync(IstisnaAskiKoduDosya entity)
        {
            await _context.IstisnaAskiKoduDosyalar.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IstisnaAskiKoduDosya entity)
        {
            _context.IstisnaAskiKoduDosyalar.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(long id)
        {
            var entity = await _context.IstisnaAskiKoduDosyalar.FindAsync(id);
            if (entity != null)
            {
                entity.SilindiMi = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}