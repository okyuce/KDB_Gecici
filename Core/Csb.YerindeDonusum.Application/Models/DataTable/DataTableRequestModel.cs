using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Models.DataTable
{
    public class DataTableRequestModel
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
    }
}
