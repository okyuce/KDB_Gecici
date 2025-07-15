namespace Csb.YerindeDonusum.Application.Interfaces;

public interface ICacheService
{
    Task<bool> ClearByPattern(string pattern);
    Task<string> GetValueAsync(string key);
    Task<bool> SetValueAsync(string key, string value);
    Task<bool> SetValueAsync(string key, string value, TimeSpan expireTime);
    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action) where T : class;
    T GetOrAdd<T>(string key, Func<T> action) where T : class;
    Task Clear(string key);
    void ClearAll();
}
