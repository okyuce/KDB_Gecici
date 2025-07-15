using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities;

public partial class BinaMuteahhit
{
    public long BinaMuteahhitId { get; set; }

    public long BinaDegerlendirmeId { get; set; }

    public string Adsoyadunvan { get; set; } = null!;

    public string? Adres { get; set; }

    public string? CepTelefonu { get; set; }

    public string? Telefon { get; set; }

    public string? Eposta { get; set; }

    public string VergiKimlikNo { get; set; } = null!;

    public string? YetkiBelgeNo { get; set; }

    public string? Aciklama { get; set; }

    public string? IbanNo { get; set; }

    public string? OlusturanIp { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public long OlusturanKullaniciId { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public long? GuncelleyenKullaniciId { get; set; }

    public string? GuncelleyenIp { get; set; }

    public bool? AktifMi { get; set; }

    public bool SilindiMi { get; set; }

    public long BinaMuteahhitTapuTurId { get; set; }

    public virtual BinaDegerlendirme BinaDegerlendirme { get; set; } = null!;

    public virtual BinaMuteahhitTapuTur BinaMuteahhitTapuTur { get; set; } = null!;
}
