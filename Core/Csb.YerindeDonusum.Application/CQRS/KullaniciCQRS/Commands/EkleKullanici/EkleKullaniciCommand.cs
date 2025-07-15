using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.StringAddons;
using Csb.YerindeDonusum.Domain.Cryptography;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS.Commands.EkleKullanici;

public class EkleKullaniciCommand : IRequest<ResultModel<EkleKullaniciCommandResponseModel>>
{
    public string? KullaniciAdi { get; set; }
    public string? Sifre { get; set; }
    public long? KullaniciHesapTipId { get; set; }
    public long? BirimId { get; set; }
    public bool? AktifMi { get; set; }
    public long? TcKimlikNo { get; set; }
    public string? Eposta { get; set; }
    public string? CepTelefonu { get; set; }
    public string? Ad { get; set; }
    public string? Soyad { get; set; }
    public List<long>? SecilenRolIdList { get; set; }

    public class EkleKullaniciCommandHandler : IRequestHandler<EkleKullaniciCommand, ResultModel<EkleKullaniciCommandResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly ICacheService _cacheService;
        private readonly IMailService _emailService;
        public EkleKullaniciCommandHandler(IMapper mapper
            , IKullaniciRepository kullaniciRepository
            , ICacheService cacheService
            , IWebHostEnvironment webHostEnvironment
            , IKullaniciBilgi kullaniciBilgi
            , IMailService emailService)
        {
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _kullaniciRepository = kullaniciRepository;
            _kullaniciBilgi = kullaniciBilgi;
            _emailService = emailService;
        }

        public async Task<ResultModel<EkleKullaniciCommandResponseModel>> Handle(EkleKullaniciCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<EkleKullaniciCommandResponseModel>();

            var kullaniciMevcutMu = _kullaniciRepository.GetWhereEnumerable(x =>
                    x.SilindiMi == false
                    &&
                    (
                        StringAddons.NormalizeText(x.KullaniciAdi).Equals(StringAddons.NormalizeText(request.KullaniciAdi))
                        ||
                        x.TcKimlikNo == request.TcKimlikNo
                    )
                ).Any();

            if (kullaniciMevcutMu)
            {
                result.ErrorMessage("Bu TC kimlik numarasına ya da kullanıcı adına ait kullanıcı zaten mevcut.");
                return await Task.FromResult(result);
            }

            var kullanici = _mapper.Map<Kullanici>(request);
            long.TryParse(_kullaniciBilgi.GetUserInfo().KullaniciId, out long kullaniciId);
            var ipAdresi = _kullaniciBilgi.GetUserInfo().IpAdresi;
            string uretilenSifre = string.Empty;

            if (kullanici.KullaniciHesapTipId == (long)KullaniciHesapTipEnum.Local)
            {
                uretilenSifre = UserPasswordGenerator.GeneratePassword();
                kullanici.Sifre = CsbCryptography.Sha256(CsbCryptography.Sha256(CsbCryptography.MD5(uretilenSifre)));
                kullanici.SonSifreDegisimTarihi = DateTime.Now;
            }
            else
            {
                kullanici.Sifre = null;
            }

            kullanici.KullaniciAdi = kullanici.KullaniciAdi.Trim();
            kullanici.Eposta = kullanici.Eposta?.Trim().ToLowerInvariant();
            kullanici.CepTelefonu = StringAddon.ToClearPhone(kullanici.CepTelefonu);
            kullanici.SistemKullanicisiMi = false; //sistem kullanıcısı ise true olacak, bunu da db den manuel olarak yapıyoruz. edevlet, afad kullanıcıları vb.
            kullanici.OlusturanKullaniciId = kullaniciId;
            kullanici.OlusturanIp = ipAdresi;

            foreach (var rolId in request.SecilenRolIdList.Distinct())
            {
                kullanici.KullaniciRols.Add(new KullaniciRol
                {
                    RolId = rolId,
                    OlusturanKullaniciId = kullaniciId,
                    OlusturmaTarihi = DateTime.Now,
                    OlusturanIp = ipAdresi
                });
            }

            await _kullaniciRepository.AddAsync(kullanici);
            await _kullaniciRepository.SaveChanges(cancellationToken);

            string eklemeMesaji = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.";

            if (!string.IsNullOrWhiteSpace(uretilenSifre))
            {
                var mailGonderim = _emailService
                                    .SendMail(
                                        kullanici.Eposta!,
                                        "Yerinde Dönüşüm Kullanıcı Bilgileri",
                                        @$"Merhaba <b>{kullanici.Ad} {kullanici.Soyad}</b>, <br />
                                        Yerinde dönüşüm kullanıcınız başarıyla oluşturulmuştur. <br />
                                        Kullanıcı Adınız: <b>{kullanici.KullaniciAdi}</b> <br />
                                        Şifreniz: <b>{uretilenSifre}</b>"
                                    );

                if (mailGonderim.IsError)
                    eklemeMesaji = "Kullanıcı başarıyla eklendi fakat şifre bilgisi eposta yoluyla gönderilemedi.";
                else
                    eklemeMesaji = "Kullanıcı başarıyla eklenerek şifre bilgisi eposta yoluyla paylaşıldı.";
            }

            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirKullaniciListeQuery)}";
            await _cacheService.Clear(cacheKey);

            result.Result = new EkleKullaniciCommandResponseModel
            {
                Mesaj = eklemeMesaji
            };

            return await Task.FromResult(result);
        }
    }
}