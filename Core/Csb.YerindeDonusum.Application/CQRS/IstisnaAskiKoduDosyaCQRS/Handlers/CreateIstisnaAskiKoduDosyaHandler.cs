using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using System;
using System.Threading;
using Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Commands;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Dtos;


namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Handlers
{
    public class CreateIstisnaAskiKoduDosyaHandler : IRequestHandler<CreateIstisnaAskiKoduDosyaCommand, CreateIstisnaAskiKoduDosyaResponseModel>
    {
        private readonly IIstisnaAskiKoduDosyaRepository _repo;
        private readonly IMapper _mapper;

        public CreateIstisnaAskiKoduDosyaHandler(IIstisnaAskiKoduDosyaRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CreateIstisnaAskiKoduDosyaResponseModel> Handle(CreateIstisnaAskiKoduDosyaCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<IstisnaAskiKoduDosya>(request.Model);
            await _repo.AddAsync(entity);
            return new CreateIstisnaAskiKoduDosyaResponseModel { Id = entity.IstisnaAskiKoduDosyaId };
        }
    }
}