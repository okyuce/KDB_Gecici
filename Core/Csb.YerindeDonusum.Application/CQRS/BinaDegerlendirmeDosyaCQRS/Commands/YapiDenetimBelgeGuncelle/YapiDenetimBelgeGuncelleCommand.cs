using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeDosyaCQRS.Commands.YapiDenetimBelgeGuncelle;

public class YapiDenetimBelgeGuncelleCommand : IRequest<ResultModel<YapiDenetimBelgeGuncelleCommandResponseModel>>
{
    public Guid? BinaYapiDenetimDosyaGuid { get; set; }
    public DosyaDto? YapiDenetimIlerlemeBelgesi { get; set; }

    public class YapiDenetimBelgeGuncelleCommandHandler : IRequestHandler<YapiDenetimBelgeGuncelleCommand, ResultModel<YapiDenetimBelgeGuncelleCommandResponseModel>>
    {
        private readonly IBinaYapiDenetimSeviyeTespitDosyaRepository _binaYapiDenetimSeviyeTespitDosyaRepository;
        private readonly IConfiguration _configuration;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public YapiDenetimBelgeGuncelleCommandHandler(IBasvuruRepository basvuruRepository
            , IBinaDegerlendirmeRepository binaDegerlendirmeRepository
            , IKullaniciBilgi kullaniciBilgi
            , IConfiguration configuration
            , IBinaYapiDenetimSeviyeTespitDosyaRepository binaYapiDenetimSeviyeTespitDosyaRepository)
        {
            _configuration = configuration;
            _kullaniciBilgi = kullaniciBilgi;
            _binaYapiDenetimSeviyeTespitDosyaRepository = binaYapiDenetimSeviyeTespitDosyaRepository;
        }

        public async Task<ResultModel<YapiDenetimBelgeGuncelleCommandResponseModel>> Handle(YapiDenetimBelgeGuncelleCommand request, CancellationToken cancellationToken)
        {
            ResultModel<YapiDenetimBelgeGuncelleCommandResponseModel> result = new();

            var binaYapiDenetimSeviyeTespitDosya = await _binaYapiDenetimSeviyeTespitDosyaRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                && x.BinaYapiDenetimSeviyeTespitDosyaGuid == request.BinaYapiDenetimDosyaGuid
                                                , asNoTracking: true                                                
                                            ).FirstOrDefaultAsync();

            if (binaYapiDenetimSeviyeTespitDosya == null)
            {
                result.ErrorMessage("Bina yapı denetim dosya bilgisi bulunamadı.");
                return await Task.FromResult(result);
            }

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            long.TryParse(kullaniciBilgi.KullaniciId, out long kullaniciId);

            binaYapiDenetimSeviyeTespitDosya ??= new BinaYapiDenetimSeviyeTespitDosya
            {
                AktifMi = true,
                SilindiMi = false,
            };
           
            if (request.YapiDenetimIlerlemeBelgesi != null)
            {
                binaYapiDenetimSeviyeTespitDosya.DosyaAdi = string.Concat(Guid.NewGuid(), request.YapiDenetimIlerlemeBelgesi?.DosyaUzanti);
                binaYapiDenetimSeviyeTespitDosya.DosyaYolu = DateTime.Now.ToString("yyyy-MM-dd");
                binaYapiDenetimSeviyeTespitDosya.DosyaTuru = MimeTypes.GetMimeType(request.YapiDenetimIlerlemeBelgesi?.DosyaUzanti ?? "");

                byte[] data = Convert.FromBase64String(request.YapiDenetimIlerlemeBelgesi?.DosyaBase64);

                if (!FileTypeVerifier.Verify(data).IsVerified)
                {
                    result.ErrorMessage("Geçersiz veya Hatalı Dosya Uzantısı.");
                    return await Task.FromResult(result);
                }

                string uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", binaYapiDenetimSeviyeTespitDosya.DosyaYolu);

                if (!Directory.Exists(uploadDirectoryPath))
                    Directory.CreateDirectory(uploadDirectoryPath);

                string filePath = string.Concat(uploadDirectoryPath, "\\", binaYapiDenetimSeviyeTespitDosya.DosyaAdi);
                using FileStream stream = File.Create(filePath);
                stream.Write(data, 0, data.Length);

            }

            binaYapiDenetimSeviyeTespitDosya.GuncelleyenKullaniciId = kullaniciId;
            binaYapiDenetimSeviyeTespitDosya.GuncellemeTarihi = DateTime.Now;
            binaYapiDenetimSeviyeTespitDosya.GuncelleyenIp = kullaniciBilgi.IpAdresi;

            _binaYapiDenetimSeviyeTespitDosyaRepository.Update(binaYapiDenetimSeviyeTespitDosya);
            await _binaYapiDenetimSeviyeTespitDosyaRepository.SaveChanges();

            result.Result = new YapiDenetimBelgeGuncelleCommandResponseModel
            {
                Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.",
                BinaDosyaGuid = binaYapiDenetimSeviyeTespitDosya.BinaYapiDenetimSeviyeTespitDosyaGuid,
                DosyaAdi = binaYapiDenetimSeviyeTespitDosya.DosyaAdi,
            };

            return await Task.FromResult(result);
        }
    }
}