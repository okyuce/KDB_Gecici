using System.ComponentModel.DataAnnotations;

namespace Csb.YerindeDonusum.Application.Enums;

public enum BinaOdemeDurumEnum
{
    [Display(Name = "Ödeme bekleniyor")]
    Bekleniyor = 1,

    [Display(Name = "Ödeme isteği alındı")]
    IstekAlindi = 2,

    [Display(Name = "Ödeme onaylandı")]
    Onaylandi = 3,

    [Display(Name = "Ödeme reddedildi")]
    Reddedildi = 4,

    [Display(Name = "HYS’den ödeme talep edildi")]
    HYSAktarildi = 5,

    [Display(Name = "Ödeme müteahhit hesabına aktarıldı")]
    MuteahhiteAktarildi = 6,
}