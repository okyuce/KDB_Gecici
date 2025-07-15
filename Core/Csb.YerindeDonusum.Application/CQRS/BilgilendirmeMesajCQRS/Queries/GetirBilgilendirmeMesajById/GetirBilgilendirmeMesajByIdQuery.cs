using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BilgilendirmeMesajCQRS.Queries.GetirBilgilendirmeMesajById;

public class GetirBilgilendirmeMesajByIdQuery : IRequest<ResultModel<GetirBilgilendirmeMesajByIdQueryResponseModel>>
{
    public int Id { get; set; }

    internal class GetirBilgilendirmeMesajByIdQueryHandler : IRequestHandler<GetirBilgilendirmeMesajByIdQuery, ResultModel<GetirBilgilendirmeMesajByIdQueryResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IBilgilendirmeMesajRepository _bilgilendirmeMesajRepository;

        public GetirBilgilendirmeMesajByIdQueryHandler(IMapper mapper, IBilgilendirmeMesajRepository bilgilendirmeMesajRepository)
        {
            _mapper = mapper;
            _bilgilendirmeMesajRepository = bilgilendirmeMesajRepository;
        }

        public async Task<ResultModel<GetirBilgilendirmeMesajByIdQueryResponseModel>> Handle(GetirBilgilendirmeMesajByIdQuery request, CancellationToken cancellationToken)
        {
            ResultModel<GetirBilgilendirmeMesajByIdQueryResponseModel> result = new();

            if (request == null)
            {
                result.Exception(new ArgumentNullException("Geçersiz veya hatalı parametre gönderimi. request değeri null olamaz. ERR-GBMBIQ-GBMBIQH-100"), "Geçersiz veya hatalı parametre gönderimi. Lütfen bilgilerinizi kontrol ediniz. ERR-GBMBIQ-GBMBIQH-100");
                return result;
            }

            if (request.Id <= 0)
            {
                result.Exception(new ArgumentNullException("Geçersiz veya hatalı değer. request id değeri <=0 olamaz. ERR-GBMBIQ-GBMBIQH-200"), "Geçersiz veya hatalı istek. Lütfen bilgilerinizi kontrol ediniz. ERR-GBMBIQ-GBMBIQH-200");
                return result;
            }

            var queryResult = _mapper.Map<GetirBilgilendirmeMesajByIdQueryResponseModel>(_bilgilendirmeMesajRepository.GetWhere(x => x.BilgilendirmeMesajId == request.Id, true).FirstOrDefault());

            if (queryResult == null)
            {
                result.Exception(new Exception($"Id : {request.Id} bilgisine ait kayıt bulunamadı. ERR-GBMBIQ-GBMBIQH-200"), "Geçersiz veya hatalı parametre gönderimi. Lütfen bilgilerinizi kontrol ediniz. ERR-GBMBIQ-GBMBIQH-100");
                return result;
            }

            result.Result = queryResult;

            return result;
        }
    }
}
