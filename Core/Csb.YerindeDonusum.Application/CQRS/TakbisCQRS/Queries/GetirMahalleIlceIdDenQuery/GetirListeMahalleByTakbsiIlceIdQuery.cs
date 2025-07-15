using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeMahalleByTakbisIlceIdQuery;

public class GetirListeMahalleByTakbsiIlceIdQuery : IRequest<ResultModel<List<GetirListeMahalleByTakbisIlceIdQueryResponseModel>>>
{
    public int TakbisIlceId{ get; set; }

    public class GetirMahalleByIlceIdQueryHandler : IRequestHandler<GetirListeMahalleByTakbsiIlceIdQuery, ResultModel<List<GetirListeMahalleByTakbisIlceIdQueryResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly ITakbisService _takbisService;

        public GetirMahalleByIlceIdQueryHandler(IMapper mapper, ITakbisService takbisService)
        {
            _mapper = mapper;
            _takbisService = takbisService;
        }

        public async Task<ResultModel<List<GetirListeMahalleByTakbisIlceIdQueryResponseModel>>> Handle(GetirListeMahalleByTakbsiIlceIdQuery request, CancellationToken cancellationToken)
        {
            ResultModel<List<GetirListeMahalleByTakbisIlceIdQueryResponseModel>> result = new();

            try
            {
                result.Result = _mapper.Map<List<GetirListeMahalleByTakbisIlceIdQueryResponseModel>>(await _takbisService.GetirListeTakbisMahalleByTakbisIlceIdAsync(request.TakbisIlceId));
            }
            catch (Exception exc)
            {
                result.Exception(exc, "Hata Meydana Geldi. Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}
