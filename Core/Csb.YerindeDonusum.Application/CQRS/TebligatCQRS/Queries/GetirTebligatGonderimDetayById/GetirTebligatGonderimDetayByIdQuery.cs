using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirTebligatGonderimDetayById;

public class GetirTebligatGonderimDetayByIdQuery : IRequest<ResultModel<GetirTebligatGonderimDetayByIdQueryResponseModel>>
{
    public long? TebligatGonderimDetayId { get; set; }

    public class GetirTebligatGonderimDetayByIdQueryHandler : IRequestHandler<GetirTebligatGonderimDetayByIdQuery, ResultModel<GetirTebligatGonderimDetayByIdQueryResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly ITebligatGonderimDetayRepository _tebligatGonderimDosyaDetay;

        public GetirTebligatGonderimDetayByIdQueryHandler(IMapper mapper, ITebligatGonderimDetayRepository tebligatGonderimDosyaDetay)
        {
            _mapper = mapper;
            _tebligatGonderimDosyaDetay = tebligatGonderimDosyaDetay;
        }

        public Task<ResultModel<GetirTebligatGonderimDetayByIdQueryResponseModel>> Handle(GetirTebligatGonderimDetayByIdQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirTebligatGonderimDetayByIdQueryResponseModel>();

            var tebligatGonderimDetay = _tebligatGonderimDosyaDetay.GetWhere(x => x.TebligatGonderimDetayId == request.TebligatGonderimDetayId, true).FirstOrDefault();

            if(tebligatGonderimDetay ==  null)
                result.ErrorMessage("Tebligat gönderim detayı bulunamadı!");

            result.Result = _mapper.Map<GetirTebligatGonderimDetayByIdQueryResponseModel>(tebligatGonderimDetay);

            return Task.FromResult(result);
        }
    }
}
