using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class TebligatGonderim
{
    public long TebligatGonderimId { get; set; }

    public long? GonderenKullaniciId { get; set; }

    public DateTime? GonderimTarihi { get; set; }

    public bool? GonderimBasariliMi { get; set; }

    public string? GonderimAciklama { get; set; }

    public long? EdevletTakipId { get; set; }

    public virtual Kullanici? GonderenKullanici { get; set; }

    public virtual ICollection<TebligatGonderimDetay> TebligatGonderimDetays { get; set; } = new List<TebligatGonderimDetay>();
}
