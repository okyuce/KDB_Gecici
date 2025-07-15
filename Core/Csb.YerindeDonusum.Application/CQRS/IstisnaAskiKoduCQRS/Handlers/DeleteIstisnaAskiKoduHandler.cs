using Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Commands;
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
    public class DeleteIstisnaAskiKoduHandler : IRequestHandler<DeleteIstisnaAskiKoduCommand, DeleteIstisnaAskiKoduResponseModel>
    {
        private readonly IIstisnaAskiKoduRepository _repo;
        public DeleteIstisnaAskiKoduHandler(IIstisnaAskiKoduRepository repo) { _repo = repo; }
        public async Task<DeleteIstisnaAskiKoduResponseModel> Handle(DeleteIstisnaAskiKoduCommand request, CancellationToken cancellationToken)
        {
            await _repo.SoftDeleteAsync(request.Id);
            return new DeleteIstisnaAskiKoduResponseModel { Success = true };
        }
    }
}
