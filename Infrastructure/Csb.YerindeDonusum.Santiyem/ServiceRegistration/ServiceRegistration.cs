using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Csb.YerindeDonusum.Santiyem.ServiceRegistration;

public static class ServiceRegistration
{
    public static IServiceCollection AddSantiyemServiceRegistration(this IServiceCollection serviceProviders, IConfiguration configuration)
    {
        serviceProviders.AddScoped<ISantiyemService, SantiyemService>();

        return serviceProviders;
    }
}
