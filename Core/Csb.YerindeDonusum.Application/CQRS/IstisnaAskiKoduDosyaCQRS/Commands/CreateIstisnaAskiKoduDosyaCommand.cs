using Csb.YerindeDonusum.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Commands
{
    public class CreateIstisnaAskiKoduDosyaCommand : IRequest<CreateIstisnaAskiKoduDosyaResponseModel>
    {
        public IstisnaAskiKoduDosyaCreateDto Model { get; set; }
    }
}
