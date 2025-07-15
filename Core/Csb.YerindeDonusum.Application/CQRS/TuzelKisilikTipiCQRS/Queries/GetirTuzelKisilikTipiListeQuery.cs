using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.TuzelKisilikTipiCQRS;

public class GetirTuzelKisilikTipiListeQuery : IRequest<ResultModel<List<TuzelKisilikTipiDto>>>
{
    public class GetirTuzelKisilikTipiListeQueryHandler : IRequestHandler<GetirTuzelKisilikTipiListeQuery, ResultModel<List<TuzelKisilikTipiDto>>>
    {
        public GetirTuzelKisilikTipiListeQueryHandler()
        { }

        public Task<ResultModel<List<TuzelKisilikTipiDto>>> Handle(GetirTuzelKisilikTipiListeQuery request, CancellationToken cancellationToken)
        {
            var list = Enum.GetValues(typeof(TuzelKisilikEnum))
                    .Cast<TuzelKisilikEnum>()
                    .Select(s => new TuzelKisilikTipiDto { TuzelKisilikTipiId = (int)s, TuzelKisilikTipiAd = s.GetDisplayName() })
                    .Where(x => x.TuzelKisilikTipiId != (int)TuzelKisilikEnum.KOOPERATIF && x.TuzelKisilikTipiId != (int) TuzelKisilikEnum.SIRKET)
                    .ToList();

            return Task.FromResult(new ResultModel<List<TuzelKisilikTipiDto>>(list));
        }
    }
}