using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.Afad;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirAfadAccessToken;

public class GetirAfadAccessTokenQuery : IRequest<ResultModel<string>>
{
    public class GetirAfadAccessTokenQueryHandler : IRequestHandler<GetirAfadAccessTokenQuery, ResultModel<string>>
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IAfadGirisBilgiRepository _afadGirisBilgiRepository;
        private readonly AfadAuthOptionModel _afadAuthOptionModel;
        private static Object thisLock = new Object();

        public GetirAfadAccessTokenQueryHandler(IHttpClientFactory clientFactory, IAfadGirisBilgiRepository afadGirisBilgiRepository, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _afadGirisBilgiRepository = afadGirisBilgiRepository;
            _afadAuthOptionModel = configuration.GetSection("AFADOptions:AuthOptions").Get<AfadAuthOptionModel>();
        }

        public async Task<ResultModel<string>> Handle(GetirAfadAccessTokenQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<string>();

            lock (thisLock)
            {
                try
                {
                    var dtNow = DateTime.Now;

                    AfadResultModel<AfadResultTokenModel>? afadTokenResult = null;

                    //afad tarafından alınan token bilgisi db ye kaydedililir. db deki token yoken süresi 5 dakikadan az kalmışsa yeni token alınır değilse db deki token kullanılarak işleme devam edilir
                    var sonGirisBilgisi = _afadGirisBilgiRepository.GetAllQueryable().OrderByDescending(o => o.AfadGirisBilgiId).FirstOrDefault();
                    if (sonGirisBilgisi != null)
                    {
                        if (sonGirisBilgisi.GirisTarihi.AddSeconds(sonGirisBilgisi.GecerlilikSuresi) > dtNow)
                        {
                            if ((sonGirisBilgisi.GirisTarihi.AddSeconds(sonGirisBilgisi.GecerlilikSuresi) - dtNow).TotalSeconds < 30)
                            {
                                #region ...: token bitiş süresine 30 saniyeden az kalmışsa refresh token ile token yenileniyor :...

                                using (HttpClient client = new HttpClient())
                                {
                                    client.Timeout = new TimeSpan(0, 0, 30);
                                    client.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Concat(_afadAuthOptionModel.BasicUserName, ":", _afadAuthOptionModel.BasicPassword)))}");

                                    using (HttpResponseMessage clientResponse = client.PostAsync($"{_afadAuthOptionModel.Url}?grant_type=refresh_token&refresh_token={sonGirisBilgisi.RefreshToken}", null).Result)
                                    {
                                        using (HttpContent content = clientResponse.Content)
                                        {
                                            if (clientResponse.StatusCode == HttpStatusCode.OK)
                                                afadTokenResult = JsonConvert.DeserializeObject<AfadResultModel<AfadResultTokenModel>>(content.ReadAsStringAsync().Result);
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                //dbdeki afad token süresi 30 saniyeden fazla, bu token ile işleme devam edebiliriz
                                result.Result = sonGirisBilgisi.AccessToken;

                                return result;
                            }
                        }
                    }

                    //dbden afad token alınmadığı için afad api ile login olunup token alındıktan sonra db ye bu token bilgisi kaydedilecek
                    if (afadTokenResult == null)
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            client.Timeout = new TimeSpan(0, 0, 30);
                            client.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Concat(_afadAuthOptionModel.BasicUserName, ":", _afadAuthOptionModel.BasicPassword)))}");

                            using (HttpResponseMessage clientResponse = client.PostAsync($"{_afadAuthOptionModel.Url}?grant_type=password&username={_afadAuthOptionModel.UserName}&password={_afadAuthOptionModel.Password}", null).Result)
                            {
                                using (HttpContent content = clientResponse.Content)
                                {
                                    if (clientResponse.StatusCode == HttpStatusCode.OK)
                                    {
                                        afadTokenResult = JsonConvert.DeserializeObject<AfadResultModel<AfadResultTokenModel>>(content.ReadAsStringAsync().Result);
                                    }
                                    else
                                    {
                                        var afadErrorResult = JsonConvert.DeserializeObject<AfadResultModel<object>>(content.ReadAsStringAsync().Result);

                                        if (afadErrorResult.MessageList?.Any() == true)
                                            result.Exception(new ArgumentNullException(string.Join(", ", afadErrorResult.MessageList)), "AFAD Servis Girişi Yapılırken Hata Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz. ERR-GAATCH-100");
                                        else
                                            result.ErrorMessage("AFAD Servis Girişi Yapılırken Hata Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz. ERR-GAATCH-200");

                                        return result;
                                    }
                                }
                            }
                        }
                    }

                    //yeni afad token geldiyse tabloya ekleniyor ve değeri geri dönülüyor
                    if (afadTokenResult != null)
                    {
                        _afadGirisBilgiRepository.AddAsync(new AfadGirisBilgi
                        {
                            AccessToken = afadTokenResult.Data.AccessToken,
                            TokenTuru = afadTokenResult.Data.TokenType,
                            RefreshToken = afadTokenResult.Data.RefreshToken,
                            GecerlilikSuresi = afadTokenResult.Data.ExpiresIn,
                            GirisTarihi = dtNow
                        }).Wait();

                        _afadGirisBilgiRepository.SaveChanges(cancellationToken).Wait();

                        result.Result = afadTokenResult.Data.AccessToken;
                        return result;
                    }

                    result.ErrorMessage("AFAD Servis Girişi Yapılamadı! ERR-GAATCH-300");
                }
                catch (Exception ex)
                {
                    result.Exception(ex, "AFAD Servis Girişi Yapılırken Hata Oluştu. Lütfen Daha Sonra Tekrar Deneyiniz. ERR-GAATCH-400");
                }
            }

            return result;
        }
    }
}