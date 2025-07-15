using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirAfadAccessToken;
using Csb.YerindeDonusum.Application.Dtos.Afad;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.Afad;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirAfadBasvuruListeByTcNo;

public class GetirAfadBasvuruListeByTcNoQuery : IRequest<ResultModel<List<AfadBasvuruDto>>>, ICacheMediatrQuery
{
    public string? TcKimlikNo { get; set; }

    #region Cache Ayar
    public bool? CacheCustomUser => null;
    public int? CacheMinute => 60 * 12; //1 saat
    public bool CacheIsActive => true;
    #endregion

    public class GetirAfadBasvuruListeByTcNoQueryHandler : IRequestHandler<GetirAfadBasvuruListeByTcNoQuery, ResultModel<List<AfadBasvuruDto>>>
    {
        private readonly IMediator _mediator;
        private readonly AfadOptionModel _afadOptionModel;

        public GetirAfadBasvuruListeByTcNoQueryHandler(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _afadOptionModel = configuration.GetSection("AFADOptions").Get<AfadOptionModel>();
        }

        public async Task<ResultModel<List<AfadBasvuruDto>>> Handle(GetirAfadBasvuruListeByTcNoQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<List<AfadBasvuruDto>>();

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
                    client.Timeout = new TimeSpan(0, 0, 30);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {afadAccessToken.Result}");

                    using (HttpResponseMessage clientResponse = await client.GetAsync($"{_afadOptionModel.BaseUrl}hibeDestek?tckn={request.TcKimlikNo}"))
                    {
                        using (HttpContent content = clientResponse.Content)
                        {
                            var response = await content.ReadAsStringAsync();
                            if (clientResponse.StatusCode == HttpStatusCode.OK)
                            {
                                result.Result = JsonConvert.DeserializeObject<AfadResultModel<List<AfadBasvuruDto>>>(response).Data;

                                return await Task.FromResult(result);
                            }
                            else
                            {
                                var afadErrorResult = JsonConvert.DeserializeObject<AfadResultModel<object>>(response);

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