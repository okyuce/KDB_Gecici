using System;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Domain.Entities.Kds;

public partial class TblYapi
{
    public long TblYapiId { get; set; }

    public long YapiKod { get; set; }

    public long YapiKimlikno { get; set; }

    public string? DisKapiNo { get; set; }

    public string? BlokAdi { get; set; }

    public string? SiteAdi { get; set; }

    public string? PostaKodu { get; set; }

    public string? AdaNo { get; set; }

    public string? ParselNo { get; set; }

    public string? Pafta { get; set; }

    public string? YapiTipTanimKod { get; set; }

    public string? YapiDurumTanimKod { get; set; }

    public short? YolAltiKatSayisi { get; set; }

    public short? YolUstuKatSayisi { get; set; }

    public short? ToplamKatSayisi { get; set; }

    public bool? Aktif { get; set; }

    public long Versiyon { get; set; }

    public virtual TblTanim? YapiDurumTanimKodNavigation { get; set; }

    public virtual TblTanim? YapiTipTanimKodNavigation { get; set; }
}
