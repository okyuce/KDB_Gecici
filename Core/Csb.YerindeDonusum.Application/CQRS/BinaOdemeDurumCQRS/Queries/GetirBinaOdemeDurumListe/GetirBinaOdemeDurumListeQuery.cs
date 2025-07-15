using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BinaOdemeDurumCQRS.Queries.GetirBinaOdemeDurumListe;

public class GetirBinaOdemeDurumListeQuery : IRequest<ResultModel<List<DurumDto>>>
{
    public class GetirBinaOdemeDurumListeQueryHandler : IRequestHandler<GetirBinaOdemeDurumListeQuery, ResultModel<List<DurumDto>>>
    {
        private readonly IBinaOdemeDurumRepository _binaOdemeDurumRepository;

        public GetirBinaOdemeDurumListeQueryHandler(IBinaOdemeDurumRepository binaOdemeDurumRepository)
        {
            _binaOdemeDurumRepository = binaOdemeDurumRepository;
        }

        public async Task<ResultModel<List<DurumDto>>> Handle(GetirBinaOdemeDurumListeQuery request, CancellationToken cancellationToken)
        {
            var result = _binaOdemeDurumRepository.GetAllQueryable(x => x.SilindiMi == false && x.AktifMi == true)
                              .Select(x => new DurumDto()
                              {
                                  Id = x.BinaOdemeDurumId,
                                  Ad = x.Ad,
                              }).ToList();

            return await Task.FromResult(new ResultModel<List<DurumDto>>(result));


        }
    }
}