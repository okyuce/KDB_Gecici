namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetBelge;

public class KaydetBelgeCommandResponseModel
{
    public string Mesaj { get; set; }
    public string DosyaAdi { get; set; } = null!;
    public Guid BinaDosyaGuid { get; set; }
}