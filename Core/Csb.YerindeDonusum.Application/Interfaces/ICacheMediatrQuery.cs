namespace Csb.YerindeDonusum.Application.Interfaces;

public interface ICacheMediatrQuery
{
    bool? CacheCustomUser { get; }
    int? CacheMinute { get; }
    bool CacheIsActive { get; }
}