using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.WebApp.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.WebApp.Services;     // IHttpService

namespace Csb.YerindeDonusum.WebApp.Services
{
    public class NatamamYapiAppService : INatamamYapiAppService
    {
        private readonly IHttpService _http;
        public NatamamYapiAppService(IHttpService http) => _http = http;

        public async Task<IEnumerable<NatamamYapiIslemViewModel>> GetAllAsync()
        {
            // 1) Kod ve dosya verisini çek
            var kodResult = await _http.GetAsync<List<IstisnaAskiKoduDto>>("IstisnaAskiKodu/GetAll");
            var dosyaResult = await _http.GetAsync<List<IstisnaAskiKoduDosyaDto>>("IstisnaAskiKoduDosya/GetAll");
            var kodlar = kodResult?.ResultModel?.Result ?? new List<IstisnaAskiKoduDto>();
            var dosyalar = dosyaResult?.ResultModel?.Result ?? new List<IstisnaAskiKoduDosyaDto>();

            // 2) Kullanıcı listesini çek
            var userResult = await _http.GetAsync<List<KullaniciDto>>("Kullanici/GetAll");
            var kullanicilar = userResult?.ResultModel?.Result ?? new List<KullaniciDto>();

            // 3) Join & Map
            var vmList = kodlar.Select(k =>
            {
                // Bu koda ait tüm dosyalar
                var myFiles = dosyalar.Where(d => d.IstisnaAskiKoduId == k.IstisnaAskiKoduId);

                // Oluşturan/Güncelleyen kullanıcıyı bul
                // ID’leri kullanıcı listesinde eşleştiriyoruz
                var oluşturan = kullanicilar.FirstOrDefault(u => u.KullaniciId == k.OlusturanKullaniciId);
                var güncelleyen = k.GuncelleyenKullaniciId.HasValue
                    ? kullanicilar.FirstOrDefault(u => u.KullaniciId == k.GuncelleyenKullaniciId.Value)
                    : null;

                // İşte burası değişti:
                string ComposeName(KullaniciDto u) =>
                    $"{u.Ad?.Trim()} {u.Soyad?.Trim()}".Trim();

                return new NatamamYapiIslemViewModel
                {
                    Id = k.IstisnaAskiKoduId,
                    AskiKodu = k.AskiKodu,
                    ResmiYaziDosyaAdi = myFiles.FirstOrDefault(d => d.DosyaTuru == "Resmi")?.DosyaAdi,
                    DigerYaziDosyaAdi = myFiles.FirstOrDefault(d => d.DosyaTuru == "Diger")?.DosyaAdi,
                    OlusturanKullaniciAd = oluşturan != null ? ComposeName(oluşturan) : "-",
                    GuncelleyenKullaniciAd = güncelleyen != null ? ComposeName(güncelleyen) : "-",
                    AktifMi = k.AktifMi
                };
            });

            return vmList;
        }
    }

}
