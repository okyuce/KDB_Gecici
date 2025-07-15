using Csb.YerindeDonusum.Application.Dtos;

namespace Csb.YerindeDonusum.WebApi.BasicAuthFiles.Interfaces;
public interface IBasicAuthenticationService
{
    Task<KullaniciSonucDto> IsValidUserAsync(string user, string password);
}
