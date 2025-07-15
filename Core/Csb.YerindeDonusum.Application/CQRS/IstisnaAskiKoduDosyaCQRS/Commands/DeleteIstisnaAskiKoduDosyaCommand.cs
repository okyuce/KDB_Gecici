using Csb.YerindeDonusum.Application.CQRS.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Commands
{
    public class DeleteIstisnaAskiKoduDosyaCommand : IRequest<DeleteIstisnaAskiKoduDosyaResponseModel> { public long Id { get; set; } }
}
