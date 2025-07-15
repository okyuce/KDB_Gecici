
using System.ComponentModel.DataAnnotations;

namespace Csb.YerindeDonusum.Application.Enums;

public enum AdaParselGuncellemeTipiEnum : long
{
    [Display(Name = "Tevhid")]
    Tevhid = 1,

    [Display(Name = "İfraz")]
    Ifraz = 2,

    [Display(Name = "Başka Parsel")]
    BaskaParsel = 3,

    [Display(Name = "3402 sayılı Kanun 22-A")]
    Kanun22A = 4,
}