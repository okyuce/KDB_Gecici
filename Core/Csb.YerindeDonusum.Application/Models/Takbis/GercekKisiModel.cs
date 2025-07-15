using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.Application.Models.Takbis;

public class GercekKisiModel
{
    public decimal? Id;
    public long? TcKimlikNo;
    public string? Ad;
    public string? Soyad;
    public string? BabaAd;
    public string? AnaAd;
    public DateTime? DogumTarih;
    public string? DogumYer;
    public string? Cilt;
    public string? Sira;
    public string? NufusCuzdaniSeriNo;
    //public CinsiyetEnum cinsiyetField;
    public DateTime? OlumTarih;
    public DurumEnum? Durum;
}