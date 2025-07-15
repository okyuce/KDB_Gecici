using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class TebligatGonderimDetay
{
    public long TebligatGonderimDetayId { get; set; }

    public long? TebligatGonderimId { get; set; }

    public string? TcKimlikNo { get; set; }

    public string? HasarTespitAskiKodu { get; set; }

    public string? HasarTespitHasarDurumu { get; set; }

    public string? TapuIlAdi { get; set; }

    public string? TapuIlceAdi { get; set; }

    public string? TapuMahalleAdi { get; set; }

    public int? TapuTasinmazId { get; set; }

    public string? TapuAda { get; set; }

    public string? TapuParsel { get; set; }

    public int? TebligatTipiId { get; set; }

    public DateTime? TebligTarihi { get; set; }

    public string? TuzelKisiVergiNo { get; set; }

    public virtual TebligatGonderim? TebligatGonderim { get; set; }

    public virtual ICollection<TebligatGonderimDetayDosya> TebligatGonderimDetayDosyas { get; set; } = new List<TebligatGonderimDetayDosya>();
}
