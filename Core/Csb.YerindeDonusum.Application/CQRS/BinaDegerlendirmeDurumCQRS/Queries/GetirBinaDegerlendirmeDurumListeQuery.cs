using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeDurumCQRS.Queries;

public class GetirBinaDegerlendirmeDurumListeQuery : IRequest<ResultModel<List<BasvuruDurumDto>>>
{
    public class GetirBinaDegerlendirmeDurumListeQueryHandler : IRequestHandler<GetirBinaDegerlendirmeDurumListeQuery, ResultModel<List<BasvuruDurumDto>>>
    {
        private readonly IBinaDegerlendirmeDurumRepository _BinaDegerlendirmeDurumRepository;
        
        public GetirBinaDegerlendirmeDurumListeQueryHandler(IBinaDegerlendirmeDurumRepository BinaDegerlendirmeDurumRepository)
        {
            _BinaDegerlendirmeDurumRepository = BinaDegerlendirmeDurumRepository;
        }
        
        public Task<ResultModel<List<BasvuruDurumDto>>> Handle(GetirBinaDegerlendirmeDurumListeQuery request, CancellationToken cancellationToken)
        {
            var result = _BinaDegerlendirmeDurumRepository.GetAllQueryable(x=> x.SilindiMi == false && x.AktifMi == true)
                              .Select(x => new BasvuruDurumDto()
                              {
                                  BasvuruDurumId = x.BinaDegerlendirmeDurumId,
                                  Ad = x.Ad,
                              }).ToList();
            return Task.FromResult(new ResultModel<List<BasvuruDurumDto>>(result));
        }
    }
}