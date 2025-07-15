using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.KullaniciGirisBasariliCQRS.Commands.EkleKullaniciGirisBasarili;
using Csb.YerindeDonusum.Application.CQRS.KullaniciGirisHataCQRS.Commands.EkleKullaniciGirisHata;
using Csb.YerindeDonusum.Application.CQRS.KullaniciGirisKodDenemeCQRS.Command.EkleKullaniciGirisKodDeneme;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using CSB.Core.LogHandler.Attr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciGirisDogrula;

[MaskLoginInfo]
public class KullaniciGirisDogrulaCommand : IRequest<ResultModel<TokenDto>>
{
    public string GirisGuid { get; set; }
    public string DogrulamaKodu { get; set; }


    public class KullaniciGirisDogrulaCommandHandler : IRequestHandler<KullaniciGirisDogrulaCommand, ResultModel<TokenDto>>
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IKullaniciGirisKodRepository _kullaniciGirisKodRepository;
        private readonly IKullaniciGirisHataRepository _kullaniciGirisHataRepository;
        private JwtOptionModel JwtOptions { get; set; }

        public KullaniciGirisDogrulaCommandHandler(
            IMediator mediator, 
            IServiceProvider serviceProvider, 
            IMapper mapper, 
            IHttpContextAccessor contextAccessor,
            IKullaniciRepository kullaniciRepository,
            IKullaniciGirisKodRepository kullaniciGirisKodRepository,
            IKullaniciGirisHataRepository kullaniciGirisHataRepository)
        {
            _mediator = mediator;
            _contextAccessor = contextAccessor;
            _kullaniciRepository = kullaniciRepository;
            _kullaniciGirisKodRepository = kullaniciGirisKodRepository;
            _kullaniciGirisHataRepository = kullaniciGirisHataRepository;
            JwtOptions = serviceProvider.GetRequiredService<IOptionsMonitor<JwtOptionModel>>().CurrentValue;
        }

