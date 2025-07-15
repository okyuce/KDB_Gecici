using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.YambisCQRS.Queries.GetirYambisYetkiBelgeNo;

public class GetirYambisYetkiBelgeNoQuery : IRequest<ResultModel<GetirYambisYetkiBelgeNoQueryResponseModel>>
{
    public string? YetkiBelgeNo { get; set; }

    public class GetirYambisYetkiBelgeNoQueryHandler : IRequestHandler<GetirYambisYetkiBelgeNoQuery, ResultModel<GetirYambisYetkiBelgeNoQueryResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly ISantiyemService _santiyemService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;

        public GetirYambisYetkiBelgeNoQueryHandler(IMapper mapper, ISantiyemService santiyemService, ICacheService cacheService, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _santiyemService = santiyemService;
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ResultModel<GetirYambisYetkiBelgeNoQueryResponseModel>> Handle(GetirYambisYetkiBelgeNoQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirYambisYetkiBelgeNoQueryResponseModel>();

            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirYambisYetkiBelgeNoQueryHandler)}_{request.YetkiBelgeNo}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
            {
                result.Result = JsonConvert.DeserializeObject<GetirYambisYetkiBelgeNoQueryResponseModel>(redisCache);
                return result;
            }
            var yambisSonuc = await _santiyemService.YetkiBelgesiNoSorgula(request.YetkiBelgeNo); 
     

            if (!yambisSonuc.IsSuccess)
            {
                result.ErrorMessage(yambisSonuc.Message ?? "Yetki belge no bilgileri sorgulanamadı!");
                return result;
            }
            else
            {
                result.Result = _mapper.Map<GetirYambisYetkiBelgeNoQueryResponseModel>(yambisSonuc.Data);
                result.Result.YBNO = request.YetkiBelgeNo;
                await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result.Result), TimeSpan.FromMinutes(300));
            }

            return result;
        }
    }
}