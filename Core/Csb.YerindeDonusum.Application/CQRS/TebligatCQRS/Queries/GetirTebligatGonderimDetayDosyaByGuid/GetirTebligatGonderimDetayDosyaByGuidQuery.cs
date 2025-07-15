using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirTebligatGonderimDetayDosyaByGuid;

public class GetirTebligatGonderimDetayDosyaByGuidQuery : IRequest<ResultModel<GetirTebligatGonderimDetayDosyaByGuidQueryResponseModel>>
{
    public Guid? DosyaGuid { get; set; }

    public class GetirTebligatGonderimDetayDosyaByGuidQueryHandler : IRequestHandler<GetirTebligatGonderimDetayDosyaByGuidQuery, ResultModel<GetirTebligatGonderimDetayDosyaByGuidQueryResponseModel>>
    {
        private readonly ITebligatGonderimDetayDosyaRepository _tebligatGonderimDetayDosyaRepository;
        private readonly IConfiguration _configuration;

        public GetirTebligatGonderimDetayDosyaByGuidQueryHandler(ITebligatGonderimDetayDosyaRepository tebligatGonderimDetayDosyaRepository, IConfiguration configuration)
        {
            _tebligatGonderimDetayDosyaRepository = tebligatGonderimDetayDosyaRepository;
            _configuration = configuration;
        }

        public Task<ResultModel<GetirTebligatGonderimDetayDosyaByGuidQueryResponseModel>> Handle(GetirTebligatGonderimDetayDosyaByGuidQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirTebligatGonderimDetayDosyaByGuidQueryResponseModel>();

            var tebligatDosya = _tebligatGonderimDetayDosyaRepository.GetWhere(x => x.TebligatGonderimDetayDosyaGuid == request.DosyaGuid&& !x.SilindiMi && x.AktifMi == true, true).FirstOrDefault();

            if (tebligatDosya == null)
            {
                result.ErrorMessage("Dosya bulunamadı!");
                return Task.FromResult(result);
            }

            var fileDiskPath = _configuration.GetSection("UploadFile:Path").Value;

            var fullFilePath = Path.Combine(fileDiskPath!, tebligatDosya.DosyaYolu!, tebligatDosya.DosyaAdi);

            if (!File.Exists(fullFilePath))
            {
                result.Exception(new FileNotFoundException("Sunucuda dosya bulunamadı."), "Sunucuda dosya bulunamadı.");
                return Task.FromResult(result);
            }

            using var fs = new FileStream(fullFilePath, FileMode.Open);
            using var ms = new MemoryStream();

            fs.CopyTo(ms);

            result.Result = new GetirTebligatGonderimDetayDosyaByGuidQueryResponseModel
            {
                Ad = tebligatDosya.DosyaAdi,
                Tur = tebligatDosya.DosyaTuru,
                Icerik = ms.ToArray()
            };

            return Task.FromResult(result);
        }
    }
}
