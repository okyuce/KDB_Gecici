using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirAdaMahalleIdDenQuery;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeMahalleByTakbisIlceIdQuery;

public class GetirListeAdaByTakbisMahalleIdQuery : IRequest<ResultModel<List<GetirListeAdaByTakbisMahalleIdQueryResponseModel>>>
{
    public int TakbisMahalleId{ get; set; }

    public class GetirAdaByMahalleIdQueryHandler : IRequestHandler<GetirListeAdaByTakbisMahalleIdQuery, ResultModel<List<GetirListeAdaByTakbisMahalleIdQueryResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly ITakbisService _takbisService;

        public GetirAdaByMahalleIdQueryHandler(IMapper mapper, ITakbisService takbisService)
        {
            _mapper = mapper;
            _takbisService = takbisService;
        }

        public async Task<ResultModel<List<GetirListeAdaByTakbisMahalleIdQueryResponseModel>>> Handle(GetirListeAdaByTakbisMahalleIdQuery request, CancellationToken cancellationToken)
        {
            ResultModel<List<GetirListeAdaByTakbisMahalleIdQueryResponseModel>> result = new();

            try
            {
                result.Result = _mapper.Map<List<GetirListeAdaByTakbisMahalleIdQueryResponseModel>>(await _takbisService.GetirListeTakbisAdaByTakbisMahalleIdAsync(request.TakbisMahalleId));
            }
            catch (Exception exc)
            {
                result.Exception(exc, "Hata Meydana Geldi. Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}
