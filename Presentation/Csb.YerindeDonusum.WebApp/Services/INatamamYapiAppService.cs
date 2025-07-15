using Csb.YerindeDonusum.WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.WebApp.Services
{
    public interface INatamamYapiAppService
    {
        Task<IEnumerable<NatamamYapiIslemViewModel>> GetAllAsync();
    }
}
