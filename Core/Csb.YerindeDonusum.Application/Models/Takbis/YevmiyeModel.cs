using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Models.Takbis;

public class YevmiyeModel
{
    public decimal Id { get; set; }
    public string Kurum { get; set; }
    public DateTime Tarih { get; set; }
    public long YevmiyeNo { get; set; }
}
