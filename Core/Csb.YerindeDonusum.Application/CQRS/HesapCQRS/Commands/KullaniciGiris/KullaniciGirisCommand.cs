using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.KullaniciGirisBasariliCQRS.Commands.EkleKullaniciGirisBasarili;
using Csb.YerindeDonusum.Application.CQRS.KullaniciGirisHataCQRS.Commands.EkleKullaniciGirisHata;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Cryptography;
using Csb.YerindeDonusum.Domain.Entities;
using CSB.Core.LogHandler.Attr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data;

namespace Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciGiris;

[MaskLoginInfo]
public class KullaniciGirisCommand : IRequest<ResultModel<KullaniciGirisKodDto>>
{
    public string? KullaniciAdi { get; set; }
    public string? Sifre { get; set; }
    public string? GuvenlikKoduKey { get; set; }
    public string? GuvenlikKodu { get; set; }

    public class KullaniciGirisCommandHandler : IRequestHandler<KullaniciGirisCommand, ResultModel<KullaniciGirisKodDto>>
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IKullaniciGirisKodRepository _kullaniciGirisKodRepository;
        private readonly IKullaniciGirisHataRepository _kullaniciGirisHataRepository;
        private readonly ISmsService _smsService;
        private JwtOptionModel JwtOptions { get; set; }

        public KullaniciGirisCommandHandler(
            IMediator mediator, 
            IServiceProvider serviceProvider, 
            IMapper mapper, 
            IHttpContextAccessor contextAccessor, 
            IKullaniciRepository kullaniciRepository, 
            IKullaniciGirisKodRepository kullaniciGirisKodRepository,
            IKullaniciGirisHataRepository kullaniciGirisHataRepository,
            ISmsService smsService)
        {
            _mediator = mediator;
            _contextAccessor = contextAccessor;
            _kullaniciRepository = kullaniciRepository;
            _kullaniciGirisKodRepository = kullaniciGirisKodRepository;
            _kullaniciGirisHataRepository = kullaniciGirisHataRepository;
            JwtOptions = serviceProvider.GetRequiredService<IOptionsMonitor<JwtOptionModel>>().CurrentValue;
            _smsService = smsService;
        }

