using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Csb.YerindeDonusum.Application.Enums;

public enum BasvuruDurumEnum : long
{
    [Display(Name = "Başvurunuz alınmıştır")]
    BasvurunuzAlinmistir = 5,

    [Display(Name = "Başvuru değerlendirme aşamasındadır")]
    BasvurunuzDegerlendirmeAsamasindadir = 1,

    [Display(Name = "Başvurunuz onaylanmıştır")]
    BasvurunuzOnaylanmistir = 6,

    [Display(Name = "Başvuru geçersizdir")]
    BasvurunuzGecersizdir = 2,

    [Display(Name = "Başvuru iptal edilmiştir")]
    BasvurunuzIptalEdilmistir = 4,

    [Display(Name = "Başvuru iptal edildi")]
    BasvuruIptalEdildi = 3,

    [Display(Name = "Başvurunuz reddedildilmiştir")]
    BasvuruReddedilmistir = 7,
}