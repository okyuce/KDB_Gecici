using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciRolCQRS;

public class GetirKullaniciRolListeQuery : IRequest<ResultModel<List<KullaniciRolDto>>>
{
    public class GetirKullaniciRoluListeQueryHandler : IRequestHandler<GetirKullaniciRolListeQuery, ResultModel<List<KullaniciRolDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IKullaniciRolRepository _repository;
        
        public GetirKullaniciRoluListeQueryHandler(IMapper mapper, IKullaniciRolRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        
        public Task<ResultModel<List<KullaniciRolDto>>> Handle(GetirKullaniciRolListeQuery request, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<List<KullaniciRolDto>>(_repository.GetAll().ToList());
            return Task.FromResult(new ResultModel<List<KullaniciRolDto>>(result));
        }
    }
}