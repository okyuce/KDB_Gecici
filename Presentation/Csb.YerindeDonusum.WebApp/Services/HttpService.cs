using Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciTokenYenile;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.WebApp.Models;
using CSB.Core.Extensions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;

namespace Csb.YerindeDonusum.WebApp.Services;

public class HttpService : IHttpService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthService _authService;

    public HttpService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor, IAuthService authService)
    {
        _clientFactory = clientFactory;
        _httpContextAccessor = httpContextAccessor;
        _authService = authService;
    }

    public async Task<AppResultModel<T>> PostAsync<T, K>(string endpoint, K request) where T : class
    {
        AppResultModel<T>? response = new AppResultModel<T>();

        try
        {
            if (request == null)
            {
                response.ResultModel.ErrorMessage("İstek alınamadı. Lütfen yüklediğiniz dosyaların boyutunu kontrol ediniz.");
                return response;
            }

            using (HttpClient client = _clientFactory.CreateClient("YerindeDonusumApi"))
            {
                var accessToken = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "AccessToken")?.Value;
                if (!string.IsNullOrWhiteSpace(accessToken))
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                client.DefaultRequestHeaders.Add("X-Forwarded-For", _httpContextAccessor.HttpContext?.GetIpAddress());

                var json = JsonConvert.SerializeObject(request ?? new object());
                var finalRequest = new StringContent(json ?? "", Encoding.UTF8, "application/json");

                bool tokenYenilemeIstegiYapildiMi = false;
                bool tokenYenilensinMi = false;
                do
                {
                    using (HttpResponseMessage clientResponse = await client.PostAsync(client.BaseAddress + endpoint, finalRequest))
                    {
                        tokenYenilensinMi = !string.IsNullOrWhiteSpace(accessToken) && clientResponse.StatusCode == HttpStatusCode.Unauthorized && !tokenYenilemeIstegiYapildiMi;
                        #region ...: Token Süresi Dolmuşsa Refresh Token İle Token Yenileme İşlemi Yapılıyor :...
                        if (tokenYenilensinMi)
                        {
                            var responseRefreshToken = await GetRefreshTokenAsync();
                            if (responseRefreshToken != null)
                            {
                                //yeni token başarıyla alındı
                                await _authService.SignInAsync(responseRefreshToken);

                                if (client.DefaultRequestHeaders.Any(x => x.Key == "Authorization"))
                                    client.DefaultRequestHeaders.Remove("Authorization");

                                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {responseRefreshToken.AccessToken}");
                            }
                            tokenYenilemeIstegiYapildiMi = true;
                            continue;
                        }
                        #endregion

                        _httpContextAccessor.HttpContext.Response.StatusCode = (int)clientResponse.StatusCode;
                        response.StatusCode = (int)clientResponse.StatusCode;

                        using (HttpContent content = clientResponse.Content)
                        {
                            string result = await content.ReadAsStringAsync();

                            response.ResultModel = JsonConvert.DeserializeObject<ResultModel<T>>(result);

                            if (response.ResultModel == null)
                            {
                                response.ResultModel = new ResultModel<T>();
                                response.ResultModel.ErrorMessage((int)clientResponse.StatusCode + " " + (clientResponse?.ReasonPhrase ?? "Servisten yanıt alınırken bir hata oluştu."));
                            }
                            else if (response.ResultModel.IsError)
                            {
                                if (response.StatusCode == HttpStatusCode.OK.GetHashCode())
                                    response.StatusCode = HttpStatusCode.BadRequest.GetHashCode();

                                if (response.ResultModel.ValidationErrors?.Any() == true)
                                    response.ResultModel.ErrorMessage(string.Join("\n", response.ResultModel.ValidationErrors.Select(s => s.Mesaj.Replace("Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.", ""))));
                            }
                        }
                    }
                } while (tokenYenilensinMi);
            }
        }
        catch (Exception ex)
        {
            response.ResultModel.Exception(ex, "Bir hata oluştu.");
        }

        return response;
    }

    public async Task<AppResultModel<T>> GetAsync<T>(string endpoint, object? request = null) where T : class
    {
        AppResultModel<T>? response = new AppResultModel<T>();

        try
        {
            using (HttpClient client = _clientFactory.CreateClient("YerindeDonusumApi"))
            {
                endpoint += await CreateQueryStringAsync(request);

                var accessToken = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "AccessToken")?.Value;
                if (!string.IsNullOrWhiteSpace(accessToken))
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                client.DefaultRequestHeaders.Add("X-Forwarded-For", _httpContextAccessor.HttpContext?.GetIpAddress());

                bool tokenYenilemeIstegiYapildiMi = false;
                bool tokenYenilensinMi = false;
                do
                {
                    //malikleri getiren metot için işlem uzun sürebileceği için timeout süresi artırılıyor
                    if (endpoint.Contains("BasvuruWeb/GetirListeMalikler"))
                        client.Timeout = new TimeSpan(0, 10, 0); //10 dakika

                    using (HttpResponseMessage clientResponse = await client.GetAsync(client.BaseAddress + endpoint))
                    {
                        tokenYenilensinMi = !string.IsNullOrWhiteSpace(accessToken) && clientResponse.StatusCode == HttpStatusCode.Unauthorized && !tokenYenilemeIstegiYapildiMi;

                        #region ...: Token Süresi Dolmuşsa Refresh Token İle Token Yenileme İşlemi Yapılıyor :...
                        if (tokenYenilensinMi)
                        {
                            var responseRefreshToken = await GetRefreshTokenAsync();
                            if (responseRefreshToken != null)
                            {
                                // yeni token başarıyla alındı
                                await _authService.SignInAsync(responseRefreshToken);

                                if (client.DefaultRequestHeaders.Any(x => x.Key == "Authorization"))
                                    client.DefaultRequestHeaders.Remove("Authorization");

                                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {responseRefreshToken.AccessToken}");
                            }
                            tokenYenilemeIstegiYapildiMi = true;
                            continue;
                        }
                        #endregion

                        _httpContextAccessor.HttpContext.Response.StatusCode = (int)clientResponse.StatusCode;
                        response.StatusCode = (int)clientResponse.StatusCode;

                        using (HttpContent content = clientResponse.Content)
                        {
                            string result = await content.ReadAsStringAsync();

                            response.ResultModel = JsonConvert.DeserializeObject<ResultModel<T>>(result);

                            if (response.ResultModel == null)
                            {
                                response.ResultModel = new ResultModel<T>();
                                response.ResultModel.ErrorMessage((int)clientResponse.StatusCode + " " + (clientResponse?.ReasonPhrase ?? "Bir hata oluştu."));
                            }
                            else if (response.ResultModel.IsError && response.StatusCode == (int)HttpStatusCode.OK)
                            {
                                response.StatusCode = (int)HttpStatusCode.BadRequest;
                            }
                        }
                    }
                } while (tokenYenilensinMi);
            }
        }
        catch (Exception ex)
        {
            response.ResultModel.Exception(ex, "Bir hata oluştu.");
        }

        return response;
    }

    protected T Initialize<T>(params object[] args)
    {
        return (T)Activator.CreateInstance(typeof(T), args);
    }

    private async Task<string> CreateQueryStringAsync(object? model)
    {
        if (model == null) return "";

        List<PropertyInfo> props = new List<PropertyInfo>(model.GetType().GetProperties());
        StringBuilder sb = new StringBuilder();
        foreach (PropertyInfo prop in props)
        {
            if (string.IsNullOrEmpty(prop?.GetValue(model)?.ToString()))
            {
                //sb.Append(HttpUtility.UrlEncode(prop?.Name)).Append("=''").Append("&");
                continue;
            }

            if (typeof(IEnumerable).IsAssignableFrom(prop?.PropertyType) && prop?.PropertyType != typeof(string))
            {
                var value = prop?.GetValue(model, null);
                if (value != null)
                {
                    var iterableInt = value as List<int>;
                    var iterableLong = value as List<long>;
                    var iterableStr = value as List<string>;
                    if (iterableInt != null)
                    {
                        foreach (var item in iterableInt)
                        {
                            sb.Append(HttpUtility.UrlEncode(prop?.Name)).Append("=").Append(HttpUtility.UrlEncode(item.ToString().Trim())).Append("&");
                        }
                    }
                    else if (iterableLong != null)
                    {
                        foreach (var item in iterableLong)
                        {
                            sb.Append(HttpUtility.UrlEncode(prop?.Name)).Append("=").Append(HttpUtility.UrlEncode(item.ToString().Trim())).Append("&");
                        }
                    }
                    else if (iterableStr != null)
                    {
                        foreach (var item in iterableStr)
                        {
                            sb.Append(HttpUtility.UrlEncode(prop?.Name)).Append("=").Append(HttpUtility.UrlEncode(item.Trim())).Append("&");
                        }
                    }
                }
                continue;
            }

            sb.Append(HttpUtility.UrlEncode(prop?.Name)).Append("=").Append(HttpUtility.UrlEncode(prop?.GetValue(model, null)?.ToString()?.Trim())).Append("&");

        }
        string query = sb.ToString();

        if (query.IsNullOrEmpty()) return await Task.FromResult("");

        return await Task.FromResult("?" + query.Substring(0, (query.Length - 1)));
    }

    private async Task<TokenDto>? GetRefreshTokenAsync()
    {
        try
        {
            var response = new AppResultModel<TokenDto>();

            using (HttpClient client = _clientFactory.CreateClient("YerindeDonusumApi"))
            {
                var accessToken = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "AccessToken")?.Value;
                var refreshToken = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "RefreshToken")?.Value;

                if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
                    return null;

                client.DefaultRequestHeaders.Add("X-Forwarded-For", _httpContextAccessor.HttpContext.GetIpAddress());

                var json = JsonConvert.SerializeObject(new KullaniciTokenYenileCommand
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });

                var finalRequest = new StringContent(json ?? "", Encoding.UTF8, "application/json");
                using (HttpResponseMessage clientResponse = await client.PostAsync(client.BaseAddress + "Hesap/Yenile", finalRequest))
                {
                    _httpContextAccessor.HttpContext.Response.StatusCode = (int)clientResponse.StatusCode;
                    response.StatusCode = (int)clientResponse.StatusCode;

                    using (HttpContent content = clientResponse.Content)
                    {
                        if (clientResponse.StatusCode == HttpStatusCode.OK)
                        {
                            string result = await content.ReadAsStringAsync();

                            response.ResultModel = JsonConvert.DeserializeObject<ResultModel<TokenDto>>(result);

                            if (!string.IsNullOrWhiteSpace(response.ResultModel?.Result?.AccessToken))
                                return response.ResultModel.Result;
                        }
                    }
                }
            }
        }
        catch { }

        return null;
    }
}