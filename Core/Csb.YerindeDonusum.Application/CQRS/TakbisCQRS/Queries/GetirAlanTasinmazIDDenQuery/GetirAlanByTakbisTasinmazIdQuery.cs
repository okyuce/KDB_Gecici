using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirAlanByTakbisTasinmazIdQuery;

public class GetirAlanByTakbisTasinmazIdQuery : IRequest<ResultModel<List<GetirAlanByTasinmazIdQueryResponseModel>>>
{
    public GetirAlanByTakbisTasinmazIdQueryModel Model { get; set; }

    public class GetirAlanByTakbisTasinmazIdQueryHandler : IRequestHandler<GetirAlanByTakbisTasinmazIdQuery, ResultModel<List<GetirAlanByTasinmazIdQueryResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly ITakbisService _takbisService;

        public GetirAlanByTakbisTasinmazIdQueryHandler(IMapper mapper, ITakbisService takbisService)
        {
            _mapper = mapper;
            _takbisService = takbisService;
        }

        public async Task<ResultModel<List<GetirAlanByTasinmazIdQueryResponseModel>>> Handle(GetirAlanByTakbisTasinmazIdQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<List<GetirAlanByTasinmazIdQueryResponseModel>>();

            try
            {
                result.Result = _mapper.Map<List<GetirAlanByTasinmazIdQueryResponseModel>>(await _takbisService.GetirListeAlanByTakbisTasinmazIdAsync(request.Model));
            }
            catch (Exception exception)
            {
                result.Exception(exception, "Hata Meydana Geldi. Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}