        public async Task<ResultModel<KullaniciGirisKodDto>> Handle(KullaniciGirisCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<KullaniciGirisKodDto>();

            #region Doğrulama Kodu Kontrolleri

            string ipAddress = _contextAccessor?.HttpContext?.GetIpAddress();
            
            var girisHataKontrolSonSaat = _kullaniciGirisHataRepository.Count(x=> x.KullaniciAdi == request.KullaniciAdi && x.OlusturmaTarihi >= DateTime.Now.AddHours(-1) && x.SilindiMi == false);
            //son 1 saat içinde 6'dan fazla hatalı şifre denemesi yapmışsa engelle
            if (girisHataKontrolSonSaat > 6)
            {
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    KullaniciAdi = request.KullaniciAdi,
                    Sifre = request.Sifre,
                    Aciklama = "1 saat içinde 6'dan fazla deneme yapıldı."
                });
                result.Exception(new Exception("Çok fazla deneme yaptınız, lütfen biraz bekleyin!"));
                return await Task.FromResult(result);
            }

            var girisHataKontrolSon6Saat = _kullaniciGirisHataRepository.Count(x => x.KullaniciAdi == request.KullaniciAdi && x.OlusturmaTarihi >= DateTime.Now.AddHours(-6) && x.SilindiMi == false);
            //son 6 saat içinde 12'dan fazla hatalı şifre denemesi yapmışsa engelle
            if (girisHataKontrolSon6Saat > 12)
            {
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    KullaniciAdi = request.KullaniciAdi,
                    Sifre = request.Sifre,
                    Aciklama = "6 saat içinde 12'den fazla deneme yapıldı."
                });
                result.Exception(new Exception("Çok fazla deneme yaptınız, lütfen biraz bekleyin!"));
                return await Task.FromResult(result);
            }

            var girisHataKontrolSon12Saat = _kullaniciGirisHataRepository.Count(x => x.KullaniciAdi == request.KullaniciAdi && x.OlusturmaTarihi >= DateTime.Now.AddHours(-12) && x.SilindiMi == false);
            //son 12 saat içinde 20'den fazla hatalı şifre denemesi yapmışsa engelle
            if (girisHataKontrolSon12Saat > 20)
            {
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    KullaniciAdi = request.KullaniciAdi,
                    Sifre = request.Sifre,
                    Aciklama = "12 saat içinde 20'den fazla deneme yapıldı."
                });
                result.Exception(new Exception("Çok fazla deneme yaptınız, lütfen biraz bekleyin!"));
                return await Task.FromResult(result);
            }

            var girisHataKontrolSon24SaatIpAdres = _kullaniciGirisHataRepository.Count(x=> x.IpAdres == ipAddress && x.IpAdres != "::1" && x.OlusturmaTarihi == DateTime.Now.AddHours(-24) && x.SilindiMi == false);
            //son 24 saat içinde aynı ipden 10'dan fazla hatalı giriş gelirse engelle
            if (girisHataKontrolSon24SaatIpAdres > 10)
            {
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    KullaniciAdi = request.KullaniciAdi,
                    Sifre = request.Sifre,
                    Aciklama = "24 saat içinde aynı ip adresinden 10'dan fazla deneme yapıldı."
                });
                result.Exception(new Exception("Çok fazla deneme yaptınız, lütfen biraz bekleyin!"));
                return await Task.FromResult(result);
            }

            var girisHataKontrolSonAyIpAdres = _kullaniciGirisHataRepository.Count(x => x.IpAdres== ipAddress && x.IpAdres!= "::1" && x.OlusturmaTarihi >= DateTime.Now.AddMonths(-1) && x.SilindiMi == false);
            //son 1 ay içinde aynı ipden 30'dan fazla hatalı giriş gelirse engelle
            if (girisHataKontrolSonAyIpAdres > 30)
            {
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    KullaniciAdi = request.KullaniciAdi,
                    Sifre = request.Sifre,
                    Aciklama = "1 ay içinde aynı ip adresinden 30'dan fazla deneme yapıldı."
                });
                result.Exception(new Exception("Çok fazla deneme yaptınız, lütfen biraz bekleyin!"));
                return await Task.FromResult(result);
            }

            #endregion

            #region Kullanici Bilgiler Kontrol

            var kullanici = await _kullaniciRepository
                .GetWhere(x =>x.KullaniciAdi.Equals(request.KullaniciAdi) && !x.SilindiMi)
                .Include(x => x.Birim)
                .Include(x => x.KullaniciRols.Where(y => !y.SilindiMi && y.AktifMi == true))
                .ThenInclude(x => x.Rol)
                .FirstOrDefaultAsync();

            if (kullanici == null)
            {
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    KullaniciAdi = request.KullaniciAdi,
                    Sifre = request.Sifre,
                    Aciklama = "Kullanıcı bilgisi geçersiz veya hatalı."
                });
                result.Exception(new NullReferenceException("Geçersiz veya Hatalı Kullanıcı Bilgisi! ERR-KGCH-200"));
                return await Task.FromResult(result);
            }
            else if (kullanici.AktifMi != true)
            {
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    KullaniciAdi = request.KullaniciAdi,
                    Sifre = request.Sifre,
                    Aciklama = "Kullanıcı hesabı aktif değil."
                });
                result.Exception(new NullReferenceException("Hesabınız Aktif Olmadığı İçin Giriş Yapamazsınız! ERR-KGCH-300"));
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
                await _mediator.Send(new EkleKullaniciGirisHataCommand()
                {
                    KullaniciAdi = request.KullaniciAdi,
                    Sifre = request.Sifre,
                    Aciklama = "Kullanıcı adı veya şifre bilgisi geçersiz veya hatalı."
                });
                result.Exception(new NullReferenceException("Geçersiz veya Hatalı Kullanıcı Adı veya Şifre Bilgisi! ERR-KGCH-400"));
                return await Task.FromResult(result);
            }

            #endregion

            #region Doğrulama Kodu İşlemleri

            //Son 5 dk içinde kod gönderildiyse son gönderilen kodu alır
            var sonKullaniciGirisKodu = _kullaniciGirisKodRepository.GetWhere(x => x.KullaniciId == kullanici.KullaniciId && x.OlusturmaTarihi >= DateTime.Now.AddMinutes(-5) && x.SilindiMi == false).OrderByDescending(o => o.OlusturmaTarihi).FirstOrDefault();
            //Kod ile giriş yapılmamış ise sms göndermeden bu kod ile giriş beklenir
            if (sonKullaniciGirisKodu != null && !sonKullaniciGirisKodu.Tamamlandi)
            {
                result.Result = new KullaniciGirisKodDto { GirisGuid = sonKullaniciGirisKodu.GirisGuid.ToString() };
                return await Task.FromResult(result);
            }

            string telefonNo = StringAddon.ToClearPhone(kullanici.CepTelefonu ?? "");

            if(!StringAddon.ValidatePhone(telefonNo))
            {
                result.Exception(new NullReferenceException("Sistemde kayıtlı telefon numaranız hatalı olduğu için doğrulama kodu gönderilemedi, lütfen sistem yöneticizle irtibata geçin."));
                return await Task.FromResult(result);
            }

            var kullaniciGirisKod = _kullaniciGirisKodRepository.AddAsync(new KullaniciGirisKod
            {
                KullaniciId = kullanici.KullaniciId,
                Code = new Random().Next(100000, 999999).ToString(),
                Tamamlandi = false,
                OlusturanKullaniciId = kullanici.KullaniciId,
                OlusturmaTarihi = DateTime.Now,
                OlusturanIp = ipAddress,
                Telefon = kullanici.CepTelefonu,
                GirisGuid = Guid.NewGuid()
            }).Result;

            string smsMetin = $"Yerinde dönüşüm sistemine giriş yapabilmek için {kullaniciGirisKod.Code} kodunu kullanabilirsiniz.";

            var smsGonderildiMi = _smsService.SendSms(telefonNo, smsMetin, kullanici.KullaniciAdi, ipAddress).Result;
            
            if(smsGonderildiMi)
            {
                result.Result = new KullaniciGirisKodDto { GirisGuid = kullaniciGirisKod.GirisGuid.ToString() };
            }
            else
            {
                kullaniciGirisKod.SilindiMi = true;
                result.Exception(new NullReferenceException("Telefonunuza doğrulama kodu gönderilemedi, lütfen daha sonra tekrar deneyin."));
            }

            kullaniciGirisKod.SmsGonderildiMi = smsGonderildiMi;
            await _kullaniciGirisKodRepository.SaveChanges(cancellationToken);

            await _mediator.Send(new EkleKullaniciGirisBasariliCommand()
            {
                KullaniciAdi = kullanici.KullaniciAdi,
                Sifre = string.Concat(kullanici.KullaniciHesapTipId == 1 ? "LDAP doğrulamasından başarıyla geçildi" : "Local kullanıcı doğrulamasından başarıyla geçildi", ", Sms Gönderim Durumu: ", smsGonderildiMi),
            });

            #endregion

            return await Task.FromResult(result);
        }
    }
}