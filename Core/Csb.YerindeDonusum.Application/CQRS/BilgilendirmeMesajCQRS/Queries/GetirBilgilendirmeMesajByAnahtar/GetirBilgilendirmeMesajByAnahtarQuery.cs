using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BilgilendirmeMesajCQRS.Queries.GetirBilgilendirmeMesajById;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Csb.YerindeDonusum.Application.CQRS.BilgilendirmeMesajCQRS.Queries.GetirBilgilendirmeMesajByAnahtar;

public class GetirBilgilendirmeMesajByAnahtarQuery : IRequest<ResultModel<GetirBilgilendirmeMesajByIdQueryResponseModel>>
{
    public string Anahtar { get; set; }

    internal class GetirBilgilendirmeMesajByAnahtarQueryHandler : IRequestHandler<GetirBilgilendirmeMesajByAnahtarQuery, ResultModel<GetirBilgilendirmeMesajByIdQueryResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IBilgilendirmeMesajRepository _bilgilendirmeMesajRepository;

        public GetirBilgilendirmeMesajByAnahtarQueryHandler(IMapper mapper, IBilgilendirmeMesajRepository bilgilendirmeMesajRepository)
        {
            _mapper = mapper;
            _bilgilendirmeMesajRepository = bilgilendirmeMesajRepository;
        }

        public async Task<ResultModel<GetirBilgilendirmeMesajByIdQueryResponseModel>> Handle(GetirBilgilendirmeMesajByAnahtarQuery request, CancellationToken cancellationToken)
        {
            ResultModel<GetirBilgilendirmeMesajByIdQueryResponseModel> result = new();

            try
            {
                degerleriKontrolEt(request, result);

                // kontroller sirasında bir hata ile karsilasiliyorsa program akisini keserek geriye donuyoruz.
                if (result.IsError)
                    return await Task.FromResult(result);

                var queryResult = _mapper.Map<GetirBilgilendirmeMesajByIdQueryResponseModel>(await _bilgilendirmeMesajRepository.GetWhere(x => x.Anahtar.Equals(request.Anahtar) && x.SilindiMi == false, true).FirstOrDefaultAsync());

                if (queryResult == null)
                {
                    result.Exception(new Exception($"Id : {request.Anahtar} bilgisine ait kayıt bulunamadı. ERR-GBMBAQ-GBMBAQH-200"), "Talep Edilen Bilgilendirme Mesajı Bulunamadı. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz. ERR-GBMBAQ-GBMBAQH-300");
                    return result;
                }

                result.Result = queryResult;
            }
            catch (Exception ex)
            {
                result.Exception(ex, "Bilgilendirme Mesajı Bilgisi Alınırken Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz. ERR-GBMBAQ-GBMBAQH-1000");
            }

            return result;
        }

        private void degerleriKontrolEt(GetirBilgilendirmeMesajByAnahtarQuery request, ResultModel<GetirBilgilendirmeMesajByIdQueryResponseModel> result)
        {
            if (request == null)
            {
                result.Exception(new ArgumentNullException("Geçersiz veya hatalı parametre gönderimi. request değeri null olamaz. ERR-GBMBAQ-GBMBAQH-100"), "Geçersiz veya hatalı parametre gönderimi. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz. ERR-GBMBAQ-GBMBAQH-100");
                return;
            }

            if (string.IsNullOrWhiteSpace(request.Anahtar))
            {
                result.Exception(new ArgumentNullException("Geçersiz veya hatalı değer. request id değeri null olamaz. ERR-GBMBAQ-GBMBAQH-200"), "Geçersiz veya hatalı istek. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz. ERR-GBMBAQ-GBMBAQH-200");
                return;
            }
        }
    }
}
