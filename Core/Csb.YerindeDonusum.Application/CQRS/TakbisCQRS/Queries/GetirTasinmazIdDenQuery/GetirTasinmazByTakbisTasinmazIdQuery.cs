using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazByTakbisTasinmazId;

public class GetirTasinmazByTakbisTasinmazIdQuery : IRequest<ResultModel<GetirTasinmazByTakbisTasinmazIdQueryResponseModel>>
{
    public GetirTasinmazByTakbisTasinmazIdQueryModel Model { get; set; }

    public class GetirTasinmazIdDenQueryHandler : IRequestHandler<GetirTasinmazByTakbisTasinmazIdQuery, ResultModel<GetirTasinmazByTakbisTasinmazIdQueryResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly ITakbisService _takbisService;

        public GetirTasinmazIdDenQueryHandler(IMapper mapper, ITakbisService takbisService)
        {
            _mapper = mapper;
            _takbisService = takbisService;
        }

        public async Task<ResultModel<GetirTasinmazByTakbisTasinmazIdQueryResponseModel>> Handle(GetirTasinmazByTakbisTasinmazIdQuery request, CancellationToken cancellationToken)
        {
            ResultModel<GetirTasinmazByTakbisTasinmazIdQueryResponseModel> result = new();

            try
            {
                result.Result = _mapper.Map<GetirTasinmazByTakbisTasinmazIdQueryResponseModel>(await _takbisService.GetirTasinmazByTakbisTasinmazIdAsync(request.Model));
            }
            catch (Exception exc)
            {
                result.Exception(exc, "Hata Meydana Geldi. Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}
