using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Maks;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.MaksCQRS.Queries.GetirBagimsizBolumByNumaratajUavtKodu
{
    public class GetirBagimsizBolumByNumaratajUavtKoduQuery : IRequest<ResultModel<List<BagimsizBolumDto>>>
    {
        public long? BinaNoId { get; set; }

        public class GetirBagimsizBolumByNumaratajUavtKoduQueryHandler : IRequestHandler<GetirBagimsizBolumByNumaratajUavtKoduQuery, ResultModel<List<BagimsizBolumDto>>>
        {
            private readonly IMapper _mapper;
            private readonly IMaksTopluBagimsizbolumRepository _maksTopluBagimsizBolumRepository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly ICacheService _cacheService;

            public GetirBagimsizBolumByNumaratajUavtKoduQueryHandler(IMapper mapper,
                                                                     IWebHostEnvironment webHostEnvironment,
                                                                     IMaksTopluBagimsizbolumRepository maksTopluBagimsizBolumRepository,
                                                                     ICacheService cacheService)
            {
                _mapper = mapper;
                _maksTopluBagimsizBolumRepository = maksTopluBagimsizBolumRepository;
                _webHostEnvironment = webHostEnvironment;
                _cacheService = cacheService;
            }

            public async Task<ResultModel<List<BagimsizBolumDto>>> Handle(GetirBagimsizBolumByNumaratajUavtKoduQuery request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<List<BagimsizBolumDto>>();

                var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirBagimsizBolumByNumaratajUavtKoduQueryHandler)}_{request.BinaNoId}";
                var redisCache = await _cacheService.GetValueAsync(cacheKey);

                if (redisCache != null)
                    return new ResultModel<List<BagimsizBolumDto>>(JsonConvert.DeserializeObject<List<BagimsizBolumDto>>(redisCache));

                result.Result = _mapper.Map<List<BagimsizBolumDto>>(_maksTopluBagimsizBolumRepository.GetAllQueryable(x => x.NumaratajNo == request.BinaNoId).OrderBy(x => x.Bagimsizbolumno).ToList());
             
                if (!result.Result.Any())
                {
                    result.ErrorMessage("Seçilen binaya ait iç kapı bulunamadı!");
                    return await Task.FromResult(result);
                }

                await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result.Result), TimeSpan.FromMinutes(300));

                return await Task.FromResult(new ResultModel<List<BagimsizBolumDto>>(result.Result));
            }
        }
    }
}