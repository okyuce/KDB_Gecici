namespace Csb.YerindeDonusum.Application.Enums;

public static class BasvuruDestekTurEnum
{
    private static long hibe = 1;
    private static long kredi = 2;
    private static long hibeVeKredi = 3;
    private static long malikUstlenecek = 4;

    public static long Hibe { get => hibe; }
    public static long Kredi { get => kredi; }
    public static long HibeVeKredi { get => hibeVeKredi; }
    public static long MalikUstlenecek { get => malikUstlenecek; }
}