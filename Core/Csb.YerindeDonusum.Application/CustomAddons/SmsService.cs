using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using SmsService;
using Csb.YerindeDonusum.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Csb.YerindeDonusum.Application.Models.Sms;
using Csb.YerindeDonusum.Application.Models;

namespace Csb.YerindeDonusum.Application.CustomAddons;

public class SmsService : ISmsService
{
    private readonly IConfiguration _configuration;
    private readonly SmsOptionModel? _authDto;

    public SmsService(IConfiguration configuration)
    {
        _configuration = configuration;
        _authDto = _configuration.GetSection("MessagingOptions").GetSection("SmsOptions").Get<SmsOptionModel>();
    }

    public async Task<bool> SendSms(string phoneNumber, string message, string userName, string ipAddress)
    {
        try
        {
            if (!StringAddon.ValidatePhone(phoneNumber))
            {
                return false;
            }

            phoneNumber = StringAddon.ToClearPhone(phoneNumber);
            //phoneNumber = phoneNumber.Where(x => StringAddon.ValidatePhone(x)).Select(s => StringAddon.ToClearPhone(s)).FirstOrDefault();

            //if (!phoneList.Any())
            //    return false;

            var client = new SmsClient();

            client.ClientCredentials.UserName.UserName = _authDto.ServiceUserName;
            client.ClientCredentials.UserName.Password = _authDto.ServicePassword;
            client.Endpoint.Address = new EndpointAddress(_authDto.Url);

            var serialize = new DataContractSerializer(typeof(CSBApiHeader));

            var csbheader = new CSBApiHeader
            {
                IslemKodu = "sms",
                KullaniciAdi = userName,
                IslemZamani = DateTime.Now.ToShortDateString(),
                KullaniciIp = ipAddress
            };

            var addressBuilder = new EndpointAddressBuilder(client.Endpoint.Address);

            addressBuilder.Headers.Add(System.ServiceModel.Channels.AddressHeader.CreateAddressHeader("CSBApiHeader", "", csbheader, serialize));

            client.Endpoint.Address = addressBuilder.ToEndpointAddress();

            var httpRequestMessageProperty = new HttpRequestMessageProperty();
            httpRequestMessageProperty.Headers[System.Net.HttpRequestHeader.Authorization] = $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_authDto.ServiceUserName}:{_authDto.ServicePassword}"))}";

            var scope = new OperationContextScope(client.InnerChannel);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestMessageProperty;

            await client.SendAsync("12", phoneNumber, message);

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
