using Newtonsoft.Json;
using Serilog;
using System.Text.Json.Serialization;

namespace Csb.YerindeDonusum.Application.Models;

public class ResultModel<T> where T : class
{
    [JsonPropertyName("result")]
    public T Result { get; set; }

    [JsonPropertyName("isError")]
    public bool IsError { get; set; } = false;

    [JsonPropertyName("errorMessage")]
    [JsonProperty("errorMessage")]
    public string ErrorMessageContent { get; protected set; } = string.Empty;

    private Exception? _exception { get; set; }

	[JsonPropertyName("validationErrors")]
	[JsonProperty("validationErrors")]
	public List<ValidationErrorModel>? ValidationErrors { get; protected set; }

    public ResultModel()
    {
        
    }

    public ResultModel(T result)
    {
        Result = result;
    }

    private void ExceptionBase(Exception? exception, string errorMessage = "", bool isCustomErrorMessageEnabled = false)
    {
        if (exception == null)
            return;

        if (!string.IsNullOrWhiteSpace(errorMessage))
            ErrorMessage(errorMessage);
        else
        {
            if (!isCustomErrorMessageEnabled)
                ErrorMessage(exception.Message);
        }

        IsError = true;
        _exception = exception;

        Log.Error("{Exception}", exception);
    }
    
    public void Exception(Exception exception) =>
        ExceptionBase(exception, string.Empty, false);

    public void Exception(Exception exception, string errorMessage) => 
        ExceptionBase(exception, errorMessage, true);

    public void ErrorMessage(string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
            return;

        ErrorMessageContent = errorMessage;
        IsError = true;

        Log.Error("{ErrorMessage}", errorMessage);
    }

    public void ValidationError(string errorMessage, List<ValidationErrorModel> validationErrors)
    {
        if (validationErrors?.Any() != true)
            return;

        ErrorMessageContent = validationErrors.First().Mesaj!;
        IsError = true;
        ValidationErrors = validationErrors;

        Log.Error("{ErrorMessage}", errorMessage);
    }
}