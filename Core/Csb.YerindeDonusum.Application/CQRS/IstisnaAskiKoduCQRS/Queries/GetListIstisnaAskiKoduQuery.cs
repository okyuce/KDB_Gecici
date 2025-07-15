using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models.DataTable;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.IstisnaAskiKoduCQRS.Queries
{
    /// <summary>
    /// DataTable için istisna aski kodu listeleme sorgusu.
    /// </summary>
    public class GetListIstisnaAskiKoduQuery : IRequest<DataTableResponseModel<List<IstisnaAskiKoduListItem>>>
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
    }
}
