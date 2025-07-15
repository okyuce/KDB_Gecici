using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruDurumCQRS;

public class GetirBasvuruDurumListeQuery : IRequest<ResultModel<List<BasvuruDurumDto>>>
{
    public class GetirBasvuruDurumListeQueryHandler : IRequestHandler<GetirBasvuruDurumListeQuery, ResultModel<List<BasvuruDurumDto>>>
    {
        private readonly IBasvuruDurumRepository _basvuruDurumRepository;
        
        public GetirBasvuruDurumListeQueryHandler(IBasvuruDurumRepository basvuruDurumRepository)
        {
            _basvuruDurumRepository = basvuruDurumRepository;
        }
        
        public Task<ResultModel<List<BasvuruDurumDto>>> Handle(GetirBasvuruDurumListeQuery request, CancellationToken cancellationToken)
        {
            var result = _basvuruDurumRepository.GetAllQueryable(x=> x.SilindiMi == false && x.AktifMi == true)
                              .Select(x => new BasvuruDurumDto()
                              {
                                  BasvuruDurumId = x.BasvuruDurumId,
                                  BasvuruDurumGuid = x.BasvuruDurumGuid,
                                  Ad = x.Ad,
                              }).ToList();
            return Task.FromResult(new ResultModel<List<BasvuruDurumDto>>(result));
        }
    }
}