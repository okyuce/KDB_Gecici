using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class AfadGirisBilgi
{
    public long AfadGirisBilgiId { get; set; }

    public string AccessToken { get; set; } = null!;

    public string? TokenTuru { get; set; }

    public string RefreshToken { get; set; } = null!;

    public DateTime GirisTarihi { get; set; }

    /// <summary>
    /// saniye
    /// </summary>
    public int GecerlilikSuresi { get; set; }
}
