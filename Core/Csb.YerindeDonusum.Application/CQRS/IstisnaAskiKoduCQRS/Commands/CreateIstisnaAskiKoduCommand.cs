using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Csb.YerindeDonusum.Application.Dtos;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Commands
{
    public class CreateIstisnaAskiKoduCommand : IRequest<CreateIstisnaAskiKoduResponseModel>
    {
        public IstisnaAskiKoduCreateDto Model { get; set; }
    }
}
