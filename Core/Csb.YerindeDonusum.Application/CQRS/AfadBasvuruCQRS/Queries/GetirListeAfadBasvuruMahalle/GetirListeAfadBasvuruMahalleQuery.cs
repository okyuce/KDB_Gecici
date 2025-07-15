using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirListeAfadBasvuruMahalle;

public class GetirListeAfadBasvuruMahalleQuery : IRequest<ResultModel<List<SelectDto<int>>>>, ICacheMediatrQuery
{
    public int? IlceId { get; set; }

    #region Cache Ayar
    public bool? CacheCustomUser => null;
    public int? CacheMinute => null;
    public bool CacheIsActive => true;
    #endregion

    public class GetirListeAfadBasvuruMahalleQueryHandler : IRequestHandler<GetirListeAfadBasvuruMahalleQuery, ResultModel<List<SelectDto<int>>>>
    {
        private readonly IAfadBasvuruTekilRepository _afadBasvuruTekilRepository;

        public GetirListeAfadBasvuruMahalleQueryHandler(IAfadBasvuruTekilRepository afadBasvuruTekilRepository)
        {
            _afadBasvuruTekilRepository = afadBasvuruTekilRepository;
        }

        public async Task<ResultModel<List<SelectDto<int>>>> Handle(GetirListeAfadBasvuruMahalleQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<List<SelectDto<int>>>();

            result.Result = _afadBasvuruTekilRepository
                                .GetWhere(x =>
                                    x.IlceId == request.IlceId
                                    &&
                                    x.MahalleId != null
                                    &&
                                    !x.CsbSilindiMi
                                    &&
                                    x.CsbAktifMi == true,
                                    asNoTracking: true
                                )
                                .GroupBy(g => new
                                {
                                    g.MahalleId,
                                    g.Mahalle
                                })
                                .Select(s => new SelectDto<int>
                                {
                                    Id = s.Key.MahalleId.Value,
                                    Ad = s.Key.Mahalle
                                })
                                .OrderBy(o => o.Ad)
                                .ToList();

            return await Task.FromResult(result);
        }
    }
}