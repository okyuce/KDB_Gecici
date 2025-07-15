using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruAfadDurumCQRS;

public class GetirBasvuruAfadDurumListeQuery : IRequest<ResultModel<List<BasvuruDurumDto>>>
{
    public class GetirBasvuruAfadDurumListeQueryHandler : IRequestHandler<GetirBasvuruAfadDurumListeQuery, ResultModel<List<BasvuruDurumDto>>>
    {
        private readonly IBasvuruAfadDurumRepository _basvuruAfadDurumRepository;
        
        public GetirBasvuruAfadDurumListeQueryHandler(IBasvuruAfadDurumRepository basvuruAfadDurumRepository)
        {
            _basvuruAfadDurumRepository = basvuruAfadDurumRepository;
        }
        
        public Task<ResultModel<List<BasvuruDurumDto>>> Handle(GetirBasvuruAfadDurumListeQuery request, CancellationToken cancellationToken)
        {
            var result = _basvuruAfadDurumRepository.GetAllQueryable(x=> x.SilindiMi == false && x.AktifMi == true)
                              .Select(x => new BasvuruDurumDto()
                              {
                                  BasvuruDurumId = x.BasvuruAfadDurumId,
                                  Ad = x.Ad,
                              }).ToList();
            return Task.FromResult(new ResultModel<List<BasvuruDurumDto>>(result));
        }
    }
}