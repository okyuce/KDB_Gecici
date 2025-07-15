using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.BoyutCQRS.Queries.GetirListeIl;

public class GetirListeIlQuery : IRequest<ResultModel<List<BoyutKonumDto>>>
{
    public class GetirListeIlQueryHandler : IRequestHandler<GetirListeIlQuery, ResultModel<List<BoyutKonumDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IKdsBoyutTblIlRepository _boyutIlRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;
        public GetirListeIlQueryHandler(IMapper mapper, IKdsBoyutTblIlRepository boyutIlRepository, ICacheService cacheService, IWebHostEnvironment webHostEnvironment)
        {
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _boyutIlRepository = boyutIlRepository;
        }

        public async Task<ResultModel<List<BoyutKonumDto>>> Handle(GetirListeIlQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirListeIlQuery)}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
                return new ResultModel<List<BoyutKonumDto>>(JsonConvert.DeserializeObject<List<BoyutKonumDto>>(redisCache));

            var liste = _mapper.Map<List<BoyutKonumDto>>(_boyutIlRepository.GetAllQueryable(x => x.Aktif == true).OrderBy(x => x.Ad).ToList());

            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(liste), TimeSpan.FromMinutes(300));

            return await Task.FromResult(new ResultModel<List<BoyutKonumDto>>(liste));
        }
    }
}