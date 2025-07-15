using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Csb.YerindeDonusum.NVIYapiBelge.ServiceRegistration;

public static class ServiceRegistration
{
    public static IServiceCollection AddNVIYapiBelgeServiceRegistration(this IServiceCollection serviceProviders, IConfiguration configuration)
    {
        serviceProviders.AddScoped<INVIYapiBelgeSorguService, NVIYapiBelgeSorguService>();

        return serviceProviders;
    }
}