        public async Task<ResultModel<TokenDto>> Handle(KullaniciGirisDogrulaCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<TokenDto>();

            var jwtAddon = new JwtAddon(_contextAccessor, JwtOptions);

            string ipAddress = _contextAccessor?.HttpContext?.GetIpAddress();

            #region Kullanici Giris Kod Deneme Ekle

            var kullaniciGirisKodDeneme = new EkleKullaniciGirisKodDenemeCommand()
            {
                Code = request.DogrulamaKodu,
                IpAdres = ipAddress,
                GirisGuid = request.GirisGuid
            };
            await _mediator.Send(kullaniciGirisKodDeneme, cancellationToken);

            #endregion

            #region Request Kontrolleri

            if (!Guid.TryParse(request.GirisGuid, out Guid girisGuid))
            {
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    Code = request.DogrulamaKodu,
                    GirisGuid = request.GirisGuid,
                    Aciklama = "Giris guid hatalı."
                });
                result.Exception(new Exception("Hatalı istek yaptınız!"));
                return await Task.FromResult(result);
            }

            request.DogrulamaKodu = request.DogrulamaKodu.Trim();

            var kullaniciGirisKod = _kullaniciGirisKodRepository.GetWhere(x => !x.Tamamlandi && x.GirisGuid == girisGuid && x.SilindiMi == false).FirstOrDefault();

            if (kullaniciGirisKod == null)
            {
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    Code = request.DogrulamaKodu,
                    GirisGuid = request.GirisGuid,
                    Aciklama = "GirisGuid ile eşleşen giriş kod kaydı bulunamadı."
                });
                result.Exception(new NullReferenceException("Doğrulama kodu bulunamadı lütfen tekrar giriş yapınız!"));
                return await Task.FromResult(result);
            }

            #endregion

            #region Kullanıcı Bilgileri Kontrolleri

            var kullanici = await _kullaniciRepository
                 .GetWhere(x => x.KullaniciId == kullaniciGirisKod.KullaniciId && !x.SilindiMi)
                 .Include(x => x.Birim)
                 .Include(x => x.KullaniciRols.Where(y => !y.SilindiMi && y.AktifMi == true))
                 .ThenInclude(x => x.Rol)
                 .FirstOrDefaultAsync();

            if (kullanici == null)
            {
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    Code = request.DogrulamaKodu,
                    GirisGuid = request.GirisGuid,
                    Aciklama = "Kullanıcı bulunamadı."
                });
                result.Exception(new NullReferenceException("Geçersiz veya Hatalı Kullanıcı Bilgisi! ERR-KGCH-200"));
                return await Task.FromResult(result);
            }
            else if (kullanici.AktifMi != true)
            {
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    KullaniciAdi = kullanici.KullaniciAdi,
                    Code = request.DogrulamaKodu,
                    GirisGuid = request.GirisGuid,
                    Aciklama = "Kullanıcı hesabı aktif değil."
                });
                result.Exception(new NullReferenceException("Hesabınız Aktif Olmadığı İçin Giriş Yapamazsınız! ERR-KGCH-300"));
                return await Task.FromResult(result);
            }

            #endregion

            #region Hatalı Deneme Kontrolleri

            var girisHataKontrolSonSaat = _kullaniciGirisHataRepository.Count(x => x.KullaniciAdi == kullanici.KullaniciAdi && x.OlusturmaTarihi >= DateTime.Now.AddHours(-1) && x.SilindiMi == false);
            //son 1 saat içinde 6'dan fazla hatalı şifre denemesi yapmışsa engelle
            if (girisHataKontrolSonSaat > 6)
            {
                result.Exception(new Exception("Çok fazla deneme yaptınız, lütfen biraz bekleyin!"));
                return await Task.FromResult(result);
            }

            var girisHataKontrolSon6Saat = _kullaniciGirisHataRepository.Count(x => x.KullaniciAdi == kullanici.KullaniciAdi && x.OlusturmaTarihi >= DateTime.Now.AddHours(-6) && x.SilindiMi == false);
            //son 6 saat içinde 12'dan fazla hatalı şifre denemesi yapmışsa engelle
            if (girisHataKontrolSon6Saat > 12)
            {
                result.Exception(new Exception("Çok fazla deneme yaptınız, lütfen biraz bekleyin!"));
                return await Task.FromResult(result);
            }

            var girisHataKontrolSon12Saat = _kullaniciGirisHataRepository.Count(x => x.KullaniciAdi == kullanici.KullaniciAdi && x.OlusturmaTarihi >= DateTime.Now.AddHours(-12) && x.SilindiMi == false);
            //son 12 saat içinde 20'den fazla hatalı şifre denemesi yapmışsa engelle
            if (girisHataKontrolSon12Saat > 20)
            {
                result.Exception(new Exception("Çok fazla deneme yaptınız, lütfen biraz bekleyin!"));
                return await Task.FromResult(result);
            }

            var girisHataKontrolSon24SaatIpAdres = _kullaniciGirisHataRepository.Count(x => x.IpAdres == ipAddress && x.OlusturmaTarihi == DateTime.Now.AddHours(-24) && x.SilindiMi == false);
            //son 24 saat içinde aynı ipden 10'dan fazla hatalı giriş gelirse engelle
            if (girisHataKontrolSon24SaatIpAdres > 10)
            {
                result.Exception(new Exception("Çok fazla deneme yaptınız, lütfen biraz bekleyin!"));
                return await Task.FromResult(result);
            }

            var girisHataKontrolSonAyIpAdres = _kullaniciGirisHataRepository.Count(x => x.IpAdres == ipAddress && x.OlusturmaTarihi >= DateTime.Now.AddMonths(-1) && x.SilindiMi == false);
            //son 1 ay içinde aynı ipden 30'dan fazla hatalı giriş gelirse engelle
            if (girisHataKontrolSonAyIpAdres > 30)
            {
                result.Exception(new Exception("Çok fazla deneme yaptınız, lütfen biraz bekleyin!"));
                return await Task.FromResult(result);
            }

            #endregion

            #region Doğrulama Kodu Kontrolleri

            if (kullaniciGirisKod == null)
            {
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    KullaniciAdi = kullanici.KullaniciAdi,
                    Code = request.DogrulamaKodu,
                    GirisGuid = request.GirisGuid,
                    Aciklama = "Doğrulama kodu bulunamadı."
                });
                result.Exception(new NullReferenceException("Giriş isteğine ait kod bulunamadı!"));
                return await Task.FromResult(result);
            }
            else if (kullaniciGirisKod.Code != request.DogrulamaKodu)
            {
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    KullaniciAdi = kullanici.KullaniciAdi,
                    Code = request.DogrulamaKodu,
                    GirisGuid = request.GirisGuid,
                    Aciklama = "Doğrulama kodu hatalı."
                });
                result.Exception(new Exception("Doğrulama kodunu hatalı girdiniz, lütfen tekrar deneyin."));
                return await Task.FromResult(result);
            }
            else if (kullaniciGirisKod.OlusturmaTarihi.AddMinutes(15) < DateTime.Now)
            {
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    KullaniciAdi = kullanici.KullaniciAdi,
                    Code = request.DogrulamaKodu,
                    GirisGuid = request.GirisGuid,
                    Aciklama = "Doğrulama kodu süresi doldu."
                });
                result.Exception(new Exception("Doğrulama kodu süresini aştınız, lütfen tekrar giriş yapın."));
                return await Task.FromResult(result);
            }

            #endregion

            #region Doğrulama Başarılı Kaydetme

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

            kullaniciGirisKod.GuncellemeTarihi = DateTime.Now;
            kullaniciGirisKod.GuncelleyenKullaniciId = kullanici.KullaniciId;
            kullaniciGirisKod.GuncelleyenIp = ipAddress;
            kullaniciGirisKod.Tamamlandi = true;

            _kullaniciGirisKodRepository.Update(kullaniciGirisKod);

            //Tamamlandı olmayan doğrulama kodlarını sil
            var kullaniciGirisKodList = _kullaniciGirisKodRepository.GetWhere(x => x.KullaniciGirisKodId != kullaniciGirisKod.KullaniciGirisKodId && x.KullaniciId == kullanici.KullaniciId && x.SilindiMi == false).ToList();
            foreach (var _kullaniciGirisKod in kullaniciGirisKodList)
            {
                _kullaniciGirisKod.GuncellemeTarihi = DateTime.Now;
                _kullaniciGirisKod.GuncelleyenKullaniciId = kullanici.KullaniciId;
                _kullaniciGirisKod.GuncelleyenIp = ipAddress;
                _kullaniciGirisKod.SilindiMi = true;
            }
            _kullaniciGirisKodRepository.UpdateRange(kullaniciGirisKodList);
            await _kullaniciGirisKodRepository.SaveChanges(cancellationToken);

            await _mediator.Send(new EkleKullaniciGirisBasariliCommand()
            {
                KullaniciAdi = kullanici.KullaniciAdi,
                Sifre = "LDAP + SMS"
            });
            
            #endregion

            result.Result = new TokenDto()
            {
                AccessToken = jwtAddon.GenerateJwt(kullanici),
                RefreshToken = kullanici.RefreshToken
            };

            return await Task.FromResult(result);            
        }
    }
}
