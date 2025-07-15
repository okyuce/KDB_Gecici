using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Commands;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Handlers
{
    public class UpdateIstisnaAskiKoduHandler : IRequestHandler<UpdateIstisnaAskiKoduCommand, UpdateIstisnaAskiKoduResponseModel>
    {
        private readonly IIstisnaAskiKoduRepository _repo;
        private readonly IMapper _mapper;
        public UpdateIstisnaAskiKoduHandler(IIstisnaAskiKoduRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
        public async Task<UpdateIstisnaAskiKoduResponseModel> Handle(UpdateIstisnaAskiKoduCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<IstisnaAskiKodu>(request.Model);
            await _repo.UpdateAsync(entity);
            return new UpdateIstisnaAskiKoduResponseModel { Id = entity.IstisnaAskiKoduId };
        }
    }
}
