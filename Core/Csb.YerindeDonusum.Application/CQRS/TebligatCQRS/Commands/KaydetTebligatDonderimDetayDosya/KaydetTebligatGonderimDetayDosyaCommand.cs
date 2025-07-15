using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Commands.KaydetTebligatDonderimDetayDosya;

public class KaydetTebligatGonderimDetayDosyaCommand : IRequest<ResultModel<TebligatGonderimDetayDosyaDto>>
{
    public long TebligatGonderimDetayId { get; set; }
    public DosyaDto? TebligatSozlesmesi { get; set; }

    public class KaydetTebligatGonderimDetayDosyaCommandHandler : IRequestHandler<KaydetTebligatGonderimDetayDosyaCommand, ResultModel<TebligatGonderimDetayDosyaDto>>
    {
        private readonly IMapper _mapper;
        private readonly ITebligatGonderimDetayRepository _tebligatGonderimDetayRepository;
        private readonly ITebligatGonderimDetayDosyaRepository _tebligatGonderimDetayDosyaRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IConfiguration _configuration;

        public KaydetTebligatGonderimDetayDosyaCommandHandler(IMapper mapper, IConfiguration configuration, ITebligatGonderimDetayDosyaRepository tebligatGonderimDetayDosyaRepository, ITebligatGonderimDetayRepository tebligatGonderimDetayRepository, IKullaniciBilgi kullaniciBilgi)
        {
            _mapper = mapper;
            _configuration = configuration;
            _tebligatGonderimDetayDosyaRepository = tebligatGonderimDetayDosyaRepository;
            _tebligatGonderimDetayRepository = tebligatGonderimDetayRepository;
            _kullaniciBilgi = kullaniciBilgi;
        }

        public async Task<ResultModel<TebligatGonderimDetayDosyaDto>> Handle(KaydetTebligatGonderimDetayDosyaCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<TebligatGonderimDetayDosyaDto>();

            var tebligatGonderimDetay = _tebligatGonderimDetayRepository.GetWhere(
                x => x.TebligatGonderimDetayId == request.TebligatGonderimDetayId,
                true,
                t => t.TebligatGonderimDetayDosyas.Where(d => !d.SilindiMi && d.AktifMi == true))
                .FirstOrDefault();

            var tebligatGonderimDetayDosya = tebligatGonderimDetay.TebligatGonderimDetayDosyas.FirstOrDefault();

            if (tebligatGonderimDetay == null)
            {
                result.ErrorMessage("Tebligat gönderim detay bilgisi bulunamadı!");
                return await Task.FromResult(result);
            }

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            long.TryParse(kullaniciBilgi.KullaniciId, out long kullaniciId);

            var dateTimeNow = DateTime.Now;

            if (FluentValidationExtension.NotEmpty(request?.TebligatSozlesmesi))
            {
                byte[] data = Convert.FromBase64String(request?.TebligatSozlesmesi?.DosyaBase64);
                var isTheFileTypeAllowed = FileTypeVerifier.Verify(data);
                if (isTheFileTypeAllowed.IsVerified)
                {
                    var DosyaAdi = string.Concat(Guid.NewGuid(), request?.TebligatSozlesmesi?.DosyaUzanti);
                    var DosyaTuru = MimeTypes.GetMimeType(request?.TebligatSozlesmesi?.DosyaUzanti);
                    var DosyaYolu = dateTimeNow.ToString("yyyy-MM-dd");

                    if (tebligatGonderimDetayDosya != null)
                    {
                        tebligatGonderimDetayDosya.AktifMi = false;
                        tebligatGonderimDetayDosya.SilindiMi = true;
                        _tebligatGonderimDetayDosyaRepository.Update(tebligatGonderimDetayDosya);
                    }
                    tebligatGonderimDetayDosya = new TebligatGonderimDetayDosya()
                    {
                        TebligatGonderimDetayId = request.TebligatGonderimDetayId,
                        DosyaAdi = DosyaAdi,
                        DosyaTuru = DosyaTuru,
                        DosyaYolu = DosyaYolu,
                        OlusturanKullaniciId = kullaniciId,
                        OlusturanIp = kullaniciBilgi.IpAdresi,
                        OlusturmaTarihi = dateTimeNow,
                        AktifMi = true,
                        SilindiMi = false
                    };
                    var uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", DosyaYolu);
                    if (!Directory.Exists(uploadDirectoryPath))
                        Directory.CreateDirectory(uploadDirectoryPath);
                    var filePath = string.Concat(uploadDirectoryPath, "\\", DosyaAdi);
                    using var stream = File.Create(filePath);
                    stream.Write(data, 0, data.Length);
                }
            }

            await _tebligatGonderimDetayDosyaRepository.AddAsync(tebligatGonderimDetayDosya);

            await _tebligatGonderimDetayDosyaRepository.SaveChanges();

            result.Result = _mapper.Map<TebligatGonderimDetayDosyaDto>(tebligatGonderimDetayDosya);

            return await Task.FromResult(result);
        }
    }
}
