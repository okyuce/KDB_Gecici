using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BasvuruTurCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruTurCQRS;

public class GetirBasvuruKanalListeQuery : IRequest<ResultModel<List<BasvuruKanalDto>>>
{
    public class GetirBasvuruKanalListeQueryHandler : IRequestHandler<GetirBasvuruKanalListeQuery, ResultModel<List<BasvuruKanalDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruKanalRepository _repository;
        
        public GetirBasvuruKanalListeQueryHandler(IMapper mapper, IBasvuruKanalRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        
        public Task<ResultModel<List<BasvuruKanalDto>>> Handle(GetirBasvuruKanalListeQuery request, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<List<BasvuruKanalDto>>(_repository.GetAll().ToList());
            return Task.FromResult(new ResultModel<List<BasvuruKanalDto>>(result));
        }
    }
}