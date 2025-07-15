using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.BoyutCQRS.Queries.GetirListeIl;

public class GetirListeCsbmQuery : IRequest<ResultModel<List<BoyutKonumDto>>>
{
    public int? MahalleKod { get; set; }
    public class GetirListeCsbmQueryHandler : IRequestHandler<GetirListeCsbmQuery, ResultModel<List<BoyutKonumDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IKdsBoyutTblCsbmRepository _boyutCsbmRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;
        public GetirListeCsbmQueryHandler(IMapper mapper, IKdsBoyutTblCsbmRepository boyutCsbmRepository, ICacheService cacheService, IWebHostEnvironment webHostEnvironment)
        {
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _boyutCsbmRepository = boyutCsbmRepository;
        }

        public async Task<ResultModel<List<BoyutKonumDto>>> Handle(GetirListeCsbmQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirListeCsbmQuery)}_{request.MahalleKod}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
                return new ResultModel<List<BoyutKonumDto>>(JsonConvert.DeserializeObject<List<BoyutKonumDto>>(redisCache));

            var liste = _mapper.Map<List<BoyutKonumDto>>(_boyutCsbmRepository.GetWhere(x => x.Aktif == true && x.MahalleKod == request.MahalleKod, true, x => x.TipKodNavigation).OrderBy(x => x.Ad).ToList());

            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(liste), TimeSpan.FromMinutes(300));

            return await Task.FromResult(new ResultModel<List<BoyutKonumDto>>(liste));
        }
    }
}