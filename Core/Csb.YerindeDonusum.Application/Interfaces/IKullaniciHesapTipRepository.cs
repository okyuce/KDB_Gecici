using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IKullaniciHesapTipRepository : IGenericRepositoryAsync<KullaniciHesapTip>
{
    IQueryable<KullaniciHesapTip> GetAll();
}