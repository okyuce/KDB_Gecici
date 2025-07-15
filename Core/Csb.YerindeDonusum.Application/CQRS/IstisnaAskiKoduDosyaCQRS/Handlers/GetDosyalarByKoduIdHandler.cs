using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Handlers
{
    public class GetDosyalarByKoduIdHandler : IRequestHandler<GetDosyalarByKoduIdQuery, IEnumerable<IstisnaAskiKoduDosyaDto>>
    {
        private readonly IIstisnaAskiKoduDosyaRepository _repo;
        private readonly IMapper _mapper;

        public GetDosyalarByKoduIdHandler(IIstisnaAskiKoduDosyaRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IstisnaAskiKoduDosyaDto>> Handle(GetDosyalarByKoduIdQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repo.GetByKoduIdAsync(request.IstisnaAskiKoduId);
            return _mapper.Map<IEnumerable<IstisnaAskiKoduDosyaDto>>(entities);
        }
    }
}
