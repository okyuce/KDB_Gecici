namespace Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiBilgileriTcDen;

public class GetirKisiBilgileriTcDenQueryResponseModel
{
    public string Ad { get; set; }
    public string Soyad { get; set; }
    public DateOnly DogumTarih { get; set; }
    public DateOnly? OlumTarih { get; set; }
}