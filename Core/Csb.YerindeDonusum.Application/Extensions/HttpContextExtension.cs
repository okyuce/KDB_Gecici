using Microsoft.AspNetCore.Http;

namespace Csb.YerindeDonusum.Application.Extensions;

public static class HttpContextExtension
{
    public static string? GetIpAddress(this HttpContext context)
    {
        if (context?.Request?.Headers?.ContainsKey("X-Forwarded-For") == true)
            return context.Request.Headers["X-Forwarded-For"].ToString();

        return context?.Connection?.RemoteIpAddress?.ToString();
    }
}