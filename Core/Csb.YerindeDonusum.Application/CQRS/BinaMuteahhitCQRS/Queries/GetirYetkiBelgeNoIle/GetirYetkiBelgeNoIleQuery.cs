using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.YambisCQRS.Queries.GetirYambisYetkiBelgeNo;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using static Csb.YerindeDonusum.Application.CQRS.YambisCQRS.Queries.GetirYambisYetkiBelgeNo.GetirYambisYetkiBelgeNoQuery;

namespace Csb.YerindeDonusum.Application.CQRS.MuteahhitCQRS.Queries.GetirYetkiBelgeNoIle;

public class GetirYetkiBelgeNoIleQuery : IRequest<ResultModel<GetirYetkiBelgeNoIleQueryResponseModel>>
{
    public string? YetkiBelgeNo { get; set; }
    public string? AskiKodu { get; set; }
    public int? MahalleKodu { get; set; }

    public class GetirIdIleQueryHandler : IRequestHandler<GetirYetkiBelgeNoIleQuery, ResultModel<GetirYetkiBelgeNoIleQueryResponseModel>>
    {
        private readonly ISantiyemService _santiyemServis;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;


        public GetirIdIleQueryHandler(ISantiyemService santiyemServis, IMapper mapper, IWebHostEnvironment webHostEnvironment, ICacheService cacheService)
        {
            _santiyemServis = santiyemServis;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _cacheService = cacheService;
        }

        public async Task<ResultModel<GetirYetkiBelgeNoIleQueryResponseModel>> Handle(GetirYetkiBelgeNoIleQuery request, CancellationToken cancellationToken)
        {
            ResultModel<GetirYetkiBelgeNoIleQueryResponseModel> result = new ResultModel<GetirYetkiBelgeNoIleQueryResponseModel>();
            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirIdIleQueryHandler)}_{request.YetkiBelgeNo}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
            {
                result.Result = JsonConvert.DeserializeObject<GetirYetkiBelgeNoIleQueryResponseModel>(redisCache);
                return result;
            }
            var yambisSonuc = await _santiyemServis.YetkiBelgesiNoSorgula(request.YetkiBelgeNo);

            if (!yambisSonuc.IsSuccess)
            {
                result.ErrorMessage(yambisSonuc.Message ?? "Yetki belge no bilgileri sorgulanamadı!");
                return result;
            } 
            else
            {
                result.Result = _mapper.Map<GetirYetkiBelgeNoIleQueryResponseModel>(yambisSonuc.Data);
                result.Result.YetkiBelgeNo = request.YetkiBelgeNo;
                result.Result.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.AskiKodu) ?? string.Empty;
                result.Result.UavtMahalleNo = request.MahalleKodu ?? 0;
                await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result.Result), TimeSpan.FromMinutes(300));
            }

            return await Task.FromResult(result);
        }
    }
}
