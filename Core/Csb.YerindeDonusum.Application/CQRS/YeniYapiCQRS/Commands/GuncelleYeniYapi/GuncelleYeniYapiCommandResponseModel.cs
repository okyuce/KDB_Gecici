namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.GuncelleYeniYapi;

public class GuncelleYeniYapiCommandResponseModel
{
    public string Mesaj { get; set; }
    public string DosyaAdi { get; set; } = null!;
    public Guid BinaDosyaGuid { get; set; }
}