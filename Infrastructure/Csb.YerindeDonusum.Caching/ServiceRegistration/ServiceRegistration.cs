using Csb.YerindeDonusum.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Csb.YerindeDonusum.Caching.ServiceRegistration;

public static class ServiceRegistration
{
    public static IServiceCollection AddCachingServiceRegistration(this IServiceCollection serviceProviders, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis");
        var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString!);
        serviceProviders.AddSingleton<IConnectionMultiplexer>(multiplexer);
        serviceProviders.AddSingleton<ICacheService, RedisCacheService>();

        return serviceProviders;
    }
}