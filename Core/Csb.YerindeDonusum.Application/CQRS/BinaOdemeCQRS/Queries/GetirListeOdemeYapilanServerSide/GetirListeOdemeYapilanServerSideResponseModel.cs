namespace Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeOdemeYapilanServerSide;

public class GetirListeOdemeYapilanServerSideResponseModel
{
    public string? Adi { get; set; }
    public string? Tipi { get; set; }
    public decimal? HibeOdemeTutari { get; set; }
    public decimal? KrediOdemeTutari { get; set; }
    public decimal? DigerOdemeTutari { get; set; }
    public decimal? OdemeTutari { get; set; }
    public string? IbanNo { get; set; }
}