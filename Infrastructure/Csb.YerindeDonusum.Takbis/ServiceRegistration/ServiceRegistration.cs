using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using CSB.Core.Integration;
using CSB.Core.LogHandler;
using CSB.Core.LogHandler.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Csb.YerindeDonusum.Takbis.ServiceRegistration;

public static class ServiceRegistration
{
    public static IServiceCollection AddTakbisServiceRegistration(this IServiceCollection serviceProviders, IConfiguration configuration)
    {
        serviceProviders.AddScoped<ITakbisService, TakbisService>();
        return serviceProviders;
    }
}
