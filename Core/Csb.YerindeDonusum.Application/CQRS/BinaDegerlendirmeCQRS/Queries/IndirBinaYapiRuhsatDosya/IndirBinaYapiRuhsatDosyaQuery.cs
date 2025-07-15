using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.IndirBinaYapiRuhsatDosya;

public class IndirBinaYapiRuhsatDosyaQuery : IRequest<ResultModel<IndirBinaYapiRuhsatDosyaQueryResponseModel>>
{
    public IndirBinaYapiRuhsatDosyaQueryModel? Model { get; set; }

    public class IndirBinaYapiRuhsatDosyaQueryHandler : IRequestHandler<IndirBinaYapiRuhsatDosyaQuery, ResultModel<IndirBinaYapiRuhsatDosyaQueryResponseModel>>
    {
        private readonly IBinaYapiRuhsatIzinDosyaRepository _binaYapiRuhsatIzinDosyaRepository;
        private readonly IConfiguration _configuration;

        public IndirBinaYapiRuhsatDosyaQueryHandler(IBinaYapiRuhsatIzinDosyaRepository binaYapiRuhsatIzinDosyaRepository, IConfiguration configuration)
        {
            _binaYapiRuhsatIzinDosyaRepository = binaYapiRuhsatIzinDosyaRepository;
            _configuration = configuration;
        }

        public async Task<ResultModel<IndirBinaYapiRuhsatDosyaQueryResponseModel>> Handle(IndirBinaYapiRuhsatDosyaQuery request, CancellationToken cancellationToken)
        {
            ResultModel<IndirBinaYapiRuhsatDosyaQueryResponseModel> result = new ResultModel<IndirBinaYapiRuhsatDosyaQueryResponseModel>();
            try
            {
                Guid.TryParse(request?.Model?.BinaDosyaGuid, out Guid binaDosyaGuid);

                BinaYapiRuhsatIzinDosya? binaYapiRuhsatIzinDosyaRepository = _binaYapiRuhsatIzinDosyaRepository
                                             .GetWhere(x => x.BinaYapiRuhsatIzinDosyaGuid == binaDosyaGuid
                                                         && x.AktifMi == true
                                                         && x.SilindiMi == false)
                                             .AsNoTracking()
                                             .FirstOrDefault();

                if (binaYapiRuhsatIzinDosyaRepository != null)
                {
                    string? fileDiskPath = _configuration.GetSection("UploadFile:Path").Value;

                    string fullFilePath = Path.Combine(fileDiskPath!, binaYapiRuhsatIzinDosyaRepository.DosyaYolu!, binaYapiRuhsatIzinDosyaRepository.DosyaAdi);

                    //Sunucu Üzerindeki Dosya Var mı Yok mu Kontrolü
                    if (!File.Exists(fullFilePath))
                    {
                        result.Exception(new FileNotFoundException("Sunucuda dosya bulunamadı."), "Sunucuda dosya bulunamadı.");

                        return await Task.FromResult(result);
                    }

                    using FileStream fs = new FileStream(fullFilePath, FileMode.Open);
                    using MemoryStream ms = new MemoryStream();

                    fs.CopyTo(ms);

                    result.Result = new IndirBinaYapiRuhsatDosyaQueryResponseModel()
                    {
                        DosyaAdi = binaYapiRuhsatIzinDosyaRepository.DosyaAdi,
                        File = ms.ToArray(),
                        MimeType = MimeTypes.GetMimeType(Path.GetExtension(binaYapiRuhsatIzinDosyaRepository.DosyaAdi))
                    };
                }
                else
                {
                    result.ErrorMessage("Dosya Bilgileri Bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                result.Exception(ex, "Dosya Bilgiler Alınırken Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}