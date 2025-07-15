using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class SmsLog
{
    public int SmsLogId { get; set; }

    public string Telefon { get; set; } = null!;

    public string Icerik { get; set; } = null!;

    public string? ApiSmsId { get; set; }

    public bool GonderildiMi { get; set; }

    public string? ApiMesaj { get; set; }

    public DateTime OlusturmaTarihi { get; set; }
}
