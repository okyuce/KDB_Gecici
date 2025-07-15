using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Models
{
    public class EDevletTabligatResult
    {
        public List<long?> TakipIdList { get; set; }
        public string SonucKodu { get; set; }
        public string SonucAciklamasi { get; set; }
    }
}
