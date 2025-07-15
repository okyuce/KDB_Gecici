namespace Csb.YerindeDonusum.Application.Enums;

public static class BasvuruTurEnum
{
    private static long konut = 1;
    private static long ticarethane = 2;
    private static long ahirlikonut = 3;

    private static int evHibeTutari = 750000;
    private static int evKrediTutari = 750000;

    private static int ahirlikonutHibeTutari = 750000;
    private static int ahirlikonutKrediTutari = 1000000;

    private static int ticarethaneHibeTutari = 400000;
    private static int ticarethaneKrediTutari = 400000;

    public static long Konut { get => konut; }
    public static long Ticarethane { get => ticarethane; }
    public static long AhirliKonut { get => ahirlikonut; }
    public static int EvHibeTutari { get => evHibeTutari; }
    public static int EvKrediTutari { get => evKrediTutari; }
    public static int TicarethaneHibeTutari { get => ticarethaneHibeTutari; }
    public static int TicarethaneKrediTutari { get => ticarethaneKrediTutari; }
    public static int AhirlikonutHibeTutari { get => ahirlikonutHibeTutari; }
    public static int AhirlikonutKrediTutari { get => ahirlikonutKrediTutari; }
}