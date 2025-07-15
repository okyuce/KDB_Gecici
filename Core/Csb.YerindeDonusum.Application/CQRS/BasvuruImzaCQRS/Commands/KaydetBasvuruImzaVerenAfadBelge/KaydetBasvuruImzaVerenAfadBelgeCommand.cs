using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVerenAfadBelge;

public class KaydetBasvuruImzaVerenAfadBelgeCommand : IRequest<ResultModel<string>>
{
    public long? BasvuruId { get; set; }
    public DosyaDto? AfadBelge { get; set; }

    public class KaydetBasvuruImzaVerenAfadBelgeCommandHandler : IRequestHandler<KaydetBasvuruImzaVerenAfadBelgeCommand, ResultModel<string>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IConfiguration _configuration;

        public KaydetBasvuruImzaVerenAfadBelgeCommandHandler(IMapper mapper, IConfiguration configuration, IBasvuruRepository basvuruRepository, IKullaniciBilgi kullaniciBilgi)
        {
            _mapper = mapper;
            _configuration = configuration;
            _basvuruRepository = basvuruRepository;
            _kullaniciBilgi = kullaniciBilgi;
        }

        public async Task<ResultModel<string>> Handle(KaydetBasvuruImzaVerenAfadBelgeCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            var basvuru = _basvuruRepository
                                        .GetWhere(x =>
                                            !x.SilindiMi
                                            &&
                                            x.AktifMi == true
                                            &&
                                            x.BasvuruId == request.BasvuruId
                                            &&
                                            x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                            &&
                                            x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir,
                                            true,
                                            i => i.BasvuruDosyas
                                        )
                                        .FirstOrDefault();

            if (basvuru == null)
            {
                result.ErrorMessage("Başvuru bilgisi bulunamadı!");
                return await Task.FromResult(result);
            }
            else if (basvuru?.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.Kabul)
            {
                result.ErrorMessage("Yalnızca AFAD durumu kabul olan başvuru için işlem yapılabilir!");
                return await Task.FromResult(result);
            }

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            long.TryParse(kullaniciBilgi.KullaniciId, out long kullaniciId);

            var dateTimeNow = DateTime.Now;

            byte[] data = Convert.FromBase64String(request?.AfadBelge?.DosyaBase64);
            var isTheFileTypeAllowed = FileTypeVerifier.Verify(data);
            if (isTheFileTypeAllowed.IsVerified)
            {
                var DosyaAdi = string.Concat(Guid.NewGuid(), request?.AfadBelge?.DosyaUzanti);
                var DosyaTuru = MimeTypes.GetMimeType(request?.AfadBelge?.DosyaUzanti);
                var DosyaYolu = dateTimeNow.ToString("yyyy-MM-dd");

                basvuru.BasvuruDosyas.Add(new BasvuruDosya()
                {
                    BasvuruId = basvuru.BasvuruId,
                    BasvuruDosyaTurId = BasvuruDosyaTurEnum.AfadDurumGuncellemeBelgesi,
                    DosyaAdi = DosyaAdi,
                    DosyaTuru = DosyaTuru,
                    DosyaYolu = DosyaYolu,
                    OlusturanKullaniciId = kullaniciId,
                    OlusturanIp = kullaniciBilgi.IpAdresi,
                    OlusturmaTarihi = dateTimeNow,
                    AktifMi = true,
                    SilindiMi = false,
                });

                var uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", DosyaYolu);
                if (!Directory.Exists(uploadDirectoryPath))
                    Directory.CreateDirectory(uploadDirectoryPath);
                var filePath = string.Concat(uploadDirectoryPath, "\\", DosyaAdi);
                using var stream = File.Create(filePath);
                stream.Write(data, 0, data.Length);
            }

            basvuru.BasvuruAfadDurumId = (long)BasvuruAfadDurumEnum.IptalEdilmistir;
            basvuru.BasvuruAfadDurumGuncellemeTarihi = dateTimeNow;

            _basvuruRepository.Update(basvuru);

            await _basvuruRepository.SaveChanges();

            result.Result = "İşlem başarılı";

            return await Task.FromResult(result);
        }
    }
}