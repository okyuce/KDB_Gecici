namespace Csb.YerindeDonusum.Application.CQRS.YapiDenetimSeviyeCQRS.Queries;

public class GetirYapiDenetimSeviyeByYibfNoQueryResponseModel
{
    public string? RuhsatNo { get; set; }
    public decimal? Seviye { get; set; }
    public string? SantiyeSefi { get; set; }
    public string? YapiMuteahhiti { get; set; }
    public string? MutTicKayitNo { get; set; }
}