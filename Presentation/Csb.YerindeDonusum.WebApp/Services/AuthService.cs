using Csb.YerindeDonusum.Application.Dtos;
using CSB.Core.Entities.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Csb.YerindeDonusum.WebApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SignInAsync(TokenDto tokenData)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(tokenData.AccessToken) as JwtSecurityToken;

            var claims = jwtToken.Claims.ToList();
            claims.Add(new Claim("AccessToken", tokenData.AccessToken));
            claims.Add(new Claim("RefreshToken", tokenData.RefreshToken));

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(userIdentity);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public bool IsInRole(params string[] roles)
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.Any(x => x.Type == ClaimTypes.Role && (roles.Contains(x.Value) || x.Value == "Admin")) == true;
        }
        //public bool GenelMudurlukMu()
        //{
        //    int.TryParse(GetClaimValue("BirimIlId"), out int birimIlId);
        //    var birimilid = GetClaimValueGeneric<int>("BirimIlId");
        //    return birimIlId > 0;
        //}
        //public string GetClaimValue(string type)
        //{
        //    return _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type.ToLower() == type.ToLower())?.Value ?? "";
        //} 

        //public T GetClaimValueGeneric<T>(string type)
        //{     
        //    try
        //    {
        //        var converter = TypeDescriptor.GetConverter(typeof(T));
        //        if (converter != null)
        //        {
        //            // Cast ConvertFromString(string text) : object to (T)
        //            return (T)converter.ConvertFromString(_httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == type)?.Value);
        //        }
        //    }
        //    catch { }

        //    return default;
        //}
    }
}