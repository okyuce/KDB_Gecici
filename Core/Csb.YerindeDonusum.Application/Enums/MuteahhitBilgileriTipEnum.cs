
using System.ComponentModel.DataAnnotations;

namespace Csb.YerindeDonusum.Application.Enums;

public enum MuteahhitBilgileriTipEnum : long
{
    [Display(Name = "Müteahhit")]
    Muteahhit = 1,

    [Display(Name = "Tescil Dışı Arazi")]
    TescilDisi = 2,

    [Display(Name = "Hazine Arazisi")]
    HazineArazisi = 3,
}