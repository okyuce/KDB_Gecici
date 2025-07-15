using System.ComponentModel.DataAnnotations;

namespace Csb.YerindeDonusum.Application.Enums;

public enum BasvuruImzaVerenDosyaTurEnum : long
{
    [Display(Name = "Kredi Sözleşmesi")]
    KrediSozlesmesi = 1,

    [Display(Name = "Hibe Onayı")]
    HibeOnayi = 2,

    [Display(Name = "Taahhütname Belgesi")]
    TaahhutnameBelgesi = 3,
}