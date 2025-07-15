using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.AydinlatmaMetniCQRS.Queries;

public class GetirAyarQuery : IRequest<ResultModel<AyarDto>>
{
    public class GetirAyarQueryHandler : IRequestHandler<GetirAyarQuery, ResultModel<AyarDto>>
    {
        private readonly IMapper _mapper;
        private readonly IAyarRepository _repository;

        public GetirAyarQueryHandler(IMapper mapper, IAyarRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        
        public Task<ResultModel<AyarDto>> Handle(GetirAyarQuery request, CancellationToken cancellationToken)
        {
            var ayar = _repository
                .GetWhere(x => x.AktifMi == true && x.SilindiMi == false, true)
                .OrderByDescending(o => o.AyarId)
                .FirstOrDefault();
            
            var result = _mapper.Map<AyarDto>(ayar);

            return Task.FromResult(new ResultModel<AyarDto>(result));
        }
    }
}