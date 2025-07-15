using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IAydinlatmaMetniRepository : IGenericRepositoryAsync<AydinlatmaMetni>
{
    //Task<ClarificationText> GetById(Guid id);

    IQueryable<AydinlatmaMetni> GetAll();
}