using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Csb.YerindeDonusum.Application.Models.Mail
{
    public class MailResultModel
    {
        [JsonPropertyName("isError")]
        [JsonProperty("isError")]
        public bool IsError { get; set; } = false;

        [JsonPropertyName("message")]
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        private void ExceptionBase(Exception? exception, string message = "")
        {
            if (exception == null)
                return;

            if (!string.IsNullOrWhiteSpace(message))
                ErrorMessage(message);
            else
                ErrorMessage(exception.Message);

            IsError = true;
        }

        public void Exception(Exception exception) =>
            ExceptionBase(exception, string.Empty);

        public void Exception(Exception exception, string errorMessage) =>
            ExceptionBase(exception, errorMessage);

        public void ErrorMessage(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                return;

            Message = errorMessage;
            IsError = true;
        }
    }
}
