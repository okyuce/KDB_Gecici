using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.AydinlatmaMetniCQRS.Queries;

public class GetirAydinlatmaMetniQuery : IRequest<ResultModel<AydinlatmaMetniDto>>
{
    public class GetirAydinlatmaMetniQueryHandler : IRequestHandler<GetirAydinlatmaMetniQuery, ResultModel<AydinlatmaMetniDto>>
    {
        private readonly IMapper _mapper;
        private readonly IAydinlatmaMetniRepository _repository;

        public GetirAydinlatmaMetniQueryHandler(IMapper mapper, IAydinlatmaMetniRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        
        public Task<ResultModel<AydinlatmaMetniDto>> Handle(GetirAydinlatmaMetniQuery request, CancellationToken cancellationToken)
        {
            var clarificationTestByLastCreatedAndActiveAndNoDeletedRecord = _repository
                .GetWhere(x => x.AktifMi == true && x.SilindiMi == false, true).OrderByDescending(o => o.OlusturmaTarihi)
                .FirstOrDefault();
            
            var result = _mapper.Map<AydinlatmaMetniDto>(clarificationTestByLastCreatedAndActiveAndNoDeletedRecord);

            return Task.FromResult(new ResultModel<AydinlatmaMetniDto>(result));
        }
    }
}