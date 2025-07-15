using System.ComponentModel.DataAnnotations;

namespace Csb.YerindeDonusum.Application.Enums;

public enum BinaDegerlendirmeDurumEnum
{
    [Display(Name = "Onay bekliyor")]
    OnayBekliyor = 1,

    [Display(Name = "Başvurunuz onaylanmıştır")]
    Onaylandi = 2,

    [Display(Name = "Müteahhit atamanız gerçekleşmiştir")]
    MuteahhitAtamanizGerceklesmistir = 3,

    [Display(Name = "Başvurunuz reddedildilmiştir")]
    Reddedildi = 4,

    [Display(Name = "Başvurunuz değerlendirmeye alınmıştır")]
    BasvurunuzDegerlendirmeyeAlinmistir = 5,

    [Display(Name = "İnşaat yapım taahhüt sözleşmesi imzalanmıştır")]
    InsaatSozlesmesiImzalanmistir = 6,

    [Display(Name = "Yapı ruhsatınız onaylanmıştır")]
    YapiRuhsatinizOnaylanmistir = 7,

    [Display(Name = "Kamu Üstlenecek")]
    KamuUstlenecek = 8,

    [Display(Name = "Yapı ilerleme seviyesi %20")]
    YapiIlerlemeSeviyesiYuzde20 = 9,

    [Display(Name = "Yapı ilerleme seviyesi %60")]
    YapiIlerlemeSeviyesiYuzde60 = 10,

    [Display(Name = "Yapı tamamlanmıştır")]
    YapiTamamlanmistir = 11
}