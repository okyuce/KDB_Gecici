using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeDosyaCQRS.Commands;

public class BinaDegerlendirmeDosyaIndirCommand : IRequest<ResultModel<BinaDegerlendirmeDosyaIndirCommandResponseModel>>
{
        public string BinaDegerlendirmeDosyaId { get; set; } = string.Empty;

	public class BinaDegerlendirmeDosyaIndirCommandHandle : IRequestHandler<BinaDegerlendirmeDosyaIndirCommand, ResultModel<BinaDegerlendirmeDosyaIndirCommandResponseModel>>
    {
        private readonly IBinaDegerlendirmeDosyaRepository _binaDegerlendirmeDosyaRepository;
        private readonly IConfiguration _configuration;

        public BinaDegerlendirmeDosyaIndirCommandHandle(IBinaDegerlendirmeDosyaRepository binaDegerlendirmeDosyaRepository, IConfiguration configuration)
        {
            _binaDegerlendirmeDosyaRepository = binaDegerlendirmeDosyaRepository;
            _configuration = configuration;
        }

        public async Task<ResultModel<BinaDegerlendirmeDosyaIndirCommandResponseModel>> Handle(BinaDegerlendirmeDosyaIndirCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<BinaDegerlendirmeDosyaIndirCommandResponseModel>();

				//if (request == null)
				//{
				//    result.Exception(new ArgumentNullException("Dosya Id Boş Geçilemez"));

				//    return await Task.FromResult(result);
				//}

				// input bilgilerinin kontrol edildigi method
				// fluentvalidation yapıldı.
				//ValidationInputValue(request.Model, result);

				// kontroller sirasında bir hata ile karsilasiliyorsa program akisini keserek geriye donuyoruz.
				if (result.IsError)
                return await Task.FromResult(result);

            try
            {
                var fileGuid = Guid.Parse(request.BinaDegerlendirmeDosyaId);

                var file = _binaDegerlendirmeDosyaRepository
                        .GetWhere(x => x.BinaDosyaGuid == fileGuid                           
                            && x.AktifMi == true
                            && x.SilindiMi == false, true
                        )
                            .FirstOrDefault();

                if (file == null)
                {
                    result.Exception(new NullReferenceException($"FileGuid:{fileGuid} bilgilerine ait osya bilgisi bulunamadı."), "Talep etmiş olduğunuz dosya bilgisine ulaşılamadı. Lütfen bilgilerinizi kontrol ediniz.");
                    
                    return await Task.FromResult(result);
                }

                var fileDiskPath = _configuration.GetSection("UploadFile:Path").Value;

                var fullFilePath = Path.Combine(fileDiskPath!, file.DosyaYolu!, file.DosyaAdi);

                //Sunucu Üzerindeki Dosya Var mı Yok mu Kontrolü
                if (File.Exists(fullFilePath))
                {
                    using var fs = new FileStream(fullFilePath, FileMode.Open);
                    using var ms = new MemoryStream();

                    fs.CopyTo(ms);

                    var appealFileDownloadResponseModel = new BinaDegerlendirmeDosyaIndirCommandResponseModel();
                    appealFileDownloadResponseModel.DosyaAdi = file.DosyaAdi;
                    appealFileDownloadResponseModel.Base64 = Convert.ToBase64String(ms.ToArray());

                    result.Result = appealFileDownloadResponseModel;
                }
                else
                {
                    result.Exception(new FileNotFoundException("Dosya Bulunamadı"));

                    return await Task.FromResult(result);
                }
            }
            catch (FormatException ex)
            {
                result.Exception(ex, "Hatalı veya Geçersiz Parametre");

                return await Task.FromResult(result);
            }
            catch (IOException ex)
            {
                result.Exception(ex, "Başvuru Dosyası İndirme İşlemi Sırasında Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz.");
            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    result.Exception(ex, "Başvuru Dosyası İndirme İşlemi Sırasında Bir Hata Meydana Geldi. Hatalı veya Geçersiz İlişkili Bilgi Gönderildi. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
                }
                else
                {
                    result.Exception(ex, "Başvuru Dosyası İndirme İşlemi Sırasında Bir Hata Meydana Geldi.");
                }
            }

            return await Task.FromResult(result);
        }

        // fluentvalidation yapıldı.
        //private void ValidationInputValue(BasvuruDosyaIndirCommandModel model, ResultModel<BasvuruDosyaIndirCommandResponseModel> result)
        //{
            //if (model == null)
            //{
            //    result.Exception(new ArgumentNullException("Başvuru dosya indirme işlemleri için gerekli olan parametreler null olamaz."), "İşlem Sırasında Bir Hata Meydana Geldi. Lütfen Bilgilerinizi Kontrol Ediniz.");

            //    return;
            //}

            //if (string.IsNullOrWhiteSpace(model.BasvuruDosyaId) || !Guid.TryParse(model.BasvuruDosyaId, out Guid appealGuid))
            //{
            //    result.Exception(new ArgumentNullException("Başvuru dosya indirme işlemleri için gerekli olan istek BasvuruDosyaId bilgisi null olamaz."), "İşlem Sırasında Bir Hata Meydana Geldi. Hatalı veya Geçersiz Dosya Bilgisi.");

            //    return;
            //}

            //if (appealGuid == Guid.Empty)
            //{
            //    result.Exception(new ArgumentNullException("Başvuru dosya indirme işlemleri için gerekli olan istek BasvuruDosyaId bilgisi geçersiz."), "İşlem Sırasında Bir Hata Meydana Geldi. Hatalı veya Geçersiz Dosya Bilgisi.");

            //    return;
            //}

            //if (string.IsNullOrWhiteSpace(model.IdentificationNumber) || !Guid.TryParse(model.AppealFileId, out Guid identitficationNumber))
            //{
            //    result.Exception(new ArgumentNullException("Başvuru dosya indirme işlemleri için gerekli olan istek IdentificationNumber bilgisi null olamaz."), "İşlem sırasında bir hata meydana geldi. Hatalı veya geçersiz kişi bilgisi.");

            //    return;
            //}

            //if (identitficationNumber == Guid.Empty)
            //{
            //    result.Exception(new ArgumentNullException("Başvuru dosya indirme işlemleri için gerekli olan istek IdentificationNumber bilgisi geçersiz."), "İşlem sırasında bir hata meydana geldi. Hatalı veya geçersiz kişi bilgisi.");

            //    return;
            //}

            //if (string.IsNullOrWhiteSpace(model.TcKimlikNo))
            //{
            //    result.Exception(new ArgumentNullException("TC kimlik No Bilgisi Boş Olamaz."), "TC kimlik No Bilgisi Boş Olamaz.");

            //    return;
            //}

            //if (model.TcKimlikNo.Length != 11)
            //{
            //    result.Exception(new ArgumentNullException("Geçersiz veya Hatalı TC Kimlik No Bilgisi."), "Geçersiz veya Hatalı TC Kimlik No Bilgisi.");

            //    return;
            //}
        //}
    }
}
