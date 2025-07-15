using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.IndirYapiDenetimDosya;

public class IndirYapiDenetimDosyaQuery : IRequest<ResultModel<IndirYapiDenetimDosyaQueryResponseModel>>
{
    public IndirYapiDenetimDosyaQueryModel? Model { get; set; }

    public class IndirYapiDenetimDosyaQueryHandler : IRequestHandler<IndirYapiDenetimDosyaQuery, ResultModel<IndirYapiDenetimDosyaQueryResponseModel>>
    {
        private readonly IBinaYapiDenetimSeviyeTespitDosyaRepository _binaYapiDenetimSeviyeTespitDosyaRepository;
        private readonly IConfiguration _configuration;

        public IndirYapiDenetimDosyaQueryHandler(IConfiguration configuration, IBinaYapiDenetimSeviyeTespitDosyaRepository binaYapiDenetimSeviyeTespitDosyaRepository)
        {
            _configuration = configuration;
            _binaYapiDenetimSeviyeTespitDosyaRepository = binaYapiDenetimSeviyeTespitDosyaRepository;
        }

        public async Task<ResultModel<IndirYapiDenetimDosyaQueryResponseModel>> Handle(IndirYapiDenetimDosyaQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<IndirYapiDenetimDosyaQueryResponseModel>();

            Guid.TryParse(request?.Model?.BinaYapiDenetimDosyaGuid, out Guid binaYapiDenetimSeviyeTespitDosyaGuid);

            var binaYapiDenetimSeviyeTespit = _binaYapiDenetimSeviyeTespitDosyaRepository
                                                .GetWhere(x =>
                                                    x.BinaYapiDenetimSeviyeTespitDosyaGuid == binaYapiDenetimSeviyeTespitDosyaGuid
                                                    &&
                                                    x.AktifMi == true
                                                    &&
                                                    x.SilindiMi == false,
                                                    asNoTracking: true
                                                )
                                                .FirstOrDefault();

            if (binaYapiDenetimSeviyeTespit == null)
            {
                result.ErrorMessage("Başvuru Detayı Bulunamadı.");
                return await Task.FromResult(result);
            }

            string? fileDiskPath = _configuration.GetSection("UploadFile:Path").Value;

            string fullFilePath = Path.Combine(fileDiskPath!, binaYapiDenetimSeviyeTespit.DosyaYolu!, binaYapiDenetimSeviyeTespit.DosyaAdi);

            //Sunucu Üzerindeki Dosya Var mı Yok mu Kontrolü
            if (!File.Exists(fullFilePath))
            {
                result.Exception(new FileNotFoundException("Sunucuda dosya bulunamadı."), "Sunucuda dosya bulunamadı.");
                return await Task.FromResult(result);
            }

            using var fs = new FileStream(fullFilePath, FileMode.Open);
            using var ms = new MemoryStream();

            fs.CopyTo(ms);

            result.Result = new IndirYapiDenetimDosyaQueryResponseModel()
            {
                DosyaAdi = binaYapiDenetimSeviyeTespit.DosyaAdi,
                File = ms.ToArray(),
                MimeType = MimeTypes.GetMimeType(Path.GetExtension(binaYapiDenetimSeviyeTespit.DosyaAdi))
            };

            return await Task.FromResult(result);
        }
    }
}