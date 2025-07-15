using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.Application.Models.Takbis;

public class AnaTasinmazModel
{
    public decimal Id { get; set; }
    public decimal AltTasinmazId { get; set; }
    //public TasinmazTipEnum Tip { get; set; }
    public string Il { get; set; }
    public string Ilce { get; set; }
    public string Kurum { get; set; }
    public int KurumId { get; set; }
    public string Mahalle { get; set; }
    public int MahalleId { get; set; }
    public string Mevkii { get; set; }
    public string Nitelik { get; set; }
    public string CiltNo { get; set; }
    public string SayfaNo { get; set; }
    public decimal YuzOlcum { get; set; }
    public string Pafta { get; set; }
    public string Ada { get; set; }
    public string Parsel { get; set; }
    public int Eklenti { get; set; }
    public int Teferruat { get; set; }
    public int Muhdesat { get; set; }
    public TapuBolumDurumEnum? TapuBolumDurum { get; set; }
    public DaimiMustakilHakModel? DaimiMustakilHak { get; set; }
    public BagimsizBolumModel? BagimsizBolum { get; set; } = null;
    public IslemModel? TerkinIslem { get; set; }
    public IslemModel? TesisIslem { get; set; }
}
