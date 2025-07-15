namespace Csb.YerindeDonusum.Application.Interfaces;

public interface ISmsService
{
    public Task<bool> SendSms(string phoneNumber, string message, string userName, string ipAddress);
}
