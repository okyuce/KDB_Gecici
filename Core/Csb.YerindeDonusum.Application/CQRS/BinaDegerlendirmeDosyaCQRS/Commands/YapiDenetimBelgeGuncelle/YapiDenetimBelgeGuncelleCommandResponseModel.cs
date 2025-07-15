namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeDosyaCQRS.Commands.YapiDenetimBelgeGuncelle;

public class YapiDenetimBelgeGuncelleCommandResponseModel
{
    public string Mesaj { get; set; }
    public string DosyaAdi { get; set; } = null!;
    public Guid BinaDosyaGuid { get; set; }
}