using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirTebligatGonderimDetayDosyaByDetayId;

public class GetirTebligatGonderimDetayDosyaByDetayIdQuery : IRequest<ResultModel<TebligatGonderimDetayDosyaDto>>
{
    public long TebligatGonderimDetayId { get; set; }

    public class GetirTebligatGonderimDetayDosyaByDetayIdQueryHandler : IRequestHandler<GetirTebligatGonderimDetayDosyaByDetayIdQuery, ResultModel<TebligatGonderimDetayDosyaDto>>
    {
        private readonly ITebligatGonderimDetayDosyaRepository _tebligatGonderimDetayDosyaRepository;
        private readonly IMapper _mapper;
        
        public GetirTebligatGonderimDetayDosyaByDetayIdQueryHandler(IMapper mapper, ITebligatGonderimDetayDosyaRepository tebligatGonderimDetayDosyaRepository)
        {
            _tebligatGonderimDetayDosyaRepository = tebligatGonderimDetayDosyaRepository;
            _mapper = mapper;
        }

        public Task<ResultModel<TebligatGonderimDetayDosyaDto>> Handle(GetirTebligatGonderimDetayDosyaByDetayIdQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<TebligatGonderimDetayDosyaDto>();

            var tebligatDosya = _tebligatGonderimDetayDosyaRepository.GetWhere(x=> x.TebligatGonderimDetayId == request.TebligatGonderimDetayId && !x.SilindiMi && x.AktifMi == true, true).FirstOrDefault();

            if (tebligatDosya == null)
            {
                return Task.FromResult(result);
            }

            result.Result = _mapper.Map<TebligatGonderimDetayDosyaDto>(tebligatDosya);

            return Task.FromResult(result);
        }
    }
}
