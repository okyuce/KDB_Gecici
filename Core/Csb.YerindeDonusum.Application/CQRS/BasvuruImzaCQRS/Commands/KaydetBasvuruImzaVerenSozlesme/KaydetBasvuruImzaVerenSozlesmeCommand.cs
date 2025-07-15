using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVerenSozlesme;

public class KaydetBasvuruImzaVerenSozlesmeCommand : IRequest<ResultModel<BasvuruImzaVerenDto>>
{
    public long? BasvuruImzaVerenId { get; set; }
    public DosyaDto? KrediSozlesmesi { get; set; }
    public DosyaDto? HibeSozlesmesi { get; set; }

    public class KaydetBasvuruImzaVerenSozlesmeCommandHandler : IRequestHandler<KaydetBasvuruImzaVerenSozlesmeCommand, ResultModel<BasvuruImzaVerenDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruImzaVerenRepository _basvuruImzaVerenRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IConfiguration _configuration;

        public KaydetBasvuruImzaVerenSozlesmeCommandHandler(IMapper mapper, IConfiguration configuration, IBasvuruImzaVerenRepository basvuruImzaVerenRepository, IKullaniciBilgi kullaniciBilgi)
        {
            _mapper = mapper;
            _configuration = configuration;
            _basvuruImzaVerenRepository = basvuruImzaVerenRepository;
            _kullaniciBilgi = kullaniciBilgi;
        }

