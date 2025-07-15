using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IBirimRepository : IGenericRepositoryAsync<Birim>
{
    IQueryable<Birim> GetAll();
}