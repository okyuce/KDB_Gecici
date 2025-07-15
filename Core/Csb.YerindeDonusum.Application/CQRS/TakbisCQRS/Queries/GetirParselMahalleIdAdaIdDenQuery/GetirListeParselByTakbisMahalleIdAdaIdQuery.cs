using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirParselMahalleIdAdaIdDenQuery;

public class GetirListeParselByTakbisMahalleIdAdaIdQuery : IRequest<ResultModel<List<GetirListeParselByTakbisMahalleIdAdaIdQueryResponseModel>>>
{
    public int? TakbisMahalleId { get; set; }
    public string? AdaNo { get; set; }

    public class GetirAdaByMahalleIdQueryHandler : IRequestHandler<GetirListeParselByTakbisMahalleIdAdaIdQuery, ResultModel<List<GetirListeParselByTakbisMahalleIdAdaIdQueryResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly ITakbisService _takbisService;

        public GetirAdaByMahalleIdQueryHandler(IMapper mapper, ITakbisService takbisService)
        {
            _mapper = mapper;
            _takbisService = takbisService;
        }

        public async Task<ResultModel<List<GetirListeParselByTakbisMahalleIdAdaIdQueryResponseModel>>> Handle(GetirListeParselByTakbisMahalleIdAdaIdQuery request, CancellationToken cancellationToken)
        {
            ResultModel<List<GetirListeParselByTakbisMahalleIdAdaIdQueryResponseModel>> result = new();
            try
            {
                result.Result = _mapper.Map<List<GetirListeParselByTakbisMahalleIdAdaIdQueryResponseModel>>(await _takbisService.GetirListeTakbisParselByTakbisMahalleIdAdaNoAsync(request?.TakbisMahalleId??0, request?.AdaNo));
            }
            catch (Exception exc)
            {
                result.Exception(exc, "Hata Meydana Geldi. Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}
