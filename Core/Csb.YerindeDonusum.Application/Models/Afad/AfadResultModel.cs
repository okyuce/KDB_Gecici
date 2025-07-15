using System.Text.Json.Serialization;

namespace Csb.YerindeDonusum.Application.Models.Afad;

public class AfadResultModel<T> where T : class
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("messageCode")]
    public string MessageCode { get; set; }

    [JsonPropertyName("messageList")]
    public List<string> MessageList { get; set; }

    [JsonPropertyName("data")]
    public T Data { get; set; }

    public AfadResultModel()
    {
    }

    public AfadResultModel(T data)
    {
        Data = data;
    }
}