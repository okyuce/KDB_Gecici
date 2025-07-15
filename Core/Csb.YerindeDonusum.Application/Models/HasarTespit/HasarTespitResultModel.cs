using Newtonsoft.Json; 
namespace Csb.YerindeDonusum.Application.Models.HasarTespit{ 

    public class HasarTespitResultModel
    {
        [JsonProperty("className")]
        public string ClassName { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("msgType")]
        public string MsgType { get; set; }

        [JsonProperty("rejectedFields")]
        public string RejectedFields { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("timeout")]
        public int Timeout { get; set; }

        [JsonProperty("tokenUpdated")]
        public string TokenUpdated { get; set; }

        [JsonProperty("object")]
        public List<HasarTespitListeItem> Object { get; set; }
    }

}