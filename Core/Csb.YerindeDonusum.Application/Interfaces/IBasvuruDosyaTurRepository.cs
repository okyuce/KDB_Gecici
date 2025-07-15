using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IBasvuruDosyaTurRepository : IGenericRepositoryAsync<BasvuruDosyaTur>
{
    IQueryable<BasvuruDosyaTur> GetAll();
}