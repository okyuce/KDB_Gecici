using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class IstisnaAskiKodu
{
    public long IstisnaAskiKoduId { get; set; }
    public string AskiKodu { get; set; }
    public bool AktifMi { get; set; }
    public bool SilindiMi { get; set; }
    public string OlusturanIp { get; set; }
    public DateTime OlusturmaTarihi { get; set; }
    public long OlusturanKullaniciId { get; set; }
    public string GuncelleyenIp { get; set; }
    public DateTime? GuncellemeTarihi { get; set; }
    public long? GuncelleyenKullaniciId { get; set; }

    // Navigation properties
    public virtual ICollection<IstisnaAskiKoduDosya> Dosyalar { get; set; } = new List<IstisnaAskiKoduDosya>();
    public virtual Kullanici OlusturanKullanici { get; set; }
    public virtual Kullanici GuncelleyenKullanici { get; set; }
}
