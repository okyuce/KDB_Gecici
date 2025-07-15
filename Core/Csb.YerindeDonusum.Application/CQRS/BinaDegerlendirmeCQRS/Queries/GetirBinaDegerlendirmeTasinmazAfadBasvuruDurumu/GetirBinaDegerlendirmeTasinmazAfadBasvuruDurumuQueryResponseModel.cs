namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumu;

public class GetirBinaDegerlendirmeTasinmazAfadBasvuruDurumuQueryResponseModel
{
    public long BinaDegerlendirmeId { get; set; }
    public bool? TasinmazAfadBasvuruDurumu { get; set; }
}