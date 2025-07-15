using Csb.YerindeDonusum.Application.CustomAddons;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IKullaniciBilgi
{
    KullaniciBilgiModel GetUserInfo();
    bool IsInRole(params string[] roles);
}