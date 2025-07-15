using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeAnaTasinmaz;

public class GetirListeAnaTasinmazQuery : IRequest<ResultModel<List<GetirListeAnaTasinmazQueryResponseModel>>>
{
    public GetirAnaTasinmazQueryModel Model { get; set; }

    public class GetirListeAnaTasinmazQueryHandler : IRequestHandler<GetirListeAnaTasinmazQuery, ResultModel<List<GetirListeAnaTasinmazQueryResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly ITakbisService _takbisService;

        public GetirListeAnaTasinmazQueryHandler(IMapper mapper, ITakbisService takbisService)
        {
            _mapper = mapper;
            _takbisService = takbisService;
        }

        public async Task<ResultModel<List<GetirListeAnaTasinmazQueryResponseModel>>> Handle(GetirListeAnaTasinmazQuery request, CancellationToken cancellationToken)
        {
            ResultModel<List<GetirListeAnaTasinmazQueryResponseModel>> result = new();

            try
            {
                result.Result = _mapper.Map<List<GetirListeAnaTasinmazQueryResponseModel>>(_takbisService.GetirAnaTasinmaz(request.Model));
            }
            catch (Exception ex)
            {
                result.Exception(ex, "Ana Taşınmazlar listelenirken hata oluştu. Tekrar deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}
