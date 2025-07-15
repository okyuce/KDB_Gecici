using System.ComponentModel.DataAnnotations;

namespace Csb.YerindeDonusum.Application.Enums;

public enum TuzelKisilikEnum
{
    [Display(Name = "Tanımsız")]
    None = 0,

    [Display(Name = "Şirket")]
    SIRKET = 1,

    [Display(Name = "Dernek")]
    DERNEK = 2,

    [Display(Name = "Vakıf")]
    VAKIF = 3,

    [Display(Name = "Kooperatif")]
    KOOPERATIF = 4,
    [Display(Name = "Sendika")]
    SENDIKA = 5
}
