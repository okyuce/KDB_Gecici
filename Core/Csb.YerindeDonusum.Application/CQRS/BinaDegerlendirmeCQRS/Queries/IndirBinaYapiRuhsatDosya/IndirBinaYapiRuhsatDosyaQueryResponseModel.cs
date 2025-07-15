namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.IndirBinaYapiRuhsatDosya;

public class IndirBinaYapiRuhsatDosyaQueryResponseModel
{
    public byte[] File { get; set; }
    public string MimeType { get; set; }
    public string DosyaAdi { get; set; } = null!;
}
