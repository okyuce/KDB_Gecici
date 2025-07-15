using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.AydinlatmaMetniCQRS.Queries;

public class GetirAydinlatmaMetinListeQuery : IRequest<ResultModel<List<AydinlatmaMetniDto>>>
{
    public class GetirAydinlatmaMetinListeQueryHandler : IRequestHandler<GetirAydinlatmaMetinListeQuery, ResultModel<List<AydinlatmaMetniDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IAydinlatmaMetniRepository _repository;
        
        public GetirAydinlatmaMetinListeQueryHandler(IMapper mapper, IAydinlatmaMetniRepository clarificationTextRepository)
        {
            _mapper = mapper;
            _repository = clarificationTextRepository;
        }
        
        public Task<ResultModel<List<AydinlatmaMetniDto>>> Handle(GetirAydinlatmaMetinListeQuery request, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<List<AydinlatmaMetniDto>>(_repository.GetAll().ToList());

            return Task.FromResult(new ResultModel<List<AydinlatmaMetniDto>>(result));
        }
    }
}