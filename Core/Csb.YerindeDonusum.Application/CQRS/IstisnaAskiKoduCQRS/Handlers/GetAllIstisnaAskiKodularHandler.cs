using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Handlers
{
    public class GetAllIstisnaAskiKodularHandler : IRequestHandler<GetAllIstisnaAskiKodularQuery, IEnumerable<IstisnaAskiKoduDto>>
    {
        private readonly IIstisnaAskiKoduRepository _repo;
        private readonly IMapper _mapper;

        public GetAllIstisnaAskiKodularHandler(IIstisnaAskiKoduRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IstisnaAskiKoduDto>> Handle(GetAllIstisnaAskiKodularQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<IstisnaAskiKoduDto>>(entities);
        }
    }
}
