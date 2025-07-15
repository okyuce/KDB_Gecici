using Csb.YerindeDonusum.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Commands
{
    public class UpdateIstisnaAskiKoduCommand : IRequest<UpdateIstisnaAskiKoduResponseModel>
    {
        public IstisnaAskiKoduUpdateDto Model { get; set; }
    }
}
