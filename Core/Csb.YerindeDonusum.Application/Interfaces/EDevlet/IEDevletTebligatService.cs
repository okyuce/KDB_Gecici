using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Commands.TebligatGonder;
using Csb.YerindeDonusum.Application.Models;

namespace Csb.YerindeDonusum.Application.Interfaces.EDevlet;
public interface IEDevletTebligatService
{
    public Task<EDevletTabligatResult> TebligatGonder(TebligatGonderCommand model);
}
