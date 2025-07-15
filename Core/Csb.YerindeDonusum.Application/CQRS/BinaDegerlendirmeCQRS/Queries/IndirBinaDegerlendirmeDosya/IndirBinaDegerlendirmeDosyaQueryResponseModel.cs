namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.IndirBinaDegerlendirmeDosya;

public class IndirBinaDegerlendirmeDosyaQueryResponseModel
{
    public byte[] File { get; set; }
    public string MimeType { get; set; }
    public string DosyaAdi { get; set; } = null!;
}
