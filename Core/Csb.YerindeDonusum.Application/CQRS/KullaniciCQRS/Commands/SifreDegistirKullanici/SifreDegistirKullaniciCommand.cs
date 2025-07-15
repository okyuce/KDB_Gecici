using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.StringAddons;
using Csb.YerindeDonusum.Domain.Cryptography;
using Csb.YerindeDonusum.Domain.Entities;
using CSB.Core.Utilities.Messaging;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS.Commands
{
    public class SifreDegistirKullaniciCommand : IRequest<ResultModel<SifreDegistirKullaniciCommandResponseModel>>
    {
        public string? MevcutSifre { get; set; }
        public string? YeniSifre { get; set; }
        public string? YeniSifreYeniden { get; set; }

        public class SifreDegistirKullaniciCommandHandler : IRequestHandler<SifreDegistirKullaniciCommand, ResultModel<SifreDegistirKullaniciCommandResponseModel>>
        {
            private readonly IMapper _mapper;
            private readonly IKullaniciRepository _kullaniciRepository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly IKullaniciBilgi _kullaniciBilgi;
            private readonly ICacheService _cacheService;

            public SifreDegistirKullaniciCommandHandler(IMapper mapper
                , IKullaniciRepository kullaniciRepository,
                IWebHostEnvironment webHostEnvironment,
                ICacheService cacheService,
                IKullaniciBilgi kullaniciBilgi)
            {
                _cacheService = cacheService;
                _webHostEnvironment = webHostEnvironment;
                _mapper = mapper;
                _kullaniciRepository = kullaniciRepository;
                _kullaniciBilgi = kullaniciBilgi;
            }

            public async Task<ResultModel<SifreDegistirKullaniciCommandResponseModel>> Handle(SifreDegistirKullaniciCommand request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<SifreDegistirKullaniciCommandResponseModel>();

                var kullanici = await _kullaniciRepository.GetWhere(x =>
                    x.SilindiMi == false
                    &&
                    x.KullaniciId == Convert.ToInt64(_kullaniciBilgi.GetUserInfo().KullaniciId)                    
                ).FirstOrDefaultAsync();

                if (kullanici == null)
                {
                    result.ErrorMessage("Kullanıcı veritabanında bulunamadı.");
                    return await Task.FromResult(result);
                }
                if (kullanici.KullaniciHesapTipId != (long)KullaniciHesapTipEnum.Local)
                {
                    result.ErrorMessage("Kullanıcı hesap tipi bu işlemi yapmaya uygun değildir.");
                    return await Task.FromResult(result);
                }

                if (kullanici == null)
                {
                    result.ErrorMessage("Kullanıcı veritabanında bulunamadı.");
                    return await Task.FromResult(result);
                }

                if(kullanici.Sifre != CsbCryptography.Sha256(CsbCryptography.Sha256(CsbCryptography.MD5(request.MevcutSifre))))
                {
                    result.ErrorMessage("Mevcut şifreniz hatalı.");
                    return await Task.FromResult(result);
                }

                if(request.YeniSifre != request.YeniSifreYeniden)
                {
                    result.ErrorMessage("Yeni şifre bilgileri eşleşmiyor.");
                    return await Task.FromResult(result);
                }

                kullanici.Sifre = CsbCryptography.Sha256(CsbCryptography.Sha256(CsbCryptography.MD5(request.YeniSifre)));

                kullanici.SonSifreDegisimTarihi = DateTime.Now;

                long.TryParse(_kullaniciBilgi.GetUserInfo().KullaniciId, out long kullaniciId);
                var ipAdresi = _kullaniciBilgi.GetUserInfo().IpAdresi;

                kullanici.GuncelleyenKullaniciId = kullaniciId;
                kullanici.GuncelleyenIp = ipAdresi;
                kullanici.GuncellemeTarihi = DateTime.Now;

                await _kullaniciRepository.SaveChanges(cancellationToken);

                result.Result = new SifreDegistirKullaniciCommandResponseModel
                {
                    Mesaj = "Yeni şifreniz başarıyla değiştirilmiştir.",
                };

                return await Task.FromResult(result);
            }
        }
    }
}