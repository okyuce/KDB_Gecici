using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Csb.YerindeDonusum.Application.Behaviors;

public class CacheRemoveBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheRemoveMediatrQuery
{
    private readonly ICacheService _cache;
    private readonly CacheOptionModel _cacheOptions;

    public CacheRemoveBehavior(ICacheService cache, IServiceProvider serviceProvider)
    {
        _cache = cache;
        _cacheOptions = serviceProvider.GetRequiredService<IOptionsMonitor<CacheOptionModel>>().CurrentValue;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if (_cacheOptions.IsActive && !string.IsNullOrWhiteSpace(request.CacheRemovePattern))
        {
            var result = response as dynamic;
            if (result?.IsError == false)
                await _cache.ClearByPattern(request.CacheRemovePattern);
        }

        return response;
    }
}