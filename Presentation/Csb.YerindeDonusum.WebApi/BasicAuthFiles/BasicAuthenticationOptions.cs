using Microsoft.AspNetCore.Authentication;
namespace Csb.YerindeDonusum.WebApi.BasicAuthFiles;
public class BasicAuthenticationOptions : AuthenticationSchemeOptions
{
    public string Realm { get; set; }
}
