using Microsoft.AspNetCore.Authentication;
using System.DirectoryServices.AccountManagement;

namespace Csb.YerindeDonusum.Application.Extensions;

public static class LdapExtension
{
    public static bool KullaniciAdiSifreKontrol(string kullaniciAdi, string password)
    {
        using (PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, "csb.local"))
        {
            try
            {
                return principalContext.ValidateCredentials(kullaniciAdi, password);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}