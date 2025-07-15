using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazByTakbisTasinmazId;

public class GetirHisseTasinmazIdDenQuery : IRequest<ResultModel<GetirHisseTasinmazIdQueryResponseModel>>
{
    public GetirHisseTasinmazIdDenQueryModel Model { get; set; }

    public class GetirHisseTasinmazIdDenQueryHandler : IRequestHandler<GetirHisseTasinmazIdDenQuery, ResultModel<GetirHisseTasinmazIdQueryResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly ITakbisService _takbisService;

        public GetirHisseTasinmazIdDenQueryHandler(IMapper mapper, ITakbisService takbisService)
        {
            _mapper = mapper;
            _takbisService = takbisService;
        }

        public async Task<ResultModel<GetirHisseTasinmazIdQueryResponseModel>> Handle(GetirHisseTasinmazIdDenQuery request, CancellationToken cancellationToken)
        {
            ResultModel<GetirHisseTasinmazIdQueryResponseModel> result = new();

            try
            {
                result.Result = _mapper.Map<GetirHisseTasinmazIdQueryResponseModel>(await _takbisService.GetirHisseByTakbisTasinmazIdAsync(request.Model));
            }
            catch (Exception exc)
            {
                result.Exception(exc, "Hata Meydana Geldi. Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }


    }
}
