using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirAfadBasvuruById;

public class GetirAfadBasvuruByIdQuery : IRequest<ResultModel<GetirAfadBasvuruByIdQueryResponseModel>>
{
    public Guid? CsbId { get; set; }

    public class GetirAfadBasvuruByIdQueryHandler : IRequestHandler<GetirAfadBasvuruByIdQuery, ResultModel<GetirAfadBasvuruByIdQueryResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IAfadBasvuruTekilRepository _afadBasvuruTekilRepository;

        public GetirAfadBasvuruByIdQueryHandler(IMapper mapper, IAfadBasvuruTekilRepository afadBasvuruTekilRepository)
        {
            _mapper = mapper;
            _afadBasvuruTekilRepository = afadBasvuruTekilRepository;
        }

        public async Task<ResultModel<GetirAfadBasvuruByIdQueryResponseModel>> Handle(GetirAfadBasvuruByIdQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirAfadBasvuruByIdQueryResponseModel>();

            var afadBasvuru = _afadBasvuruTekilRepository
                                .GetWhere(x =>
                                    x.CsbId == request.CsbId
                                    &&
                                    !x.CsbSilindiMi
                                    &&
                                    x.CsbAktifMi == true,
                                    true
                                )
                                .FirstOrDefault();

            if (afadBasvuru == null)
            {
                result.ErrorMessage("Afad başvurusu bulunamadı!");
                return await Task.FromResult(result);
            }

            result.Result = _mapper.Map<GetirAfadBasvuruByIdQueryResponseModel>(afadBasvuru);

            return await Task.FromResult(result);
        }
    }
}