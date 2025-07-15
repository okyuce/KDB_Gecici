using Csb.YerindeDonusum.Application.Interfaces.EDevlet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Csb.YerindeDonusum.EDevlet.ServiceRegistration;
public static class ServiceRegistration
{
    public static IServiceCollection AddEDevletServiceRegistration(this IServiceCollection serviceProviders, IConfiguration configuration)
    {
        serviceProviders.AddScoped<IEDevletTebligatService, EDevletTebligatService>();
        return serviceProviders;
    }
}
