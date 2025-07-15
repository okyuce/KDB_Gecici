using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models.Mail;
using FluentEmail.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net;

namespace Csb.YerindeDonusum.Application.CustomAddons;
public class MailService : IMailService
{
    private readonly IFluentEmailFactory _emailFactory;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public MailService(IFluentEmailFactory emailFactory, IWebHostEnvironment hostingEnvironment)
    {
        _emailFactory = emailFactory;
        _hostingEnvironment = hostingEnvironment;
    }

    public MailResultModel SendMail(string to, string subject, string body, string? cc = null, int? currentAttemps = 0)
    {
        var result = new MailResultModel();

        currentAttemps = (currentAttemps ?? 0) + 1;

        try
        {
            var toList = to
                        .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(s => new FluentEmail.Core.Models.Address
                        {
                            EmailAddress = s.Trim()
                        })
                        .ToList();

            var ccList = (cc ?? "")
                                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                .Where(x => !string.IsNullOrWhiteSpace(x))
                                .Select(s => new FluentEmail.Core.Models.Address
                                {
                                    EmailAddress = s.Trim()
                                })
                                .ToList();

            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            var resMail = _emailFactory.Create()
                            .To(toList)
                            .CC(ccList)
                            .Subject(_hostingEnvironment.IsProduction() ? subject : ($"{subject} #{_hostingEnvironment.EnvironmentName}"))
                            .Body(body, isHtml: true)
                            .Send();

            if (resMail.Successful)
                result.Message = $"{currentAttemps} mail gönderim denemesi yapıldı";
            else
                result.ErrorMessage($"Mail gönderilemedi, {currentAttemps} mail gönderim denemesi yapıldı. ErrorMessages: {string.Join(";", resMail.ErrorMessages)}");
        }
        catch (Exception ex)
        {
            result.Exception(ex);
        }

        //bazen smtp hatası veya timeout hatası geliyor, sistem tarafında çözüm bulamadık.
        //5 kez mail gönderimi denensin diye düzenlendi
        if (result.IsError && currentAttemps <= 5)
        {
            //sırayla 10sn, 20sn, 30sn, 40sn, 50sn beklemeler yapılıp mail tekrar gönderilmeye çalışılacak
            Thread.Sleep(currentAttemps.Value * 10000);
            return SendMail(to: to, subject: subject, body: body, cc: cc, currentAttemps: currentAttemps);
        }

        Console.WriteLine($"SendMail(to: {to}, subject: {subject}, currentAttemps: {currentAttemps}) -> environment: {_hostingEnvironment.EnvironmentName} -> {DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")} -> {JsonConvert.SerializeObject(result)}");

        return result;
    }
}
