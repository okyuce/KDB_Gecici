using Newtonsoft.Json; 
using System.Collections.Generic; 
namespace Csb.YerindeDonusum.Application.Models.NVIYapiBelge.GetToken; 

    public class GetTokenResult
    {
        [JsonProperty("data")]
        public TokenBilgi Data { get; set; }

        [JsonProperty("dataList")]
        public List<object> DataList { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("isSuccessful")]
        public bool IsSuccessful { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }
    }
