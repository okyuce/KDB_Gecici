
using Csb.YerindeDonusum.Application.Models.Mail;

namespace Csb.YerindeDonusum.Application.Interfaces;

public interface IMailService
{
    public MailResultModel SendMail(string to, string subject, string body, string? cc = null, int? currentAttemps = 0);
}