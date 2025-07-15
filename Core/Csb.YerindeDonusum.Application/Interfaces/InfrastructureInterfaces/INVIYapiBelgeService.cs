using Csb.YerindeDonusum.Application.Models.NVIYapiBelge.YapiRuhsatOku;

namespace Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
public interface INVIYapiBelgeSorguService
{
    Task<YapiRuhsatOkuResult> YapiRuhsatOku(long bultenNo);
}

