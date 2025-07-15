using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.BoyutCQRS.Queries.GetirListeIl;

public class GetirListeNumaratajQuery : IRequest<ResultModel<List<BoyutKonumDto>>>
{
    public int? CsbmKod { get; set; }
    public class GetirListeNumaratajQueryHandler : IRequestHandler<GetirListeNumaratajQuery, ResultModel<List<BoyutKonumDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IKdsBoyutTblNumaratajRepository _boyutNumaratajRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;
        public GetirListeNumaratajQueryHandler(IMapper mapper, IKdsBoyutTblNumaratajRepository boyutNumaratajRepository, ICacheService cacheService, IWebHostEnvironment webHostEnvironment)
        {
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _boyutNumaratajRepository = boyutNumaratajRepository;
        }

        public async Task<ResultModel<List<BoyutKonumDto>>> Handle(GetirListeNumaratajQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirListeNumaratajQuery)}_{request.CsbmKod}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
                return new ResultModel<List<BoyutKonumDto>>(JsonConvert.DeserializeObject<List<BoyutKonumDto>>(redisCache));

            var liste = _mapper.Map<List<BoyutKonumDto>>(_boyutNumaratajRepository.GetAllQueryable(x => x.Aktif == true && x.CsbmKod == request.CsbmKod).OrderBy(x => x.KapiNo).ToList());

            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(liste), TimeSpan.FromMinutes(300));

            return await Task.FromResult(new ResultModel<List<BoyutKonumDto>>(liste));
        }
    }
}