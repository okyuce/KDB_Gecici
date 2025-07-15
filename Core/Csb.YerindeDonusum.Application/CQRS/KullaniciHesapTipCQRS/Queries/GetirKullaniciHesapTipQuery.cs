using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciHesapTipCQRS;

public class GetirKullaniciHesapTipListeQuery : IRequest<ResultModel<List<KullaniciHesapTipDto>>>
{
    public class GetirKullaniciHesapTipuListeQueryHandler : IRequestHandler<GetirKullaniciHesapTipListeQuery, ResultModel<List<KullaniciHesapTipDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IKullaniciHesapTipRepository _repository;
        
        public GetirKullaniciHesapTipuListeQueryHandler(IMapper mapper, IKullaniciHesapTipRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        
        public Task<ResultModel<List<KullaniciHesapTipDto>>> Handle(GetirKullaniciHesapTipListeQuery request, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<List<KullaniciHesapTipDto>>(_repository.GetAll().ToList());
            return Task.FromResult(new ResultModel<List<KullaniciHesapTipDto>>(result));
        }
    }
}