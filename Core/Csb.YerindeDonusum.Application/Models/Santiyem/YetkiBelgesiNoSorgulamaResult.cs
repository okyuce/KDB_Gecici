using Newtonsoft.Json; 
namespace  Csb.YerindeDonusum.Application.Models.Santiyem{ 

    public class YetkiBelgesiNoSorgulamaResult
    {
        [JsonProperty("Data")]
        public YetkiBelgesiBilgi Data { get; set; }

        [JsonProperty("IsSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }
    }

}