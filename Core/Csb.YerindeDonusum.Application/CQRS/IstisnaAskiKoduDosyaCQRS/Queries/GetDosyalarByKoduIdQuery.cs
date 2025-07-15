using Csb.YerindeDonusum.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduDosyaCQRS.Queries
{
    public class GetDosyalarByKoduIdQuery : IRequest<IEnumerable<IstisnaAskiKoduDosyaDto>>
    {
        public long IstisnaAskiKoduId { get; set; }
    }
}
