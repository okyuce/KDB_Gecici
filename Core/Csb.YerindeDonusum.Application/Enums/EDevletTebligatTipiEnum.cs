
using System.ComponentModel.DataAnnotations;

namespace Csb.YerindeDonusum.Application.Enums;

public enum EDevletTebligatTipiEnum : long
{
    [Display(Name = "Anahtar Teslim KampanyaId")]
    AnahtarTeslimKampanyaId = 1,

    [Display(Name = "İştirak KampanyaId")]
    IstirakKampanyaId = 2
}