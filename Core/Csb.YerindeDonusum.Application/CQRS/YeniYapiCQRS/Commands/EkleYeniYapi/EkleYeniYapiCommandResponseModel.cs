namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.EkleYeniYapi;

public class EkleYeniYapiCommandResponseModel
{
    public string Mesaj { get; set; }
    public string DosyaAdi { get; set; } = null!;
    public Guid BinaDosyaGuid { get; set; }
}