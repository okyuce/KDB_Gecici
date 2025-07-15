using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Dtos;

public class DosyaDto
{
    //public string? FileName { get; set; }
    public string? DosyaUzanti { get; set; }
    public string? DosyaBase64 { get; set; }
    public Guid? DosyaTurGuid { get; set; }
}
