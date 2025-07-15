using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.IndirBinaNakdiYardimTaksitDosya;

public class IndirBinaNakdiYardimTaksitDosyaQuery : IRequest<ResultModel<IndirBinaNakdiYardimTaksitDosyaQueryResponseModel>>
{
    public IndirBinaNakdiYardimTaksitDosyaQueryModel? Model { get; set; }

    public class IndirBinaNakdiYardimTaksitDosyaQueryHandler : IRequestHandler<IndirBinaNakdiYardimTaksitDosyaQuery, ResultModel<IndirBinaNakdiYardimTaksitDosyaQueryResponseModel>>
    {
        private readonly IBinaNakdiYardimTaksitDosyaRepository _binaNakdiYardimTaksitDosyaRepository;
        private readonly IConfiguration _configuration;

        public IndirBinaNakdiYardimTaksitDosyaQueryHandler(IConfiguration configuration, IBinaNakdiYardimTaksitDosyaRepository binaNakdiYardimTaksitDosyaRepository)
        {
            _binaNakdiYardimTaksitDosyaRepository = binaNakdiYardimTaksitDosyaRepository;
            _configuration = configuration;
        }

        public async Task<ResultModel<IndirBinaNakdiYardimTaksitDosyaQueryResponseModel>> Handle(IndirBinaNakdiYardimTaksitDosyaQuery request, CancellationToken cancellationToken)
        {
            ResultModel<IndirBinaNakdiYardimTaksitDosyaQueryResponseModel> result = new ResultModel<IndirBinaNakdiYardimTaksitDosyaQueryResponseModel>();
            try
            {
                Guid.TryParse(request?.Model?.BinaNakdiYardimTaksitDosyaGuid, out Guid binaNakdiYardimTaksitDosyaGuid);

                BinaNakdiYardimTaksitDosya? binaNakdiYardimTaksit = _binaNakdiYardimTaksitDosyaRepository
                                             .GetWhere(x => x.BinaNakdiYardimTaksitDosyaGuid == binaNakdiYardimTaksitDosyaGuid
                                                         && x.AktifMi == true
                                                         && x.SilindiMi == false,
                                                         true)
                                             .FirstOrDefault();

                if (binaNakdiYardimTaksit != null)
                {
                    string? fileDiskPath = _configuration.GetSection("UploadFile:Path").Value;

                    string fullFilePath = Path.Combine(fileDiskPath!, binaNakdiYardimTaksit.DosyaYolu!, binaNakdiYardimTaksit.DosyaAdi);

                    //Sunucu Üzerindeki Dosya Var mı Yok mu Kontrolü
                    if (!File.Exists(fullFilePath))
                    {
                        result.Exception(new FileNotFoundException("Sunucuda dosya bulunamadı."), "Sunucuda dosya bulunamadı.");

                        return await Task.FromResult(result);
                    }

                    using FileStream fs = new FileStream(fullFilePath, FileMode.Open);
                    using MemoryStream ms = new MemoryStream();

                    fs.CopyTo(ms);

                    result.Result = new IndirBinaNakdiYardimTaksitDosyaQueryResponseModel()
                    {
                        DosyaAdi = binaNakdiYardimTaksit.DosyaAdi,
                        File = ms.ToArray(),
                        MimeType = MimeTypes.GetMimeType(Path.GetExtension(binaNakdiYardimTaksit.DosyaAdi))
                    };
                }
                else
                {
                    result.ErrorMessage("Bina Nakdi Yardım Dosyası Bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                result.Exception(ex, "Bina Nakdi Yardım Dosyası Alınırken Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}