namespace Csb.YerindeDonusum.WebApi.BasicAuthFiles.SwaggerBasicAuth;

public static class SwaggerBasicAuthExtensions
{
    public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SwaggerBasicAuthMiddleware>();
    }
}
