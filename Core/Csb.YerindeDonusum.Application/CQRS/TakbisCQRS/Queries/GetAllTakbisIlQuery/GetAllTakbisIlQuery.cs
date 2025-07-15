using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.Takbis;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.IlCQRS.Queries.GetAllTakbisIlQuery;

public class GetAllTakbisIlQuery : IRequest<ResultModel<List<IlModel>>>
{
    public class GetAllTakbisIlQueryHandler : IRequestHandler<GetAllTakbisIlQuery, ResultModel<List<IlModel>>>
    {
        private readonly ITakbisService _takbisService;

        public GetAllTakbisIlQueryHandler(ITakbisService takbisService)
        {
            _takbisService = takbisService;
        }

        public async Task<ResultModel<List<IlModel>>> Handle(GetAllTakbisIlQuery request, CancellationToken cancellationToken)
        {
            ResultModel<List<IlModel>> result = new();
            try
            {
                result.Result = await _takbisService.GetirListeTakbisIlAsnyc();
            }
            catch (Exception exc)
            {
                result.Exception(exc, "Hata Oluştu. Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}
