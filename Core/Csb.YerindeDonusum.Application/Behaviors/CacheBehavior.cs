using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Resolver;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.Behaviors;

public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheMediatrQuery
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ICacheService _cache;
    private readonly IKullaniciBilgi _kullaniciBilgi;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly CacheOptionModel _cacheOptions;

    public CacheBehavior(IHttpContextAccessor contextAccessor, ICacheService cache, IKullaniciBilgi kullaniciBilgi, IServiceProvider serviceProvider, IWebHostEnvironment webHostEnvironment)
    {
        _contextAccessor = contextAccessor;
        _cache = cache;
        _kullaniciBilgi = kullaniciBilgi;
        _webHostEnvironment = webHostEnvironment;
        _cacheOptions = serviceProvider.GetRequiredService<IOptionsMonitor<CacheOptionModel>>().CurrentValue;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!request.CacheIsActive || !_cacheOptions.IsActive)
            return await next();

        var cacheKey = GetCacheKey(request);

        async Task<TResponse> GetResponseAndAddToCache()
        {
            var response = await next();

            try
            {
                var result = response as dynamic;
                if (result?.IsError == false && result?.Result != null)
                {
                    if (request.CacheMinute != null)
                        await _cache.SetValueAsync(cacheKey, JsonConvert.SerializeObject(response), TimeSpan.FromMinutes(request.CacheMinute.Value));
                    else
                        await _cache.SetValueAsync(cacheKey, JsonConvert.SerializeObject(response));
                }
            }
            catch { }

            return response;
        }

        var cacheData = await _cache.GetValueAsync(cacheKey);
        if (cacheData != null)
        {
            AddCacheControlHeader(request);

            try
            {
                /*
                    serverside datatable için draw setlemesi yapılıyor
                    yoksa frontendde sayfa geçişleri karışıyor
                */

                var draw = request.GetType()?.GetProperty("draw")?.GetValue(request, null);
                if (draw != null)
                {
                    dynamic cachedObj = JsonConvert.DeserializeObject(cacheData);

                    if (cachedObj?.Result?.draw != null)
                    {
                        cachedObj.Result.draw = draw;
                        cacheData = JsonConvert.SerializeObject(cachedObj);
                    }
                }
            }
            catch { }

            return JsonConvert.DeserializeObject<TResponse>(cacheData);
        }
        else
        {
            return await GetResponseAndAddToCache();
        }
    }

    private void AddCacheControlHeader(TRequest request)
    {
        _contextAccessor?.HttpContext?.Response?.Headers?.Add("Cache-Control", string.Format("max-age={0}", (TimeSpan.FromMinutes(request.CacheMinute != null ? request.CacheMinute.Value : _cacheOptions.ExpireMinute)).TotalSeconds));
    }

    private string GetCacheKey(TRequest request)
    {
        string className = typeof(TRequest).FullName!;

        if (request.CacheCustomUser == true)
        {
            var kullanici = _kullaniciBilgi.GetUserInfo();

            className += $"_{kullanici.KullaniciId}";
        }

        className += $"_{_webHostEnvironment.EnvironmentName}";

        var requestData = JsonConvert.SerializeObject(request, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new IgnorePropertiesResolver(new[] { "draw", "columns", "cacheMinute", "cacheIsActive", "cacheCustomUser", "cacheRemovePattern" }),
            MaxDepth = 5
        });

        if (!FluentValidationExtension.JsonNotEmpty(requestData))
            return className;

        return $"{className}({requestData})";
    }
}