using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeIlceByTakbisIlId;

public class GetirListeIlceByTakbisIlIdQuery : IRequest<ResultModel<List<GetirListeIlceByTakbisIlIdQueryResponseModel>>>
{
    public int? TakbisIlId { get; set; }

    public class GetirListeIlceByTakbisIlIdQueryHandler : IRequestHandler<GetirListeIlceByTakbisIlIdQuery, ResultModel<List<GetirListeIlceByTakbisIlIdQueryResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly ITakbisService _takbisService;

        public GetirListeIlceByTakbisIlIdQueryHandler(IMapper mapper, ITakbisService takbisService)
        {
            _mapper = mapper;
            _takbisService = takbisService;
        }

        public async Task<ResultModel<List<GetirListeIlceByTakbisIlIdQueryResponseModel>>> Handle(GetirListeIlceByTakbisIlIdQuery request, CancellationToken cancellationToken)
        {
            ResultModel<List<GetirListeIlceByTakbisIlIdQueryResponseModel>> result = new();

            try
            {
                result.Result = _mapper.Map<List<GetirListeIlceByTakbisIlIdQueryResponseModel>>(await _takbisService.GetirListeTakbisIlceByTakbisIlIdAsync(request.TakbisIlId??0));
            }
            catch (Exception exc)
            {
                result.Exception(exc, "Hata Oluştu. Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}
