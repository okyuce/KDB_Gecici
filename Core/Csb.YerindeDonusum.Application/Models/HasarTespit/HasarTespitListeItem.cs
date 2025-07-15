using Newtonsoft.Json; 
using System; 
namespace Csb.YerindeDonusum.Application.Models.HasarTespit{ 

    public class HasarTespitListeItem
    {
        [JsonProperty("icKapiNo")]
        public string IcKapiNo { get; set; }

        [JsonProperty("askiBaskiDetay_askiBaski_yigmIlce_tanim")]
        public string AskiBaskiDetayAskiBaskiYigmIlceTanim { get; set; }

        [JsonProperty("askiBaskiDetay_id")]
        public int AskiBaskiDetayId { get; set; }

        [JsonProperty("askiBaskiDetay_askiBaski_yigmMahalle_mahalleTipi_tanim")]
        public string AskiBaskiDetayAskiBaskiYigmMahalleMahalleTipiTanim { get; set; }

        [JsonProperty("askiBaskiDetay_askiBaski_yigmIlce_id")]
        public int AskiBaskiDetayAskiBaskiYigmIlceId { get; set; }

        [JsonProperty("askiBaskiDetay_askiBaski_yigmMahalle_tanim")]
        public string AskiBaskiDetayAskiBaskiYigmMahalleTanim { get; set; }

        [JsonProperty("askiBaskiDetay_itirazHasar_tanim")]
        public string AskiBaskiDetayItirazHasarTanim { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("askiBaskiDetay_askiBaski_yigmMahalle_id")]
        public int AskiBaskiDetayAskiBaskiYigmMahalleId { get; set; }

        [JsonProperty("askiBaskiDetay_kesinHasar_id")]
        public int AskiBaskiDetayKesinHasarId { get; set; }

        [JsonProperty("askiBaskiDetay_sokak")]
        public string AskiBaskiDetaySokak { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("konutTipiStr")]
        public string KonutTipiStr { get; set; }

        [JsonProperty("askiBaskiDetay_askiKodu")]
        public string AskiBaskiDetayAskiKodu { get; set; }

        [JsonProperty("askiBaskiDetay_askiBaski_yigmMahalle_mahalleTipi_id")]
        public int AskiBaskiDetayAskiBaskiYigmMahalleMahalleTipiId { get; set; }

        [JsonProperty("ad")]
        public string Ad { get; set; }

        [JsonProperty("askiBaskiDetay_koordinat")]
        public string AskiBaskiDetayKoordinat { get; set; }

        [JsonProperty("haneIds")]
        public string HaneIds { get; set; }

        [JsonProperty("askiBaskiDetay_hasarSonucStr")]
        public string AskiBaskiDetayHasarSonucStr { get; set; }

        [JsonProperty("askiBaskiDetay_askiBaski_il_ilAdi")]
        public string AskiBaskiDetayAskiBaskiIlIlAdi { get; set; }

        [JsonProperty("askiBaskiDetay_askiBaski_il_id")]
        public int AskiBaskiDetayAskiBaskiIlId { get; set; }

        [JsonProperty("askiBaskiDetay_askiBaski_yigmMahalle_mahalleTipi_ek")]
        public string AskiBaskiDetayAskiBaskiYigmMahalleMahalleTipiEk { get; set; }

        [JsonProperty("askiBaskiDetay_haneKodlari")]
        public string AskiBaskiDetayHaneKodlari { get; set; }

        [JsonProperty("askiBaskiDetay_binaNo")]
        public string AskiBaskiDetayBinaNo { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("askiBaskiDetay_askiBaski_deprem_siddet")]
        public double AskiBaskiDetayAskiBaskiDepremSiddet { get; set; }

        [JsonProperty("askiBaskiDetay_askiBaski_deprem_tarih")]
        public DateTime AskiBaskiDetayAskiBaskiDepremTarih { get; set; }

        [JsonProperty("askiBaskiDetay_askiBaski_deprem_tanim")]
        public string AskiBaskiDetayAskiBaskiDepremTanim { get; set; }

        [JsonProperty("tc")]
        public string Tc { get; set; }

        [JsonProperty("kullanimAmaci")]
        public string KullanimAmaci { get; set; }

        [JsonProperty("askiBaskiDetay_askiBaski_deprem_id")]
        public int AskiBaskiDetayAskiBaskiDepremId { get; set; }

        [JsonProperty("askiBaskiDetay_kesinHasar_tanim")]
        public string AskiBaskiDetayKesinHasarTanim { get; set; }

        [JsonProperty("kullanimAmaciHasar")]
        public string KullanimAmaciHasar { get; set; }

        [JsonProperty("askiBaskiDetay_itirazHasar_id")]
        public int AskiBaskiDetayItirazHasarId { get; set; }

        [JsonProperty("huid")]
        public string Huid { get; set; }

        [JsonProperty("hane_aciklama")]
        public string HaneAciklama { get; set; }

        [JsonProperty("bina_aciklama")]
        public string BinaAciklama { get; set; }

        [JsonProperty("versiyon")]
        public int Versiyon { get; set; }
    }

}