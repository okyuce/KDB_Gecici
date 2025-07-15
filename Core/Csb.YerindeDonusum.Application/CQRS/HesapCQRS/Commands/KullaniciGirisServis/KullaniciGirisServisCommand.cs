using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Cryptography;
using CSB.Core.LogHandler.Attr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciGirisServis;

[MaskLoginInfo]
public class KullaniciGirisServisCommand : IRequest<ResultModel<TokenDto>>
{
    public string? KullaniciAdi { get; set; }
    public string? Sifre { get; set; }

    public class KullaniciGirisServisCommandHandler : IRequestHandler<KullaniciGirisServisCommand, ResultModel<TokenDto>>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IKullaniciRepository _kullaniciRepository;
        private JwtOptionModel JwtOptions { get; set; }

        public KullaniciGirisServisCommandHandler(IServiceProvider serviceProvider, IHttpContextAccessor contextAccessor, IKullaniciRepository kullaniciRepository)
        {
            _contextAccessor = contextAccessor;
            _kullaniciRepository = kullaniciRepository;
            JwtOptions = serviceProvider.GetRequiredService<IOptionsMonitor<JwtOptionModel>>().CurrentValue;
        }

        public async Task<ResultModel<TokenDto>> Handle(KullaniciGirisServisCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<TokenDto>();

            var jwtAddon = new JwtAddon(_contextAccessor, JwtOptions);

            var kullanici = await _kullaniciRepository
                                .GetWhere(x =>
                                    x.KullaniciAdi.Equals(request.KullaniciAdi)
                                    &&
                                    !x.SilindiMi
                                )
                                .Include(x => x.Birim)
                                .Include(x => x.KullaniciRols.Where(y => !y.SilindiMi && y.AktifMi == true))
                                .ThenInclude(x => x.Rol)
                                .FirstOrDefaultAsync();

            if (kullanici == null)
            {
                result.Exception(new NullReferenceException("Geçersiz veya Hatalı Kullanıcı Bilgisi! ERR-KGCH-200"));
                return await Task.FromResult(result);
            }
            else if (kullanici.AktifMi != true)
            {
                result.Exception(new NullReferenceException("Hesabınız Aktif Olmadığı İçin Giriş Yapamazsınız! ERR-KGCH-300"));
                return await Task.FromResult(result);
            }
            else if (kullanici.SistemKullanicisiMi != true)
            {
                result.Exception(new NullReferenceException("Hesabınız Sistem Kullanıcısı Olmadığı İçin Giriş Yapamazsınız! ERR-KGCH-300"));
                return await Task.FromResult(result);
            }

            var sifreDogruMu = false;
            if (kullanici.KullaniciHesapTipId == KullaniciHesapTipEnum.LDAP.GetHashCode())
            {
                sifreDogruMu = LdapExtension.KullaniciAdiSifreKontrol(request.KullaniciAdi, request.Sifre);
            }
            else
            {
                var sifreHashli = CsbCryptography.Sha256(CsbCryptography.Sha256(CsbCryptography.MD5(request.Sifre)));
                sifreDogruMu = kullanici.Sifre.Equals(sifreHashli);
            }

            if (!sifreDogruMu)
            {
                result.Exception(new NullReferenceException("Geçersiz veya Hatalı Kullanıcı Adı veya Şifre Bilgisi! ERR-KGCH-400"));
                return await Task.FromResult(result);
            }

            kullanici.RefreshToken = jwtAddon.GenerateRefreshToken();
            // *güvenlik politikası gereği eklenecek son giriş ve ip bilgisi
            kullanici.SonGirisYapilanTarih = DateTime.Now;
            kullanici.SonGirisYapilanIp = _contextAccessor.HttpContext?.GetIpAddress();

            await _kullaniciRepository.SaveChanges(cancellationToken);

            result.Result = new TokenDto()
            {
                AccessToken = jwtAddon.GenerateJwt(kullanici),
                RefreshToken = kullanici.RefreshToken
            };

            return await Task.FromResult(result);
        }
    }
}
