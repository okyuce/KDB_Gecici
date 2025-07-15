using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models.Santiyem;
using Csb.YerindeDonusum.Santiyem.Models;
using CSB.Core.LogHandler.Abstraction;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Csb.YerindeDonusum.Santiyem
{
    public class SantiyemService : ISantiyemService
    {
        private readonly IConfiguration _configuration;
        private readonly SantiyemAuthenticationOptionModel? _authDto;
        private readonly IIntegrationLogService _logService;
        public SantiyemService(IConfiguration configuration, IIntegrationLogService logService)
        {
            _configuration = configuration;
            _authDto = _configuration.GetSection("SantiyemAuth").Get<SantiyemAuthenticationOptionModel>();
            _logService = logService;
        }
        public async Task<YetkiBelgesiNoSorgulamaResult> YetkiBelgesiNoSorgula(string yetkiBelgeNo)
        {
            var sonuc = await _logService.WrapServiceAsnyc<string, YetkiBelgesiNoSorgulamaResult>(yetkiBelgeNo,  async () =>
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_authDto.Url}?YetkiBelgeNo={yetkiBelgeNo}");

                var bytes = Encoding.UTF8.GetBytes($"{_authDto.UserName}:{_authDto.Password}");
                var userEncode = Convert.ToBase64String(bytes);

                request.Headers.Add("Authorization", $"Basic {userEncode}");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<YetkiBelgesiNoSorgulamaResult>( await response.Content.ReadAsStringAsync());

            });
            return sonuc;
        }
    }
}
