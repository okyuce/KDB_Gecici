using System.ComponentModel.DataAnnotations;

namespace Csb.YerindeDonusum.Application.Enums;

public enum BoyutIlIdEnum : int
{
    [Display(Name = "Adana")]
    Adana = 1,

    [Display(Name = "Adıyaman")]
    Adiyaman = 2,

    [Display(Name = "Diyarbakır")]
    Diyarbakir = 21,

    [Display(Name = "Elazığ")]
    Elazig = 23,

    [Display(Name = "Gaziantep")]
    Gaziantep = 27,

    [Display(Name = "Hatay")]
    Hatay = 31,

    [Display(Name = "Malatya")]
    Malatya = 44,

    [Display(Name = "Kahramanmaraş")]
    Kahramanmaras = 46,

    [Display(Name = "Şanlıurfa")]
    Sanliurfa = 63,

    [Display(Name = "Kilis")]
    Kilis = 79,

    [Display(Name = "Osmaniye")]
    Osmaniye = 80,

    [Display(Name = "Batman")]
    Batman = 72,

    [Display(Name = "Kayseri")]
    Kayseri = 38,

    [Display(Name = "Mardin")]
    Mardin = 47,

    [Display(Name = "Niğde")]
    Nigde = 51,

    [Display(Name = "Sivas")]
    Sivas = 58,

    [Display(Name = "Tunceli")]
    Tunceli = 62,

    [Display(Name = "Bingöl")]
    Bingol = 12,
}