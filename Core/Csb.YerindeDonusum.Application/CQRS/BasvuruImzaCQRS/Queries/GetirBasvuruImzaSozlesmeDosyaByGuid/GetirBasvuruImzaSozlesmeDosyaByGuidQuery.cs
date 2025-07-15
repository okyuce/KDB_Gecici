using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirBasvuruImzaSozlesmeDosyaByGuid;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Queries.GetirBasvuruImzaSozlesmeDosyaByGuid;

public class GetirBasvuruImzaSozlesmeDosyaByGuidQuery : IRequest<ResultModel<GetirBasvuruImzaSozlesmeDosyaByGuidResponseModel>>
{
    public long? DosyaTurId { get; set; }
    public Guid? DosyaGuid { get; set; }

    public class GetirBasvuruImzaSozlesmeDosyaByGuidQueryHandler : IRequestHandler<GetirBasvuruImzaSozlesmeDosyaByGuidQuery, ResultModel<GetirBasvuruImzaSozlesmeDosyaByGuidResponseModel>>
    {
        private readonly IBasvuruImzaVerenDosyaRepository _basvuruImzaVerenDosyaRepository;
        private readonly IConfiguration _configuration;

        public GetirBasvuruImzaSozlesmeDosyaByGuidQueryHandler(IBasvuruImzaVerenDosyaRepository basvuruImzaVerenDosyaRepository, IConfiguration configuration)
        {
            _basvuruImzaVerenDosyaRepository = basvuruImzaVerenDosyaRepository;
            _configuration = configuration;
        }

        public Task<ResultModel<GetirBasvuruImzaSozlesmeDosyaByGuidResponseModel>> Handle(GetirBasvuruImzaSozlesmeDosyaByGuidQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirBasvuruImzaSozlesmeDosyaByGuidResponseModel>();

            var imzaVerenDosya = _basvuruImzaVerenDosyaRepository.GetWhere(x => x.BasvuruImzaVerenDosyaGuid == request.DosyaGuid && x.BasvuruImzaVerenDosyaTurId == request.DosyaTurId && !x.SilindiMi && x.AktifMi == true, true).FirstOrDefault();

            if (imzaVerenDosya == null)
            {
                result.ErrorMessage("Dosya bulunamadı!");
                return Task.FromResult(result);
            }

            var fileDiskPath = _configuration.GetSection("UploadFile:Path").Value;

            var fullFilePath = Path.Combine(fileDiskPath!, imzaVerenDosya.DosyaYolu!, imzaVerenDosya.DosyaAdi);

            if (!File.Exists(fullFilePath))
            {
                result.Exception(new FileNotFoundException("Sunucuda dosya bulunamadı."), "Sunucuda dosya bulunamadı.");
                return Task.FromResult(result);
            }

            using var fs = new FileStream(fullFilePath, FileMode.Open);
            using var ms = new MemoryStream();

            fs.CopyTo(ms);

            result.Result = new GetirBasvuruImzaSozlesmeDosyaByGuidResponseModel {
                Ad = imzaVerenDosya.DosyaAdi,
                Tur = imzaVerenDosya.DosyaTuru,
                Icerik = ms.ToArray()
            };

            return Task.FromResult(result);
        }
    }
}