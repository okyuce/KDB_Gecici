using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.IndirBinaDegerlendirmeDosya;

public class IndirBinaDegerlendirmeDosyaQuery : IRequest<ResultModel<IndirBinaDegerlendirmeDosyaQueryResponseModel>>
{
    public IndirBinaDegerlendirmeDosyaQueryModel? Model { get; set; }

    public class IndirBinaDegerlendirmeDosyaQueryHandler : IRequestHandler<IndirBinaDegerlendirmeDosyaQuery, ResultModel<IndirBinaDegerlendirmeDosyaQueryResponseModel>>
    {
        private readonly IBinaDegerlendirmeDosyaRepository _binaDegerlendirmeDosyaRepository;
        private readonly IConfiguration _configuration;

        public IndirBinaDegerlendirmeDosyaQueryHandler(IBinaDegerlendirmeDosyaRepository binaDegerlendirmeDosyaRepository, IConfiguration configuration)
        {
            _binaDegerlendirmeDosyaRepository = binaDegerlendirmeDosyaRepository;
            _configuration = configuration;
        }

        public async Task<ResultModel<IndirBinaDegerlendirmeDosyaQueryResponseModel>> Handle(IndirBinaDegerlendirmeDosyaQuery request, CancellationToken cancellationToken)
        {
            ResultModel<IndirBinaDegerlendirmeDosyaQueryResponseModel> result = new ResultModel<IndirBinaDegerlendirmeDosyaQueryResponseModel>();
            try
            {
                Guid.TryParse(request?.Model?.BinaDosyaGuid, out Guid binaDosyaGuid);

                BinaDegerlendirmeDosya? binaDegerlendirmeDosya = _binaDegerlendirmeDosyaRepository
                                             .GetWhere(x => x.BinaDosyaGuid == binaDosyaGuid
                                                         && x.AktifMi == true
                                                         && x.SilindiMi == false)
                                             .AsNoTracking()
                                             .FirstOrDefault();

                if (binaDegerlendirmeDosya != null)
                {
                    string? fileDiskPath = _configuration.GetSection("UploadFile:Path").Value;

                    string fullFilePath = Path.Combine(fileDiskPath!, binaDegerlendirmeDosya.DosyaYolu!, binaDegerlendirmeDosya.DosyaAdi);

                    //Sunucu Üzerindeki Dosya Var mı Yok mu Kontrolü
                    if (!File.Exists(fullFilePath))
                    {
                        result.Exception(new FileNotFoundException("Sunucuda dosya bulunamadı."), "Sunucuda dosya bulunamadı.");

                        return await Task.FromResult(result);
                    }

                    using FileStream fs = new FileStream(fullFilePath, FileMode.Open);
                    using MemoryStream ms = new MemoryStream();

                    fs.CopyTo(ms);

                    result.Result = new IndirBinaDegerlendirmeDosyaQueryResponseModel()
                    {
                        DosyaAdi = binaDegerlendirmeDosya.DosyaAdi,
                        File = ms.ToArray(),
                        MimeType = MimeTypes.GetMimeType(Path.GetExtension(binaDegerlendirmeDosya.DosyaAdi))
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