using System.ComponentModel.DataAnnotations;

namespace Csb.YerindeDonusum.Application.Enums;

public enum BinaDegerlendirmeDosyaTurEnum : long
{
    [Display(Name = "Bina Değerlendirme")]
    BinaDegerlendirme = 1,

    [Display(Name = "Ada Parsel Güncelleme")]
    AdaParselGuncelleme = 2,

    [Display(Name = "Yapı Silme")]
    YapiSilme = 3,
}