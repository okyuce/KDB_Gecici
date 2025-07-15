using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface ISikcaSorulanSoruRepository : IGenericRepositoryAsync<SikcaSorulanSoru>
{
    IQueryable<SikcaSorulanSoru> GetAll();
}