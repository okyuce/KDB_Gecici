using Csb.YerindeDonusum.Application.Enums;
using System.Globalization;

namespace Csb.YerindeDonusum.WebApp.Middleware
{
    public class KullaniciParolaZamanAsimiMiddleware
    {
        private readonly RequestDelegate _next;
        public KullaniciParolaZamanAsimiMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? requestPath = context?.Request?.Path.Value?.ToLowerInvariant()?.Trim().TrimStart('/').TrimEnd('/');

            if (context?.User?.Identity?.IsAuthenticated != true)
            {
                //giriş yapmamışsa sonlandır
                await _next(context);
                return;
            }

            if (requestPath?.StartsWith("admin") != true)
            {
                //yönetim panelinde işlem yapılmıyorsa sonlandır
                await _next(context);
                return;
            }

            bool isAjaxRequest = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjaxRequest)
            {
                //ajax isteği yapılıyorsa 
                await _next(context);
                return;
            }

            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            var controllerName = context.Request.RouteValues["controller"]?.ToString()?.ToLowerInvariant();
            if (controllerName == "home")
            {
                //home controller içindeki işlemler public olduğu için controller == home ise sonlandır
                await _next(context);
                return;
            }

            var kullaniciHesapTipId = context.User.FindFirst("KullaniciHesapTipId")?.Value;
            if (kullaniciHesapTipId != KullaniciHesapTipEnum.Local.GetHashCode().ToString())
            {
                //local kullanıcı değilse sonlandır
                await _next(context);
                return;
            }

            DateTime.TryParseExact(
                context.User.FindFirst("SonSifreDegisimTarihi")?.Value ?? DateTime.Today.ToString("yyyy-MM-dd"),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime sonSifreDegisimTarihi
            );

            if (sonSifreDegisimTarihi < DateTime.Today.AddMonths(-3))
            {
                //şifre değiştirmesi gerekiyor
                context.Response.Redirect("/admin/");
                return;
            }

            await _next(context);
        }
    }
}