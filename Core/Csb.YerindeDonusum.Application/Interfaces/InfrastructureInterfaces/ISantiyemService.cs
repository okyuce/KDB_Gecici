
using Csb.YerindeDonusum.Application.Models.Santiyem;

namespace Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
public interface ISantiyemService
{
    Task<YetkiBelgesiNoSorgulamaResult> YetkiBelgesiNoSorgula(string yetkiBelgeNo);
}

