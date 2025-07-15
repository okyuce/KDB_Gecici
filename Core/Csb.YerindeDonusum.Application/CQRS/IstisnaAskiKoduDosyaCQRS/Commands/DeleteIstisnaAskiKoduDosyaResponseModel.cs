using Csb.YerindeDonusum.Application.CQRS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Commands
{
    public class DeleteIstisnaAskiKoduDosyaResponseModel : BaseCommandResponse { public bool Success { get; set; } }
}
