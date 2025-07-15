
using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirAlanByTakbisTasinmazIdQuery;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models.NVIYapiBelge.GetToken;
using Csb.YerindeDonusum.Application.Models.NVIYapiBelge.YapiRuhsatOku;
using Csb.YerindeDonusum.Application.Models.Santiyem;
using Csb.YerindeDonusum.NVIYapiBelge.Models;
using CSB.Core.LogHandler.Abstraction;
using CSB.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RTools_NTS.Util;
using System.Text;

namespace Csb.YerindeDonusum.NVIYapiBelge
{
    public class NVIYapiBelgeSorguService : INVIYapiBelgeSorguService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly NVIYapiBelgeAuthenticationOptionModel? _authDto;
        private readonly ICacheService _cacheService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IIntegrationLogService _logService;
        private IHttpService HttpService { get; }
        private readonly IHttpContextAccessor httpContextAccessor;
        public NVIYapiBelgeSorguService(IConfiguration configuration, IMapper mapper, ICacheService cacheService, IWebHostEnvironment webHostEnvironment, IHttpService httpService, IHttpContextAccessor httpContextAccessor, IIntegrationLogService logService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _authDto = _configuration.GetSection("NVIYapiBelgeAuth").Get<NVIYapiBelgeAuthenticationOptionModel>();
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
            HttpService = httpService;
            this.httpContextAccessor = httpContextAccessor;
            _logService = logService;
        }

        private async Task<GetTokenResult> GetToken()
        {
            var cacheKey = $"YDP_{_webHostEnvironment.EnvironmentName}_{nameof(GetTokenResult)}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
            {
                return JsonConvert.DeserializeObject<GetTokenResult>(redisCache);
            }
            var sonuc = await _logService.WrapServiceAsnyc<object?, GetTokenResult>(null, async () =>
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, _authDto.AuthUrl);
                var content = new StringContent(JsonConvert.SerializeObject(new { user_name = _authDto.UserName, password = _authDto.Password }), Encoding.UTF8, "application/json");
                request.Content = content;

                var response = await _logService.WrapServiceAsnyc<HttpRequestMessage, HttpResponseMessage>(request, async () => { return await client.SendAsync(request); });

                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<GetTokenResult>(await response.Content.ReadAsStringAsync());
            });

            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(sonuc), TimeSpan.FromMinutes(10));
            return sonuc;
        }


        public async Task<YapiRuhsatOkuResult> YapiRuhsatOku(long bultenNo)
        {
            var token = await GetToken();
            var sonuc = await _logService.WrapServiceAsnyc<long, YapiRuhsatOkuResult>(bultenNo, async () =>
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_authDto.Url}?bultenNo={bultenNo}");
                request.Headers.Add("Authorization", $"Bearer {token.Data.Token}");
                var response = await _logService.WrapServiceAsnyc<HttpRequestMessage, HttpResponseMessage>(request, async () => { return await client.SendAsync(request); });

                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<YapiRuhsatOkuResult>(await response.Content.ReadAsStringAsync());
            });
            return sonuc;
            
        }  

    }
}
