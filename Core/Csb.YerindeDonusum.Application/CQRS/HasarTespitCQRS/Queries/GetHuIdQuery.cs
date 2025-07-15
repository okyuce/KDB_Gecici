using AutoMapper;
using Azure;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.HasarTespit;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;

namespace Csb.YerindeDonusum.Application.CQRS.HasarTespitCQRS.Queries;

public class GetHuIdQuery : IRequest<ResultModel<string>>
{
    public string HasarTespitAskiKodu { get; set; }
    public string IcKapiNo { get; set; }
    public class GetHuIdQueryHandler : IRequestHandler<GetHuIdQuery, ResultModel<string>>
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;
        private readonly IHttpClientFactory _clientFactory; 
        private readonly HasarTespitOptionModel _hasarTespitOptionModel;
        public GetHuIdQueryHandler(IConfiguration configuration, ICacheService cacheService, IWebHostEnvironment webHostEnvironment, IMapper mapper, IHttpClientFactory clientFactory)
        {
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _clientFactory = clientFactory;
            _hasarTespitOptionModel = configuration.GetSection("HasarTespitOptions").Get<HasarTespitOptionModel>();
        }

        public async Task<ResultModel<string>> Handle(GetHuIdQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();
            try
            {
                var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetHuIdQueryHandler)}_{request.HasarTespitAskiKodu}";
                var redisCache = await _cacheService.GetValueAsync(cacheKey);

                if (redisCache != null)
                {
                    var hasarTespitResult = JsonConvert.DeserializeObject<HasarTespitResultModel>(redisCache);
                    var hane = hasarTespitResult.Object.Where(x => x.IcKapiNo == request.IcKapiNo).FirstOrDefault();
                    if (hane == null)
                    {
                        result.ErrorMessage("HasarTespit Hane Listesinde Aranan İç Kapı No Bulunamadı. ERR-GABLTCH-780");
                    }
                    else
                    {
                        result.Result = hane.Huid?.Replace("\"", "");
                    }
                    return await Task.FromResult(result);
                }
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 0, 30);

                    using (HttpResponseMessage clientResponse = await client.GetAsync($"{_hasarTespitOptionModel.BaseUrl}?aski_kodu={request.HasarTespitAskiKodu}&authorizationKey={_hasarTespitOptionModel.AuthorizationKey}"))
                    {
                        using (HttpContent content = clientResponse.Content)
                        {
                            var response = await content.ReadAsStringAsync();
                            if (clientResponse.StatusCode == HttpStatusCode.OK)
                            {
                                var hasarTespitResult = JsonConvert.DeserializeObject<HasarTespitResultModel>(response);
                                await _cacheService.SetValueAsync(cacheKey, response, TimeSpan.FromDays(1));
                                if (hasarTespitResult.Result == "ok")
                                {
                                    var hane = hasarTespitResult.Object.Where(x=>x.IcKapiNo == request.IcKapiNo).FirstOrDefault();
                                    if (hane == null)
                                    {
                                        result.ErrorMessage("HasarTespit Hane Listesinde Aranan İç Kapı No Bulunamadı. ERR-GABLTCH-780");
                                    }
                                    else
                                    {
                                        result.Result = hane.Huid?.Replace("\"","");
                                    }
                                }
                                else
                                    result.ErrorMessage("HasarTespit Hane Listesi Alınırken Hata Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz. ERR-GABLTCH-780" + hasarTespitResult.Msg);

                                return await Task.FromResult(result);
                            }
                            else
                            {
                                var hasarTespitErrorResult = JsonConvert.DeserializeObject<HasarTespitResultModel>(response);

                                if (hasarTespitErrorResult.MsgType=="ERROR" && hasarTespitErrorResult.Msg!=null)
                                    result.Exception(new ArgumentNullException(hasarTespitErrorResult.Msg, " HasarTespit Hane Listesi Alınırken Hata Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz. ERR-GABLTCH-780-2"));
                                else
                                    result.ErrorMessage("HasarTespit Hane Listesi Alınırken Hata Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz. ERR-GABLTCH-780-2");

                                return await Task.FromResult(result);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Exception(ex, "HasarTespit Hane Listesi Alınırken Hata Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz. ERR-GABLTCH-780-3");
                return await Task.FromResult(result);
            }
        }
    }
}
