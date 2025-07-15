using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.WebApp.Utilities.Helpers
{
    public static class RoleHelper
    {
        public static bool HasRole(this List<string> rolListe, params string[] rol) {
            return rolListe.Any(x => x == RoleEnum.Admin || rol.Contains(x));
        }
    }
}