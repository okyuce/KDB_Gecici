using Csb.YerindeDonusum.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Queries
{
    public class GetIstisnaAskiKoduByIdQuery : IRequest<IstisnaAskiKoduDto>
    {
        public long Id { get; set; }
    }
}
