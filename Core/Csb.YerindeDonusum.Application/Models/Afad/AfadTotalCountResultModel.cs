using System.Text.Json.Serialization;

namespace Csb.YerindeDonusum.Application.Models.Afad;

public class AfadTotalCountResultModel<T> where T : class
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("messageCode")]
    public string MessageCode { get; set; }

    [JsonPropertyName("messageList")]
    public List<string> MessageList { get; set; }

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("data")]
    public T Data { get; set; }

    public AfadTotalCountResultModel()
    {
    }

    public AfadTotalCountResultModel(T data)
    {
        Data = data;
    }
}