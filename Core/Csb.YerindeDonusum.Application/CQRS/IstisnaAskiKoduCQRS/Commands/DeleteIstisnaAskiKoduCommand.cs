using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Commands
{
    public class DeleteIstisnaAskiKoduCommand : IRequest<DeleteIstisnaAskiKoduResponseModel>
    {
        public long Id { get; set; }
    }
}
