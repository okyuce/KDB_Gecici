using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirListeAfadBasvuruIl;

public class GetirListeAfadBasvuruIlQuery : IRequest<ResultModel<List<SelectDto<int>>>>, ICacheMediatrQuery
{
    #region Cache Ayar
    public bool? CacheCustomUser => null;
    public int? CacheMinute => null;
    public bool CacheIsActive => true;
    #endregion

    public class GetirListeAfadBasvuruIlQueryHandler : IRequestHandler<GetirListeAfadBasvuruIlQuery, ResultModel<List<SelectDto<int>>>>
    {
        private readonly IAfadBasvuruTekilRepository _afadBasvuruTekilRepository;

        public GetirListeAfadBasvuruIlQueryHandler(IAfadBasvuruTekilRepository afadBasvuruTekilRepository)
        {
            _afadBasvuruTekilRepository = afadBasvuruTekilRepository;
        }

        public async Task<ResultModel<List<SelectDto<int>>>> Handle(GetirListeAfadBasvuruIlQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<List<SelectDto<int>>>();

            result.Result = _afadBasvuruTekilRepository
                                .GetWhere(x =>
                                    x.IlId != null
                                    &&
                                    !x.CsbSilindiMi
                                    &&
                                    x.CsbAktifMi == true,
                                    asNoTracking: true
                                )
                                .GroupBy(g => new
                                {
                                    g.IlId,
                                    g.Il
                                })
                                .Select(s => new SelectDto<int>
                                {
                                    Id = s.Key.IlId.Value,
                                    Ad = s.Key.Il
                                })
                                .OrderBy(o => o.Ad)
                                .ToList();

            return await Task.FromResult(result);
        }
    }
}