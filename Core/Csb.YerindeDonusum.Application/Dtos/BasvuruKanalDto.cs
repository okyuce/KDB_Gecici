using Csb.YerindeDonusum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Dtos;

public class BasvuruKanalDto
{
    public long? BasvuruKanalId { get; set; }

    public string? Ad { get; set; }

    public bool? AktifMi { get; set; }

    public bool? SilindiMi { get; set; }

    public Guid? BasvuruKanalGuid { get; set; }
}
