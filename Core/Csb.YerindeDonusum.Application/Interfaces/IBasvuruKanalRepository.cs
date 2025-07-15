using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IBasvuruKanalRepository : IGenericRepositoryAsync<BasvuruKanal>
{
    IQueryable<BasvuruKanal> GetAll();
}