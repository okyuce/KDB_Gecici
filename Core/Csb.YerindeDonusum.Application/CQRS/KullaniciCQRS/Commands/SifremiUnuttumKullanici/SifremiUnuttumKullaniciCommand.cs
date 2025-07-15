using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Cryptography;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS.Commands
{
    public class SifremiUnuttumKullaniciCommand : IRequest<ResultModel<SifremiUnuttumKullaniciCommandResponseModel>>
    {
        public string? KullaniciAdi { get; set; }
        public long? TcKimlikNo { get; set; }
        public string? Eposta { get; set; }
        public string? GuvenlikKodu { get; set; }
        public string? GuvenlikKoduKey { get; set; }

        public class SifremiUnuttumKullaniciCommandHandler : IRequestHandler<SifremiUnuttumKullaniciCommand, ResultModel<SifremiUnuttumKullaniciCommandResponseModel>>
        {
            private readonly IKullaniciRepository _kullaniciRepository;
            private readonly IKullaniciBilgi _kullaniciBilgi;
            private readonly IMailService _emailService;

            public SifremiUnuttumKullaniciCommandHandler(IKullaniciRepository kullaniciRepository, IKullaniciBilgi kullaniciBilgi, IMailService emailService)
            {
                _kullaniciRepository = kullaniciRepository;
                _kullaniciBilgi = kullaniciBilgi;
                _emailService = emailService;
            }

            public async Task<ResultModel<SifremiUnuttumKullaniciCommandResponseModel>> Handle(SifremiUnuttumKullaniciCommand request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<SifremiUnuttumKullaniciCommandResponseModel>();

                var kullanici = await _kullaniciRepository
                                        .GetWhere(x =>
                                            !x.SilindiMi
                                            &&
                                            x.KullaniciAdi == request.KullaniciAdi!.Trim()
                                            &&
                                            x.TcKimlikNo == request.TcKimlikNo
                                            &&
                                            x.Eposta == request.Eposta!.Trim()
                                        ).FirstOrDefaultAsync();

                if (kullanici == null)
                {
                    result.ErrorMessage("Girdiğiniz bilgilerle eşleşen bir kayıt bulunamadı.");
                    return await Task.FromResult(result);
                }

                if (kullanici.KullaniciHesapTipId != (long)KullaniciHesapTipEnum.Local)
                {
                    result.ErrorMessage("Kullanıcı hesap tipi bu işlemi yapmaya uygun değildir.");
                    return await Task.FromResult(result);
                }

                if (kullanici.SonSifreDegisimTarihi != null && kullanici.SonSifreDegisimTarihi.HasValue)
                {
                    // Farkı 10 dakikadan az olup olmadığını kontrol et
                    if ((DateTime.Now - kullanici.SonSifreDegisimTarihi.Value).TotalMinutes < 10)
                    {
                        result.ErrorMessage("Yakın zamanda şifrenizi değiştirdiğiniz için işlem yapılamıyor. Lütfen daha sonra tekrar deneyiniz!");
                        return await Task.FromResult(result);
                    }
                }

                string uretilenSifre = UserPasswordGenerator.GeneratePassword();

                var emailResult = _emailService.SendMail(
                    kullanici.Eposta!,
                    "Yerinde Dönüşüm Şifre Sıfırlama",
                    @$"Merhaba <b>{kullanici.Ad} {kullanici.Soyad}</b>, <br />
                                Yerinde dönüşüm kullanıcı şifreniz başarıyla sıfırlanmıştır.<br />
                                Kullanıcı Adı: <b>{kullanici.KullaniciAdi}</b> <br />
                                Şifre: <b>{uretilenSifre}</b>"
                    );

                if (emailResult.IsError)
                {
                    result.ErrorMessage("Eposta gönderimi yapılamadığı için işleminiz gerçekleştirilemiyor. Lütfen daha sonra tekrar deneyiniz!");
                    return await Task.FromResult(result);
                }

                kullanici.Sifre = CsbCryptography.Sha256(CsbCryptography.Sha256(CsbCryptography.MD5(uretilenSifre)));

                kullanici.SonSifreDegisimTarihi = DateTime.Now;

                var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();

                long.TryParse(kullaniciBilgi.KullaniciId, out long kullaniciId);

                kullanici.GuncelleyenKullaniciId = kullaniciId;
                kullanici.GuncelleyenIp = kullaniciBilgi.IpAdresi;
                kullanici.GuncellemeTarihi = DateTime.Now;

                await _kullaniciRepository.SaveChanges(cancellationToken);

                result.Result = new SifremiUnuttumKullaniciCommandResponseModel
                {
                    Mesaj = "Yeni şifreniz eposta adresinize gönderilmiştir.",
                };

                return await Task.FromResult(result);
            }
        }
    }
}