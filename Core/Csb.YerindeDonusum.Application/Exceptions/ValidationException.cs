namespace Csb.YerindeDonusum.Application.Exceptions;

public class ValidationException : Exception
{
    public ValidationException() : this("Hatalı veya geçersiz parametre değeri!")
    {
        
    }

    public ValidationException(string exceptionMessage) : base(exceptionMessage)
    {
        
    }

    public ValidationException(Exception exception) : this(exception.Message)
    {
        
    }
}