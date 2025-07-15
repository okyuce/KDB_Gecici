using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.RolCQRS;

public class GetirRolListeQuery : IRequest<ResultModel<List<RolDto>>>
{
    public class GetirRoluListeQueryHandler : IRequestHandler<GetirRolListeQuery, ResultModel<List<RolDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IRolRepository _repository;
        
        public GetirRoluListeQueryHandler(IMapper mapper, IRolRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        
        public Task<ResultModel<List<RolDto>>> Handle(GetirRolListeQuery request, CancellationToken cancellationToken)
        {
            var result = _mapper.ProjectTo<RolDto>(_repository.GetWhere(x=>x.RolId!=1).OrderBy(o => o.Ad)).ToList();
            return Task.FromResult(new ResultModel<List<RolDto>>(result));
        }
    }
}