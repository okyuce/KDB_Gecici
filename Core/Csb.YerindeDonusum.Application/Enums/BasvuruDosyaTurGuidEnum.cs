using System.ComponentModel;

namespace Csb.YerindeDonusum.Application.Enums;

public class BasvuruDosyaTurGuidEnum
{
    private static Guid binaFotografi = new Guid("cce38eac-f04b-48d3-8c16-5212fa4cabd5");
    private static Guid tapuFotografi = new Guid("57c56e71-07d7-40bc-bc88-0ecccc744c5a");
    private static Guid tuzelKisiBaskaninDilekcesi = new Guid("4627aff6-a6e5-462a-b1cb-24517592c1e4");
    private static Guid tuzelKisilikYetkiliOldugunuGosterirBelge = new Guid("ba9e5bb5-f152-4b85-a719-240a65f5801f");
    private static Guid hazineArazisiMuhtarBeyanBelgesi = new Guid("0c3e95ff-b31b-4aa4-8e02-47a9fed24532");

    public static Guid BinaFotografi { get => binaFotografi; }
    public static Guid TapuFotografi { get => tapuFotografi; }
    public static Guid TuzelKisiBaskaninDilekcesi { get => tuzelKisiBaskaninDilekcesi; }
    public static Guid TuzelKisilikYetkiliOldugunuGosterirBelge { get => tuzelKisilikYetkiliOldugunuGosterirBelge; }
    public static Guid HazineArazisiMuhtarBeyanBelgesi { get => hazineArazisiMuhtarBeyanBelgesi; }
}