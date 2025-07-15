using Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Commands;
using Csb.YerindeDonusum.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Handlers
{
    public class DeleteIstisnaAskiKoduDosyaHandler : IRequestHandler<DeleteIstisnaAskiKoduDosyaCommand, DeleteIstisnaAskiKoduDosyaResponseModel>
    {
        private readonly IIstisnaAskiKoduDosyaRepository _repo;

        public DeleteIstisnaAskiKoduDosyaHandler(IIstisnaAskiKoduDosyaRepository repo)
        {
            _repo = repo;
        }

        public async Task<DeleteIstisnaAskiKoduDosyaResponseModel> Handle(DeleteIstisnaAskiKoduDosyaCommand request, CancellationToken cancellationToken)
        {
            await _repo.SoftDeleteAsync(request.Id);
            return new DeleteIstisnaAskiKoduDosyaResponseModel { Success = true };
        }
    }
}