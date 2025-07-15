
namespace Csb.YerindeDonusum.WebApi.Middleware;
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate next;
    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task Invoke(HttpContext httpContext)
    {
        httpContext.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
        httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        httpContext.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
        httpContext.Response.Headers.Add("Content-Security-Policy",
         "default-src 'self' 'unsafe-inline'; " +
         "worker-src blob:; " +
         "frame-src 'self' *.google.com; " +
         "img-src 'self' https://tile.openstreetmap.org data:; " +
         "script-src 'self' 'unsafe-inline' *.csb.gov.tr; " +
         "connect-src 'self' *.csb.gov.tr wss://localhost:44316; " +
         "style-src 'self' fonts.googleapis.com 'unsafe-inline'; " +
         "font-src 'self' fonts.googleapis.com fonts.gstatic.com data:"
         );
        await next(httpContext);
        
    }
}

