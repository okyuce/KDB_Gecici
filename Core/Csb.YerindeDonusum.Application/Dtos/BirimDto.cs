using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Application.Dtos;

public partial class BirimDto
{
    public long BirimId { get; set; }

    public string Ad { get; set; } = null!;

    public long KurumId { get; set; }

    public virtual KurumDto Kurum { get; set; } = null!;
}
