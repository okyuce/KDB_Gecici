using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Commands.TebligatGonder;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.EDevlet;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.EDevlet.Models;
using CSB.Core.LogHandler.Abstraction;
using EDevletBildirimService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.ServiceModel;

namespace Csb.YerindeDonusum.EDevlet;
public class EDevletTebligatService : IEDevletTebligatService
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ICacheService _cacheService;
    private readonly IIntegrationLogService _logService;
    private readonly EDevletBildirimAuthenticationOptionModel? _authDto;
    public EDevletTebligatService(IConfiguration configuration, IMapper mapper, IWebHostEnvironment webHostEnvironment, ICacheService cacheService, IIntegrationLogService logService)
    {
        _configuration = configuration;
        _mapper = mapper;
        _authDto = _configuration.GetSection("EDevletBildirimAuth").Get<EDevletBildirimAuthenticationOptionModel>();
        _webHostEnvironment = webHostEnvironment;
        _cacheService = cacheService;
        _logService = logService;
    }
    private NotificationClient Initialize()
    {
        NotificationClient serviceSoapClient = new NotificationClient(NotificationClient.EndpointConfiguration.NotificationPort);

        if (_authDto != null)
        {
            serviceSoapClient.ClientCredentials.UserName.UserName = _authDto.UserName;
            serviceSoapClient.ClientCredentials.UserName.Password = _authDto.Password;
            serviceSoapClient.Endpoint.Address = new EndpointAddress(new Uri(_authDto.Url));
        }

        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
        binding.MaxBufferSize = 100000000;
        binding.MaxReceivedMessageSize = 100000000;
        binding.SendTimeout = TimeSpan.FromMinutes(5);
        binding.ReceiveTimeout = TimeSpan.FromMinutes(5);
        serviceSoapClient.Endpoint.Binding = binding;
        if (serviceSoapClient.InnerChannel.State == System.ServiceModel.CommunicationState.Faulted)
        {
            serviceSoapClient = new NotificationClient(NotificationClient.EndpointConfiguration.NotificationPort);

            if (_authDto != null)
            {
                serviceSoapClient.ClientCredentials.UserName.UserName = _authDto.UserName;
                serviceSoapClient.ClientCredentials.UserName.Password = _authDto.Password;
                serviceSoapClient.Endpoint.Address = new EndpointAddress(new Uri(_authDto.Url));
            }

            binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            binding.MaxBufferSize = 100000000;
            binding.MaxReceivedMessageSize = 100000000;
            serviceSoapClient.Endpoint.Binding = binding;
        }

        return serviceSoapClient;
    }


    public async Task<EDevletTabligatResult> TebligatGonder(TebligatGonderCommand model)
    {
        try
        {
            var tebligat = new campaignMessage();
            tebligat.username = _authDto?.UserName;
            tebligat.password = _authDto?.Password;
            tebligat.corporationCode = _authDto!.KurumKodu;

            if (model.TebligatTipi == Application.Enums.EDevletTebligatTipiEnum.AnahtarTeslimKampanyaId)
                tebligat.campaignId = _authDto.AnahtarTeslimKampanyaId;
            else if (model.TebligatTipi == Application.Enums.EDevletTebligatTipiEnum.IstirakKampanyaId)
                tebligat.campaignId = _authDto.IstirakKampanyaId;

            var mesajListesi = new List<msgParameterItemType>();
            foreach (var tebligatMesaj in model.TebligatYapilacaklar)
            {
                var mesaj = new msgParameterItemType();
                mesaj.trIdentityNo = tebligatMesaj.TcKimlikNoRaw.ToString() /*tebligatMesaj.TuzelKisiTipId == 1 ? tebligatMesaj.TuzelKisiVergiNo : tebligatMesaj.TcKimlikNoRaw.ToString()*/;
                mesaj.corporationExternalId = tebligatMesaj.GonderimId;
                var paramList = new List<parameterType>();

                var param = new parameterType();
                param.key = "ilAdi";
                param.value = tebligatMesaj.TapuIlAdi;
                paramList.Add(param);

                param = new parameterType();
                param.key = "ilceAdi";
                param.value = tebligatMesaj.TapuIlceAdi;
                paramList.Add(param);

                param = new parameterType();
                param.key = "mahalleAdi";
                param.value = tebligatMesaj.TapuMahalleAdi;
                paramList.Add(param);

                param = new parameterType();
                param.key = "ada";
                param.value = tebligatMesaj.TapuAda == null ? "-" : tebligatMesaj.TapuAda;
                paramList.Add(param);

                param = new parameterType();
                param.key = "parsel";
                param.value = tebligatMesaj.TapuParsel;
                paramList.Add(param);

                param = new parameterType();
                param.key = "tasinmazId";
                param.value = tebligatMesaj.TapuTasinmazId == null ? "-" : tebligatMesaj.TapuTasinmazId;
                paramList.Add(param);

                param = new parameterType();
                param.key = "hasarTespitAskiKodu";
                param.value = tebligatMesaj.AskiKodu;
                paramList.Add(param);
                param = new parameterType();

                param.key = "hasarTespitHasarDurumu";
                param.value = tebligatMesaj.HasarTespitHasarDurumu;
                paramList.Add(param);

                param = new parameterType();
                param.key = "tebligTarihi";
                param.value = tebligatMesaj.TebligTarihi.ToString("dd.MM.yyyy HH:mm:ss");
                paramList.Add(param);

                mesaj.campaignParameters = paramList.ToArray();

                mesajListesi.Add(mesaj);
            }
            tebligat.messageList = mesajListesi.ToArray();
            using var service = Initialize();
            var result = await service.sendMessageAsync(tebligat);
            return _mapper.Map<EDevletTabligatResult>(result.sendMessageC);
        }
        catch (Exception ex)
        {
            return new EDevletTabligatResult { SonucAciklamasi = ex.Message, SonucKodu = "-1" };
        }
    }
}
