using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Commands;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Handlers
{
    public class UpdateIstisnaAskiKoduDosyaHandler : IRequestHandler<UpdateIstisnaAskiKoduDosyaCommand, UpdateIstisnaAskiKoduDosyaResponseModel>
    {
        private readonly IIstisnaAskiKoduDosyaRepository _repo;
        private readonly IMapper _mapper;

        public UpdateIstisnaAskiKoduDosyaHandler(IIstisnaAskiKoduDosyaRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UpdateIstisnaAskiKoduDosyaResponseModel> Handle(UpdateIstisnaAskiKoduDosyaCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<IstisnaAskiKoduDosya>(request.Model);
            await _repo.UpdateAsync(entity);
            return new UpdateIstisnaAskiKoduDosyaResponseModel { Id = entity.IstisnaAskiKoduDosyaId };
        }
    }
}
