using Newtonsoft.Json; 
namespace  Csb.YerindeDonusum.Application.Models.Santiyem{ 

    public class YetkiBelgesiBilgi
    {
        [JsonProperty("AdSoyadUnvan")]
        public string AdSoyadUnvan { get; set; }

        [JsonProperty("VergiKimlikNo")]
        public string VergiKimlikNo { get; set; }

        [JsonProperty("Adres")]
        public string Adres { get; set; }

        [JsonProperty("Eposta")]
        public string Eposta { get; set; }

        [JsonProperty("Telefon")]
        public string Telefon { get; set; }

        [JsonProperty("CepTelefon")]
        public string CepTelefon { get; set; }
    }

}