using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.BoyutCQRS.Queries.GetirListeBagimsizBolum;

public class GetirListeBagimsizBolumQuery : IRequest<ResultModel<List<BoyutKonumDto>>>
{
    public int? BinaNoId { get; set; }
    public class GetirListeBagimsizBolumQueryHandler : IRequestHandler<GetirListeBagimsizBolumQuery, ResultModel<List<BoyutKonumDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IKdsBoyutTblBagimsizBolumRepository _boyutBagimsizBolumRepository;
        private readonly IKdsBoyutTblNumaratajRepository _boyutNumaratajRepository;
        private readonly IKdsBoyutTblYapiRepository _boyuYapiRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;
        public GetirListeBagimsizBolumQueryHandler(IMapper mapper, IKdsBoyutTblBagimsizBolumRepository boyutBagimsizBolumRepository, ICacheService cacheService, IWebHostEnvironment webHostEnvironment, IKdsBoyutTblNumaratajRepository boyutNumaratajRepository, IKdsBoyutTblYapiRepository boyuYapiRepository)
        {
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _boyutBagimsizBolumRepository = boyutBagimsizBolumRepository;
            _boyutNumaratajRepository = boyutNumaratajRepository;
            _boyuYapiRepository = boyuYapiRepository;
        }

        public async Task<ResultModel<List<BoyutKonumDto>>> Handle(GetirListeBagimsizBolumQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirListeBagimsizBolumQuery)}_{request.BinaNoId}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
                return new ResultModel<List<BoyutKonumDto>>(JsonConvert.DeserializeObject<List<BoyutKonumDto>>(redisCache));

            var result = new ResultModel<List<BoyutKonumDto>>();

            var numarataj = _boyutNumaratajRepository.GetWhere(x => x.NumaratajKod == request.BinaNoId && x.Aktif == true, true).FirstOrDefault();
            if (numarataj == null) 
            {
                result.ErrorMessage("İç kapı no listesi bulunamadı!");
                return await Task.FromResult(result);
            }

            if (!(numarataj.YapiKod > 0))
            {
                result.ErrorMessage("Seçilen binaya ait iç kapı yok!");
                return await Task.FromResult(result);
            }

            result.Result = _mapper.Map<List<BoyutKonumDto>>(_boyutBagimsizBolumRepository.GetWhere(x => x.Aktif == true && x.YapiKod == numarataj.YapiKod).OrderBy(o => o.IcKapiNo).ToList());

            if (!result.Result.Any())
            {
                result.ErrorMessage("Seçilen binaya ait iç kapı bulunamadı!");
                return await Task.FromResult(result);
            }

            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result.Result), TimeSpan.FromMinutes(300));

            return await Task.FromResult(result);
        }
    }
}