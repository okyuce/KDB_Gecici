using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.YapiBelgeCQRS.Queries.GetirYapiBelgeByYapiKimlikNo;

public class GetirYapiBelgeRuhsatByBultenNoQuery : IRequest<ResultModel<GetirYapiBelgeRuhsatByBultenNoQueryResponseModel>>
{
    public long? BultenNo { get; set; }

    public class GetirYapiBelgeByYapiKimlikNoQueryHandler : IRequestHandler<GetirYapiBelgeRuhsatByBultenNoQuery, ResultModel<GetirYapiBelgeRuhsatByBultenNoQueryResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly INVIYapiBelgeSorguService _nviYapiBelgeServis;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;

        public GetirYapiBelgeByYapiKimlikNoQueryHandler(IMapper mapper, INVIYapiBelgeSorguService nviYapiBelgeServis, IKullaniciBilgi kullaniciBilgi, ICacheService cacheService, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _nviYapiBelgeServis = nviYapiBelgeServis;
            _kullaniciBilgi = kullaniciBilgi;
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ResultModel<GetirYapiBelgeRuhsatByBultenNoQueryResponseModel>> Handle(GetirYapiBelgeRuhsatByBultenNoQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirYapiBelgeRuhsatByBultenNoQueryResponseModel>();

            if (request?.BultenNo == null)
            {
                result.ErrorMessage("Bülten no girmeniz gerekmektedir!");
                return result;
            }

            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirYapiBelgeRuhsatByBultenNoQuery)}_{request.BultenNo}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
            {
                result.Result = JsonConvert.DeserializeObject<GetirYapiBelgeRuhsatByBultenNoQueryResponseModel>(redisCache);
                return result;
            }

            long.TryParse(_kullaniciBilgi.GetUserInfo().TcKimlikNo, out long tcKimlikNo);

            if (tcKimlikNo <= 0)
            {
                result.ErrorMessage("Bülten no sorgulaması için giriş yapan kişinin T.C. kimlik numarası dolu olması gerekmektedir!");
                return result;
            }

            var yapiBelgeSonuc = await _nviYapiBelgeServis.YapiRuhsatOku(request.BultenNo.Value);
           
            if (!yapiBelgeSonuc.IsSuccessful)
            {
                result.Exception(new ArgumentNullException($"Ruhsat bilgisi bulunamadı, Servis Mesajı: ${yapiBelgeSonuc.Message}"), "Ruhsat bilgisi bulunamadı!");
                return result;
            }

            result.Result = _mapper.Map<GetirYapiBelgeRuhsatByBultenNoQueryResponseModel>(yapiBelgeSonuc.Data);
            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result.Result), TimeSpan.FromMinutes(60));

            return result;
        }
    }
}