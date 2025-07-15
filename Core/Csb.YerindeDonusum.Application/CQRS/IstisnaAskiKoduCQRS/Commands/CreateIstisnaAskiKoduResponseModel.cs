using Csb.YerindeDonusum.Application.CQRS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Commands
{
    public class CreateIstisnaAskiKoduResponseModel : BaseCommandResponse
    {
        public long Id { get; set; }
    }
}
