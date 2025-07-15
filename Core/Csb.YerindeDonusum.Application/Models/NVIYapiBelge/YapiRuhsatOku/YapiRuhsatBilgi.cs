using Newtonsoft.Json; 
namespace Csb.YerindeDonusum.Application.Models.NVIYapiBelge.YapiRuhsatOku; 

    public class YapiRuhsatBilgi
    {
        [JsonProperty("yapiKimlikNo")]
        public int? YapiKimlikNo { get; set; }

        [JsonProperty("toplamYapiInsaatAlan")]
        public decimal? ToplamYapiInsaatAlan { get; set; }

        [JsonProperty("toplamBBSayi")]
        public int? ToplamBBSayi { get; set; }

        [JsonProperty("toplamKatSayi")]
        public int? ToplamKatSayi { get; set; }

        [JsonProperty("yolKotUstKatSayi")]
        public int? YolKotUstKatSayi { get; set; }

        [JsonProperty("yolKotAltKatSayi")]
        public int? YolKotAltKatSayi { get; set; }

        [JsonProperty("acikAdres")]
        public string? AcikAdres { get; set; }

        [JsonProperty("adaNo")]
        public string? AdaNo { get; set; }

        [JsonProperty("parselNo")]
        public string? ParselNo { get; set; }

        [JsonProperty("ruhsatOnayTarihi")]
        public DateTime? RuhsatOnayTarihi { get; set; }

        [JsonProperty("ilAdi")]
        public string? IlAdi { get; set; }

        [JsonProperty("ilKimlikNo")]
        public int? IlKimlikNo { get; set; }

        [JsonProperty("ilceAdi")]
        public string? IlceAdi { get; set; }

        [JsonProperty("ilceKimlikNo")]
        public int? IlceKimlikNo { get; set; }

        [JsonProperty("csbmAdi")]
        public string? CsbmAdi { get; set; }

        [JsonProperty("csbmKimlikNo")]
        public int? CsbmKimlikNo { get; set; }

        [JsonProperty("mahalleAdi")]
        public string? MahalleAdi { get; set; }

        [JsonProperty("mahalleKimlikNo")]
        public int? MahalleKimlikNo { get; set; }

        [JsonProperty("koyAdi")]
        public string? KoyAdi { get; set; }

        [JsonProperty("koyKimlikNo")]
        public int? KoyKimlikNo { get; set; }

        [JsonProperty("numaratajKimlikNo")]
        public int? NumaratajKimlikNo { get; set; }

        [JsonProperty("disKapiNo")]
        public string? DisKapiNo { get; set; }
    }
