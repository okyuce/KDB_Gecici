using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IOfisKonumRepository : IGenericRepositoryAsync<OfisKonum>
{
    IQueryable<OfisKonum> GetAll();
}