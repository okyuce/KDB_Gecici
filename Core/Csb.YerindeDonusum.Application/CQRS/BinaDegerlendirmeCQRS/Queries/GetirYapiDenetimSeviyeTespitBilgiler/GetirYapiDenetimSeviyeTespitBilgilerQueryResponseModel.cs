namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirYapiDenetimSeviyeTespitBilgiler;

public class GetirYapiDenetimSeviyeTespitBilgilerQueryResponseModel
{
    public long? BinaYapiDenetimSeviyeTespitDosyaId { get; set; }
    public long IlerlemeYuzdesi { get; set; }
    public string DosyaAdi { get; set; } = null!;
    public Guid? BinaYapiDenetimDosyaGuid { get; set; }
}
