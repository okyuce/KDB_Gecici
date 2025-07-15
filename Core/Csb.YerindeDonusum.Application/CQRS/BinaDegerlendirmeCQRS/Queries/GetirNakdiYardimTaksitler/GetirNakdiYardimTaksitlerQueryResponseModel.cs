namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirNakdiYardimTaksitler;

public class GetirNakdiYardimTaksitlerQueryResponseModel
{
    public int? Seviye { get; set; }
    public decimal? HibeOdemeTutari { get; set; }
    public decimal? KrediOdemeTutari { get; set; }
    public decimal? DigerHibeOdemeTutari { get; set; }
    public string? FenniMesulTc { get; set; }
    public long? YibfNo { get; set; }
    public DateTime? TalepKapatmaTarihi { get; set; }
    public long? BinaOdemeDurumId { get; set; }
    public long? YapiKimlikNo { get; set; }
    public long? IzinBelgesiSayi { get; set; }
    public string? BinaOdemeDurumAd { get; set; }
    public string? MuteahhitYetkiBelgeNo { get; set; }
    public string? MuteahhitIbanNo { get; set; }
}
