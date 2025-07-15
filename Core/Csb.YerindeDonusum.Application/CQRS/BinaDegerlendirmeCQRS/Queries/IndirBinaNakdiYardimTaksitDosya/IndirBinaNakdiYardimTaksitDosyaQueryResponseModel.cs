using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.IndirBinaNakdiYardimTaksitDosya;

public class IndirBinaNakdiYardimTaksitDosyaQueryResponseModel
{
    public byte[] File { get; set; }
    public string MimeType { get; set; }
    public string DosyaAdi { get; set; } = null!;
}
