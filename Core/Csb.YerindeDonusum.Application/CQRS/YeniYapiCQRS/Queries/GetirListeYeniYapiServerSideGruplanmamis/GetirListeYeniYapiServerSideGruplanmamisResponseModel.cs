using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeDetay;
using Csb.YerindeDonusum.Application.Dtos;

namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiServerSideGruplanmamis;

public class GetirListeYeniYapiServerSideGruplanmamisResponseModel
{
    public long BinaDegerlendirmeId { get; set; }
    public string? UavtIlAdi { get; set; }
    public string? UavtIlceAdi { get; set; }
    public string? UavtMahalleAdi { get; set; }
    public long? YapiKimlikNo { get; set; }
    public string? BinaDisKapiNo { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public string? AdaParsel { get; set; }
    public string? BasvuruDegerlendirmeDurumAd { get; set; }

    public long OlusturanKullaniciId { get; set; }
    public long? GuncelleyenKullaniciId { get; set; }
    public string? OlusturanKullaniciAdi { get; set; }
    public string? GuncelleyenKullaniciAdi { get; set; }
}