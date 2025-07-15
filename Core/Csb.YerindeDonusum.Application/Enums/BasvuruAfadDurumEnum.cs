using System.ComponentModel.DataAnnotations;

namespace Csb.YerindeDonusum.Application.Enums;

public enum BasvuruAfadDurumEnum : long
{
    [Display(Name = "Kabul")]
    Kabul = 1,

    [Display(Name = "Ret")]
    Red = 2,

    [Display(Name = "Beklemede")]
    Beklemede = 3,

    [Display(Name = "Başvurusu Yok")]
    BasvuruYok = 4,

    [Display(Name = "İptal")]
    Iptal = 5,

    [Display(Name = "İptal Edilmiştir")]
    IptalEdilmistir = 6,
}