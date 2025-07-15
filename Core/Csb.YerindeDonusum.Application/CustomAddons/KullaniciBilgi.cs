using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Csb.YerindeDonusum.Application.CustomAddons
{
    public class KullaniciBilgi : IKullaniciBilgi
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public KullaniciBilgi(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public KullaniciBilgiModel GetUserInfo()
        {
            KullaniciBilgiModel kullaniciBilgi = new KullaniciBilgiModel();

            var user = _contextAccessor?.HttpContext?.User;
            var headers = _contextAccessor?.HttpContext?.Request?.Headers;

            kullaniciBilgi.KullaniciId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            kullaniciBilgi.AdSoyad = user?.FindFirst(ClaimTypes.Name)?.Value;
            kullaniciBilgi.TcKimlikNo = user?.FindFirst("TcKimlikNo")?.Value;
            kullaniciBilgi.Ad = user?.FindFirst(ClaimTypes.GivenName)?.Value;
            kullaniciBilgi.Soyad = user?.FindFirst(ClaimTypes.Surname)?.Value;
            kullaniciBilgi.Eposta = user?.FindFirst(ClaimTypes.Email)?.Value;
            kullaniciBilgi.BirimAdi = user?.FindFirst("BirimAdi")?.Value;

            long.TryParse(user?.FindFirst("BirimId")?.Value, out long birimId);
            kullaniciBilgi.BirimId = birimId;

            int.TryParse(user?.FindFirst("BirimIlId")?.Value, out int birimIlId);
            kullaniciBilgi.BirimIlId = birimIlId;

            kullaniciBilgi.IpAdresi =  _contextAccessor?.HttpContext?.GetIpAddress();
            kullaniciBilgi.Claims = user?.Claims;

            return kullaniciBilgi;
        }

        public bool IsInRole(params string[] roles)
        {
            return GetUserInfo()?.Claims?.Any(x => x.Type == ClaimTypes.Role && roles.Contains(x.Value)) == true;
        }


    }

    public class KullaniciBilgiModel
    {
        public string? KullaniciAdi { get; set; }
        public string? KullaniciId { get; set; }
        public string? KullaniciGuid { get; set; }
        public string? AdSoyad { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? Eposta { get; set; }
        public string? TcKimlikNo { get; set; }
        public long? BirimId { get; set; }
        public string? BirimAdi { get; set; }
        public int BirimIlId { get; set; }
        public IEnumerable<Claim>? Claims { get; set; }
        public string? IpAdresi { get; set; }
    }
}