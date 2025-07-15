using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirAfadAccessToken;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.Afad;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirAfadTopluDegisenBasvuruListe;

public class GetirAfadTopluDegisenBasvuruListeQuery : IRequest<ResultModel<AfadTotalCountResultModel<List<GetirAfadTopluBasvuruDto>>>>, ICacheMediatrQuery
{
    public DateTime? Tarih { get; set; }
    public int? Offset { get; set; }

    #region Cache Ayar
    public bool? CacheCustomUser => null;
    public int? CacheMinute => 60 * 12;
    public bool CacheIsActive => true;
    #endregion

    public class GetirAfadBasvuruListeTopluQueryHandler : IRequestHandler<GetirAfadTopluDegisenBasvuruListeQuery, ResultModel<AfadTotalCountResultModel<List<GetirAfadTopluBasvuruDto>>>>
    {
        private readonly IMediator _mediator;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;
        private readonly AfadOptionModel _afadOptionModel;

        public GetirAfadBasvuruListeTopluQueryHandler(IMediator mediator, IHttpClientFactory clientFactory, IMapper mapper, IConfiguration configuration)
        {
            _mediator = mediator;
            _clientFactory = clientFactory;
            _mapper = mapper;
            _afadOptionModel = configuration.GetSection("AFADOptions").Get<AfadOptionModel>();
        }

        public async Task<ResultModel<AfadTotalCountResultModel<List<GetirAfadTopluBasvuruDto>>>> Handle(GetirAfadTopluDegisenBasvuruListeQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<AfadTotalCountResultModel<List<GetirAfadTopluBasvuruDto>>>();

            if (request.Offset == null || request.Offset < 0)
                request.Offset = 0;

            try
            {
                var afadAccessToken = await _mediator.Send(new GetirAfadAccessTokenQuery());

                //token alınamadı
                if (afadAccessToken.IsError)
                {
                    result.ErrorMessage(afadAccessToken.ErrorMessageContent);
                    return await Task.FromResult(result);
                }

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 0, 120);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {afadAccessToken.Result}");

                    var jsonData = JsonConvert.SerializeObject(new
                    {
                        tarih = int.Parse(request.Tarih.Value.ToString("yyyyMMdd")),
                        offset = request.Offset
                    });
                    var postContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    using (var clientResponse = await client.PostAsync($"{_afadOptionModel.BaseUrl}komisyon/hakSahipleriDegisen", postContent))
                    {
                        using (var content = clientResponse.Content)
                        {
                            var response = await content.ReadAsStringAsync();
                            if (clientResponse.StatusCode == HttpStatusCode.OK)
                            {
                                result.Result = JsonConvert.DeserializeObject<AfadTotalCountResultModel<List<GetirAfadTopluBasvuruDto>>>(response);

                                return await Task.FromResult(result);
                            }
                            else
                            {
                                var afadErrorResult = JsonConvert.DeserializeObject<AfadTotalCountResultModel<object>>(response);

                                if (afadErrorResult.MessageList?.Any() == true)
                                    result.Exception(new ArgumentNullException(string.Join(", ", afadErrorResult.MessageList)), "AFAD Başvuru Listesi Alınırken Hata Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz. ERR-GABLTCH-100");
                                else
                                    result.ErrorMessage("AFAD Başvuru Listesi Alınırken Hata Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz. ERR-GABLTCH-200");

                                return await Task.FromResult(result);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Exception(ex, "AFAD Başvuru Listesi Alınırken Hata Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz. ERR-GABLTCH-300");
                return await Task.FromResult(result);
            }
        }
    }
}