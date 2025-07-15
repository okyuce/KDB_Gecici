using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeOdemeYapilanServerSide;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirBinaOdemeRapor;

public class GetirListeBinaOdemeRaporQueryResponseModel
{
    public long? BinaOdemeId { get; set; }
    public long? BinaDegerlendirmeId { get; set; }
    public long? UavtIlNo { get; set; }
    public string? UavtIlAdi { get; set; }
    public long? UavtIlceNo { get; set; }
    public long? BultenNo { get; set; }
    public string? UavtIlceAdi { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? UavtMahalleAdi { get; set; }
    public string? Ada { get; set; }
    public string? Parsel { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public long? YapiKimlikNo { get; set; }
    public long? IzinBelgesiSayi { get; set; }
    public DateTime? IzinBelgesiTarih { get; set; }
    public decimal? OdemeTutari { get; set; }
    public decimal? HibeOdemeTutari { get; set; }
    public decimal? KrediOdemeTutari { get; set; }
    public decimal? DigerHibeOdemeTutari { get; set; }
    public int Seviye { get; set; }
    public List<GetirListeOdemeYapilanServerSideResponseModel>? OdemeKimeYapildi { get; set; }
}