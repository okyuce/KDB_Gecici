namespace Csb.YerindeDonusum.Application.Enums;

public static class HasarTespitItirazSonucuEnum
{
    private static long hasarsiz = 1;
    private static long azHasarli = 2;
    private static long agirHasarli = 3;
    private static long yikik = 4;
    private static long acilYiktirilacak = 5;
    private static long degerlendirmeDisi = 6;
    private static long ortaHasarli = 10;
    private static long hasaraItirazYoktur = 23;
    private static long tespitYapilmadi = 24;
    private static long binaKilitliIncelemeYapilamadiGirilemedi = 25;

    public static long Hasarsiz { get => hasarsiz; }
    public static long AzHasarli { get => azHasarli; }
    public static long AgirHasarli { get => agirHasarli; }
    public static long Yikik { get => yikik; }
    public static long AcilYiktirilacak { get => acilYiktirilacak; }
    public static long DegerlendirmeDisi { get => degerlendirmeDisi; }
    public static long OrtaHasarli { get => ortaHasarli; }
    public static long HasaraItirazYoktur { get => hasaraItirazYoktur; }
    public static long TespitYapilmadi { get => tespitYapilmadi; }
    public static long BinaKilitliIncelemeYapilamadiGirilemedi { get => binaKilitliIncelemeYapilamadiGirilemedi; }
}