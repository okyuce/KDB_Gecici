using Csb.YerindeDonusum.Application.Dtos;

namespace Csb.YerindeDonusum.WebApp.Services
{
    public interface IAuthService
    {
        Task SignInAsync(TokenDto tokenData);
        Task SignOutAsync();
        bool IsInRole(params string[] roles);
        //bool GenelMudurlukMu();
        //string GetClaimValue(string type);
        //T GetClaimValueGeneric<T>(string type);
    }
}