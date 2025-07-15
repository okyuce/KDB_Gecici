namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.GuncelleYeniYapiDisKapiNo;

public class GuncelleYeniYapiDisKapiNoCommandResponseModel
{
    public string Mesaj { get; set; }
    public string DosyaAdi { get; set; } = null!;
    public Guid BinaDosyaGuid { get; set; }
}