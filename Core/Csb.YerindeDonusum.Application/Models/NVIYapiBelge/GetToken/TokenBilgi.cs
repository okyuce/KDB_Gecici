using Newtonsoft.Json; 
using System.Collections.Generic; 
namespace Csb.YerindeDonusum.Application.Models.NVIYapiBelge.GetToken; 

    public class TokenBilgi
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("first_name")]
        public object FirstName { get; set; }

        [JsonProperty("last_name")]
        public object LastName { get; set; }

        [JsonProperty("user_name")]
        public object UserName { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("mainMenu")]
        public List<MainMenu> MainMenu { get; set; }
    }
