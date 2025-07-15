namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaIcinYapilanDigerYardimlar;

public class GetirBinaIcinYapilanDigerYardimlarQueryResponseModel
{
    public DateTime Tarih { get; set; }

    public long Tutar { get; set; }

    public string Adi { get; set; } = null!;
}