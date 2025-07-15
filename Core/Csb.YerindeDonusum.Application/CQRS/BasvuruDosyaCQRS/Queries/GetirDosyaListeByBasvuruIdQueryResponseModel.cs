using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruDosyaCQRS.Queries
{
    public class GetirDosyaListeByBasvuruIdQueryResponseModel
    {
        public Guid Id { get; set; }

        public string DosyaAdi { get; set; }
    }
}
