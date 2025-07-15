namespace Csb.YerindeDonusum.WebApp.Models
{
    public class NatamamYapiIslemViewModel
    {
        public long Id { get; set; }
        public string AskiKodu { get; set; }
        public string ResmiYaziDosyaAdi { get; set; }
        public string DigerYaziDosyaAdi { get; set; }
        public string OlusturanKullaniciAd { get; set; }
        public string GuncelleyenKullaniciAd { get; set; }
        public bool AktifMi { get; set; }
    }
}
