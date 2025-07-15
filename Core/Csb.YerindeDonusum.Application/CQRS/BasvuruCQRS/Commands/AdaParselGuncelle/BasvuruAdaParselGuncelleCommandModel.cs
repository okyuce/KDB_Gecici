using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.AdaParselGuncelle;

public class BasvuruAdaParselGuncelleCommandModel
{
    public long? BinaDegerlendirmeId { get; set; }
    public string? TapuBeyanIlId { get; set; }
    public int? TapuBeyanIlceId { get; set; }
    public string? TapuBeyanIlceAdi { get; set; }
    public int? TapuBeyanMahalleId { get; set; }
    public string? TapuBeyanMahalleAdi { get; set; }
    public string? TapuBeyanAda { get; set; }
    public string? TapuBeyanParsel { get; set; }
    public AdaParselGuncellemeTipiEnum? AdaParselGuncellemeTipiId { get; set; }
    public DosyaDto? BaskaParselDosya { get; set; }
}