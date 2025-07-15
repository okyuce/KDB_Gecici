using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Maks;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.MaksCQRS.Queries.GetirNumaratajByCsbmUavtKodu
{
    public class GetirNumaratajByCsbmUavtKoduQuery : IRequest<ResultModel<List<BoyutKonumDto>>>
    {
        public int? CsbmKod { get; set; }

        public class GetirNumaratajByCsbmUavtKoduQueryHandler : IRequestHandler<GetirNumaratajByCsbmUavtKoduQuery, ResultModel<List<BoyutKonumDto>>>
        {
            private readonly IMapper _mapper;
            private readonly IMaksTopluCbsmRepository _maksTopluCbsmRepository;
            private readonly IMaksTopluNumartajRepository _maksTopluNumartajRepository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly ICacheService _cacheService;

            public GetirNumaratajByCsbmUavtKoduQueryHandler(IMapper mapper,
                                                            IMaksTopluCbsmRepository maksTopluCbsmRepository,
                                                            IWebHostEnvironment webHostEnvironment,
                                                            IMaksTopluNumartajRepository maksTopluNumartajRepository,
                                                            ICacheService cacheService)
            {
                _mapper = mapper;
                _maksTopluCbsmRepository = maksTopluCbsmRepository;
                _maksTopluNumartajRepository = maksTopluNumartajRepository;
                _webHostEnvironment = webHostEnvironment;
                _cacheService = cacheService;
            }

            public async Task<ResultModel<List<BoyutKonumDto>>> Handle(GetirNumaratajByCsbmUavtKoduQuery request, CancellationToken cancellationToken)
            {
                var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirNumaratajByCsbmUavtKoduQueryHandler)}_{request.CsbmKod}";
                var redisCache = await _cacheService.GetValueAsync(cacheKey);
                if (redisCache != null)
                    return new ResultModel<List<BoyutKonumDto>>(JsonConvert.DeserializeObject<List<BoyutKonumDto>>(redisCache));

                var liste = _mapper.Map<List<BoyutKonumDto>>(_maksTopluNumartajRepository.GetAllQueryable(x => x.Uavtkodu == request.CsbmKod).OrderBy(x => x.Kapino).ToList());

                await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(liste), TimeSpan.FromMinutes(300));

                return await Task.FromResult(new ResultModel<List<BoyutKonumDto>>(liste));
            }
        }
    }
}