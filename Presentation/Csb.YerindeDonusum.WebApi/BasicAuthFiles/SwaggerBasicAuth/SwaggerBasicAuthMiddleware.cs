﻿using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Csb.YerindeDonusum.WebApi.BasicAuthFiles.SwaggerBasicAuth;

public class SwaggerBasicAuthMiddleware
{
    private readonly RequestDelegate next;
    private readonly IConfiguration _configuration;

    public SwaggerBasicAuthMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _configuration = configuration;
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                // Get the credentials from request header
                var header = AuthenticationHeaderValue.Parse(authHeader);
                var inBytes = Convert.FromBase64String(header.Parameter);
                var credentials = Encoding.UTF8.GetString(inBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];
                // validate credentials
                var swaggerUsername = _configuration["SwaggerBasicAuth:Username"];
                var swaggerPassword = _configuration["SwaggerBasicAuth:Password"];
                if (username.Equals(swaggerUsername)
                  && password.Equals(swaggerPassword))
                {
                    await next.Invoke(context).ConfigureAwait(false);
                    return;
                }
            }
            context.Response.Headers["WWW-Authenticate"] = "Basic";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        else
        {
            await next.Invoke(context).ConfigureAwait(false);
        }
    }
}
