using Csb.YerindeDonusum.Application.Enums;

namespace Csb.YerindeDonusum.Application.Models.Takbis
{
    public class HisseModel
    {
        public decimal Id { get; set; }
        public decimal TasinmazId { get; set; }
        public decimal IstirakNo { get; set; }
        public string Pay { get; set; }
        public string Payda { get; set; }
        public decimal MalikId { get; set; }
        public TakbisMalikTipEnum MalikTip { get; set; }
        public TapuBolumDurumEnum TapuBolumDurum { get; set; }
        public string MalikAd { get; set; }
        public string MalikSoyad { get; set; }
        public long? MalikTCNo { get; set; }
        public string MalikUnvan { get; set; }
        public string MalikVergiNo { get; set; }
        public long YevmiyeNo { get; set; }
        public DateTime YevmiyeTarihi { get; set; }
    }
}