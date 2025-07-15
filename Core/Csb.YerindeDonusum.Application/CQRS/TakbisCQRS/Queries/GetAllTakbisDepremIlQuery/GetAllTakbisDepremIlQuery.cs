using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.Takbis;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.IlCQRS.Queries.GetAllTakbisDepremIlQuery;

public class GetAllTakbisDepremIlQuery : IRequest<ResultModel<List<IlModel>>>
{
    public class GetAllTakbisDepremIlQueryHandler : IRequestHandler<GetAllTakbisDepremIlQuery, ResultModel<List<IlModel>>>
    {
        private readonly ITakbisService _takbisService;

        public GetAllTakbisDepremIlQueryHandler(ITakbisService takbisService)
        {
            _takbisService = takbisService;
        }

        public async Task<ResultModel<List<IlModel>>> Handle(GetAllTakbisDepremIlQuery request, CancellationToken cancellationToken)
        {
            ResultModel<List<IlModel>> result = new();
            try
            {
                result.Result = await _takbisService.GetirListeTakbisDepremIlAsnyc();
            }
            catch (Exception exc)
            {
                result.Exception(exc, "Hata Oluştu. Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}