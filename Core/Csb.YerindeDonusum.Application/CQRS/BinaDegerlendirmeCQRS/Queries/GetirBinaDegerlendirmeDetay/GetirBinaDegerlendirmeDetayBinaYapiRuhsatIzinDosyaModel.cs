using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeDetay;

public class GetirBinaDegerlendirmeDetayBinaYapiRuhsatIzinDosyaModel
{
    public string DosyaAdi { get; set; } = null!;
    public Guid BinaDosyaGuid { get; set; }
}