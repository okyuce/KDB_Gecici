using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruDosyaCQRS.Queries;

public class GetirDosyaByIdQuery : IRequest<ResultModel<GetirDosyaByIdQueryResponseModel>>
{
    public GetirDosyaByIdQueryModel Model { get; set; }

    public class GetAppealFileByIdQueryHandler : IRequestHandler<GetirDosyaByIdQuery, ResultModel<GetirDosyaByIdQueryResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruDosyaRepository _appealFilesRepository;
        private readonly IConfiguration _configuration;

        public GetAppealFileByIdQueryHandler(IMapper mapper, IBasvuruDosyaRepository appealFilesRepository, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _appealFilesRepository = appealFilesRepository;
            _configuration = configuration;
        }

        public async Task<ResultModel<GetirDosyaByIdQueryResponseModel>> Handle(GetirDosyaByIdQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirDosyaByIdQueryResponseModel>();

            if (request.Model.Id.IsNullOrEmpty())
            {
                result.Exception(new ArgumentNullException("Dosya id alınamadığı için hata oluştu."), "Dosya id alınamadığı için hata oluştu.");

                return await Task.FromResult(result);
            }

            var id = Guid.Parse(request.Model.Id);

            var file = _appealFilesRepository.GetWhere(x => x.BasvuruDosyaGuid == id && x.AktifMi == true && x.SilindiMi == false).FirstOrDefault();
            if (file == null)
            {
                result.Exception(new FileNotFoundException("Dosya bulunamadı."), "Dosya bulunamadı.");

                return await Task.FromResult(result);
            }

            var fileDiskPath = _configuration.GetSection("UploadFile:Path").Value;

            var fullFilePath = Path.Combine(fileDiskPath!, file.DosyaYolu!, file.DosyaAdi);

            //Sunucu Üzerindeki Dosya Var mı Yok mu Kontrolü
            if (!File.Exists(fullFilePath))
            {
                result.Exception(new FileNotFoundException("Sunucuda dosya bulunamadı."), "Sunucuda dosya bulunamadı.");

                return await Task.FromResult(result);
            }

            using var fs = new FileStream(fullFilePath, FileMode.Open);
            using var ms = new MemoryStream();

            fs.CopyTo(ms);

            result.Result = new GetirDosyaByIdQueryResponseModel
            {
                File = ms.ToArray(),
                MimeType = MimeTypes.GetMimeType(Path.GetExtension(file.DosyaAdi))
            };

            return await Task.FromResult(result);
        }
    }
}