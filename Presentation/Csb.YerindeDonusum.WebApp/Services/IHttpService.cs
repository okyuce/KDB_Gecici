using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.WebApp.Models;

namespace Csb.YerindeDonusum.WebApp.Services;

public interface IHttpService
{
    public Task<AppResultModel<T>> PostAsync<T, K>(string endpoint, K data) where T : class;

    public Task<AppResultModel<T>> GetAsync<T>(string endpoint, object? request = null) where T : class;
}