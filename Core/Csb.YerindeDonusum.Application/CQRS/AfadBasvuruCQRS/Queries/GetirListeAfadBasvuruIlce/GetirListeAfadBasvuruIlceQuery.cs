using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirListeAfadBasvuruIlce;

public class GetirListeAfadBasvuruIlceQuery : IRequest<ResultModel<List<SelectDto<int>>>>, ICacheMediatrQuery
{
    public int? IlId { get; set; }

    #region Cache Ayar
    public bool? CacheCustomUser => null;
    public int? CacheMinute => null;
    public bool CacheIsActive => true;
    #endregion

    public class GetirListeAfadBasvuruIlceQueryHandler : IRequestHandler<GetirListeAfadBasvuruIlceQuery, ResultModel<List<SelectDto<int>>>>
    {
        private readonly IAfadBasvuruTekilRepository _afadBasvuruTekilRepository;

        public GetirListeAfadBasvuruIlceQueryHandler(IAfadBasvuruTekilRepository afadBasvuruTekilRepository)
        {
            _afadBasvuruTekilRepository = afadBasvuruTekilRepository;
        }

        public async Task<ResultModel<List<SelectDto<int>>>> Handle(GetirListeAfadBasvuruIlceQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<List<SelectDto<int>>>();

            result.Result = _afadBasvuruTekilRepository
                                .GetWhere(x =>
                                    x.IlId == request.IlId
                                    &&
                                    x.IlceId != null
                                    &&
                                    !x.CsbSilindiMi
                                    &&
                                    x.CsbAktifMi == true,
                                    asNoTracking: true
                                )
                                .GroupBy(g => new
                                {
                                    g.IlceId,
                                    g.Ilce
                                })
                                .Select(s => new SelectDto<int>
                                {
                                    Id = s.Key.IlceId.Value,
                                    Ad = s.Key.Ilce
                                })
                                .OrderBy(o => o.Ad)
                                .ToList();

            return await Task.FromResult(result);
        }
    }
}