        public async Task<ResultModel<BasvuruImzaVerenDto>> Handle(KaydetBasvuruImzaVerenSozlesmeCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<BasvuruImzaVerenDto>();

            var basvuruImzaVeren = _basvuruImzaVerenRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                            && x.BasvuruImzaVerenId == request.BasvuruImzaVerenId
                                        , true
                                        , i => i.BasvuruImzaVerenDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true)
                                    ).FirstOrDefault();

            if (basvuruImzaVeren == null)
            {
                result.ErrorMessage("Başvuru imza Veren bilgisi bulunamadı!");
                return await Task.FromResult(result);
            }

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            long.TryParse(kullaniciBilgi.KullaniciId, out long kullaniciId);

            var dateTimeNow = DateTime.Now;

            if (FluentValidationExtension.NotEmpty(request?.KrediSozlesmesi))
            {
                byte[] data = Convert.FromBase64String(request?.KrediSozlesmesi?.DosyaBase64);
                var isTheFileTypeAllowed = FileTypeVerifier.Verify(data);
                if (isTheFileTypeAllowed.IsVerified)
                {
                    var krediSozlesmesiDosya = basvuruImzaVeren.BasvuruImzaVerenDosyas.FirstOrDefault(x =>
                                                                    x.SilindiMi == false
                                                                    &&
                                                                    x.AktifMi == true
                                                                    &&
                                                                    x.BasvuruImzaVerenDosyaTurId == (long)BasvuruImzaVerenDosyaTurEnum.KrediSozlesmesi
                                                                );

                    var DosyaAdi = string.Concat(Guid.NewGuid(), request?.KrediSozlesmesi?.DosyaUzanti);
                    var DosyaTuru = MimeTypes.GetMimeType(request?.KrediSozlesmesi?.DosyaUzanti);
                    var DosyaYolu = dateTimeNow.ToString("yyyy-MM-dd");

                    if (krediSozlesmesiDosya == null)
                    {
                        krediSozlesmesiDosya = new BasvuruImzaVerenDosya()
                        {
                            BasvuruImzaVerenDosyaTurId = (long)BasvuruImzaVerenDosyaTurEnum.KrediSozlesmesi,
                            DosyaAdi = DosyaAdi,
                            DosyaTuru = DosyaTuru,
                            DosyaYolu = DosyaYolu,
                            OlusturanKullaniciId = kullaniciId,
                            OlusturanIp = kullaniciBilgi.IpAdresi,
                            OlusturmaTarihi = dateTimeNow,
                            AktifMi = true,
                            SilindiMi = false,
                        };

                        basvuruImzaVeren.BasvuruImzaVerenDosyas.Add(krediSozlesmesiDosya);
                    }
                    else
                    {
                        krediSozlesmesiDosya.DosyaAdi = DosyaAdi;
                        krediSozlesmesiDosya.DosyaTuru = DosyaTuru;
                        krediSozlesmesiDosya.DosyaYolu = DosyaYolu;
                        krediSozlesmesiDosya.GuncellemeTarihi = dateTimeNow;
                        krediSozlesmesiDosya.GuncelleyenKullaniciId = kullaniciId;
                        krediSozlesmesiDosya.GuncelleyenIp = kullaniciBilgi.IpAdresi;
                    }

                    var uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", DosyaYolu);
                    if (!Directory.Exists(uploadDirectoryPath))
                        Directory.CreateDirectory(uploadDirectoryPath);
                    var filePath = string.Concat(uploadDirectoryPath, "\\", DosyaAdi);
                    using var stream = File.Create(filePath);
                    stream.Write(data, 0, data.Length);
                }
            }

            if (FluentValidationExtension.NotEmpty(request?.HibeSozlesmesi))
            {
                byte[] data = Convert.FromBase64String(request?.HibeSozlesmesi?.DosyaBase64);
                var isTheFileTypeAllowed = FileTypeVerifier.Verify(data);
                if (isTheFileTypeAllowed.IsVerified)
                {
                    var hibeOnayiDosya = basvuruImzaVeren.BasvuruImzaVerenDosyas.FirstOrDefault(x =>
                                                            x.SilindiMi == false
                                                            &&
                                                            x.AktifMi == true
                                                            &&
                                                            x.BasvuruImzaVerenDosyaTurId == (long)BasvuruImzaVerenDosyaTurEnum.HibeOnayi
                                                        );

                    var DosyaAdi = string.Concat(Guid.NewGuid(), request?.HibeSozlesmesi?.DosyaUzanti);
                    var DosyaTuru = MimeTypes.GetMimeType(request?.HibeSozlesmesi?.DosyaUzanti);
                    var DosyaYolu = DateTime.Now.ToString("yyyy-MM-dd");

                    if (hibeOnayiDosya == null)
                    {
                        hibeOnayiDosya = new BasvuruImzaVerenDosya()
                        {
                            BasvuruImzaVerenDosyaTurId = (long)BasvuruImzaVerenDosyaTurEnum.HibeOnayi,
                            DosyaAdi = DosyaAdi,
                            DosyaTuru = DosyaTuru,
                            DosyaYolu = DosyaYolu,
                            OlusturanKullaniciId = kullaniciId,
                            OlusturanIp = kullaniciBilgi.IpAdresi,
                            OlusturmaTarihi = dateTimeNow,
                            AktifMi = true,
                            SilindiMi = false,
                        };

                        basvuruImzaVeren.BasvuruImzaVerenDosyas.Add(hibeOnayiDosya);
                    }
                    else
                    {
                        hibeOnayiDosya.DosyaAdi = DosyaAdi;
                        hibeOnayiDosya.DosyaTuru = DosyaTuru;
                        hibeOnayiDosya.DosyaYolu = DosyaYolu;
                        hibeOnayiDosya.GuncellemeTarihi = dateTimeNow;
                        hibeOnayiDosya.GuncelleyenKullaniciId = kullaniciId;
                        hibeOnayiDosya.GuncelleyenIp = kullaniciBilgi.IpAdresi;
                    }

                    var uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", DosyaYolu);
                    if (!Directory.Exists(uploadDirectoryPath))
                        Directory.CreateDirectory(uploadDirectoryPath);
                    var filePath = string.Concat(uploadDirectoryPath, "\\", DosyaAdi);
                    using var stream = File.Create(filePath);
                    stream.Write(data, 0, data.Length);
                }
            }

            _basvuruImzaVerenRepository.Update(basvuruImzaVeren);

            await _basvuruImzaVerenRepository.SaveChanges();

            result.Result = _mapper.Map<BasvuruImzaVerenDto>(basvuruImzaVeren);

            return await Task.FromResult(result);
        }
    }
}