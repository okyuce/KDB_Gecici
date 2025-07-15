using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Common;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Persistance.Context;
using Csb.YerindeDonusum.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CSB.Core.Integration.KPS; // or the correct namespace for GenericRepositoryAsync
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Persistence.Repositories
{
    public class IstisnaAskiKoduRepository : IIstisnaAskiKoduRepository
    {
        private readonly ApplicationDbContext _context;
        public IstisnaAskiKoduRepository(ApplicationDbContext context) => _context = context;

        public IQueryable<IstisnaAskiKodu> GetAll() =>
            _context.IstisnaAskiKodular.AsNoTracking();

        public async Task<IEnumerable<IstisnaAskiKodu>> GetAllAsync() =>
            await _context.IstisnaAskiKodular.AsNoTracking().ToListAsync();

        public async Task<IstisnaAskiKodu> GetByIdAsync(long id) =>
            await _context.IstisnaAskiKodular.FindAsync(id);

        public async Task AddAsync(IstisnaAskiKodu entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _context.IstisnaAskiKodular.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IstisnaAskiKodu entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _context.IstisnaAskiKodular.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(long id)
        {
            var entity = await _context.IstisnaAskiKodular.FindAsync(id);
            if (entity != null)
            {
                entity.SilindiMi = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}