using Csb.YerindeDonusum.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Commands
{
    public class UpdateIstisnaAskiKoduDosyaCommand : IRequest<UpdateIstisnaAskiKoduDosyaResponseModel>
    {
        public IstisnaAskiKoduDosyaUpdateDto Model { get; set; }
    }
}
