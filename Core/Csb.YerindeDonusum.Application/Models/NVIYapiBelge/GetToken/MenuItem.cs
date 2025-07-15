using Newtonsoft.Json; 
namespace Csb.YerindeDonusum.Application.Models.NVIYapiBelge.GetToken; 

    public class MenuItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("actionname")]
        public string Actionname { get; set; }
    }
