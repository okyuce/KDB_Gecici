namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetYapiRuhsat;

public class KaydetYapiRuhsatCommandResponseModel
{
    public string? Mesaj { get; set; }
    public string? DosyaAdi { get; set; }
    public Guid? DosyaGuid { get; set; }
}