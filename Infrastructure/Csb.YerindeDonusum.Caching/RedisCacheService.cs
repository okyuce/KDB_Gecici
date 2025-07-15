using Csb.YerindeDonusum.Application.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Csb.YerindeDonusum.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redisCon;
    private readonly IDatabase _cache;
    private TimeSpan ExpireTime => TimeSpan.FromDays(1);

    public RedisCacheService(IConnectionMultiplexer redisCon)
    {
        _redisCon = redisCon;
        _cache = redisCon.GetDatabase();
    }

    public async Task Clear(string key)
    {
        await _cache.KeyDeleteAsync(key);
    }

    public async Task<bool> ClearByPattern(string pattern)
    {
        try
        {
            var cacheNamespaceSplitted = GetType().Namespace!.Split(".");
            var cacheNamespaceStartsWith = cacheNamespaceSplitted.Length == 1 ? cacheNamespaceSplitted[0] : string.Join('.', cacheNamespaceSplitted.Take(cacheNamespaceSplitted.Length - 1));

            var keyList = new List<string>();
            foreach (var ep in _redisCon.GetEndPoints())
            {
                var server = _redisCon.GetServer(ep);
                foreach (var patternItem in pattern.Split(","))
                {
                    var keys = server.Keys(pattern: $"*{patternItem.Trim()}*").Select(s => s.ToString()).Where(x => x.StartsWith(cacheNamespaceStartsWith)).ToList();
                    if (keys.Any())
                        keyList.AddRange(keys);
                }
            }

            if (keyList.Any())
                await _cache.KeyDeleteAsync(keyList.Distinct().Select(s => (RedisKey)s).ToArray());

            return true;
        }
        catch
        {
            return false;
        }
    }

    public void ClearAll()
    {
        var endpoints = _redisCon.GetEndPoints(true);
        foreach (var endpoint in endpoints)
        {
            var server = _redisCon.GetServer(endpoint);
            server.FlushAllDatabases();
        }
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action) where T : class
    {
        var result = await _cache.StringGetAsync(key);
        if (result.IsNull)
        {
            result = JsonSerializer.SerializeToUtf8Bytes(await action());
            await SetValueAsync(key, result);
        }
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
    }

    public async Task<string> GetValueAsync(string key)
    {
        return await _cache.StringGetAsync(key);
    }

    public async Task<bool> SetValueAsync(string key, string value)
    {
        return await _cache.StringSetAsync(key, value, ExpireTime);
    }

    public async Task<bool> SetValueAsync(string key, string value, TimeSpan expireTime)
    {
        return await _cache.StringSetAsync(key, value, expireTime);
    }

    public T GetOrAdd<T>(string key, Func<T> action) where T : class
    {
        var result = _cache.StringGet(key);
        if (result.IsNull)
        {
            result = JsonSerializer.SerializeToUtf8Bytes(action());
            _cache.StringSet(key, result, ExpireTime);
        }
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);
    }
}
