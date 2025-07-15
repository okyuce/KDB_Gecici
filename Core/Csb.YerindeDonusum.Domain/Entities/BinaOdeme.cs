using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaOdeme
{
    public long BinaOdemeId { get; set; }

    public long BinaDegerlendirmeId { get; set; }

    public int Seviye { get; set; }

    public decimal? OdemeTutari { get; set; }

    public long BinaOdemeDurumId { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public long OlusturanKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public decimal? HibeOdemeTutari { get; set; }

    public decimal? KrediOdemeTutari { get; set; }

    public string TalepNumarasi { get; set; } = null!;

    public DateTime? TalepKapatmaTarihi { get; set; }

    public string? TalepDurumu { get; set; }

    public decimal? DigerHibeOdemeTutari { get; set; }

    public virtual BinaDegerlendirme BinaDegerlendirme { get; set; } = null!;

    public virtual ICollection<BinaOdemeDetay> BinaOdemeDetays { get; set; } = new List<BinaOdemeDetay>();

    public virtual BinaOdemeDurum BinaOdemeDurum { get; set; } = null!;

    public virtual Kullanici? GuncelleyenKullanici { get; set; }

    public virtual Kullanici OlusturanKullanici { get; set; } = null!;
}
