namespace Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiAdSoyadTcDen;

public class GetirKisiAdSoyadTcDenQueryResponseModel
{
    public string Ad { get; set; }
    public string Soyad { get; set; }
    public DateOnly? OlumTarih { get; set; }
}