using Newtonsoft.Json; 
using System.Collections.Generic; 
namespace Csb.YerindeDonusum.Application.Models.NVIYapiBelge.GetToken; 

    public class MainMenu
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("menuItems")]
        public List<MenuItem> MenuItems { get; set; }
    }

