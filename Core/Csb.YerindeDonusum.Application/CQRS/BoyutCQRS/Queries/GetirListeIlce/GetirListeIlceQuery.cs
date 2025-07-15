using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.BoyutCQRS.Queries.GetirListeIl;

public class GetirListeIlceQuery : IRequest<ResultModel<List<BoyutKonumDto>>>
{
    public int? IlKod { get; set; }
    public class GetirListeIlceQueryHandler : IRequestHandler<GetirListeIlceQuery, ResultModel<List<BoyutKonumDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IKdsBoyutTblIlceRepository _boyutIlceRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;
        public GetirListeIlceQueryHandler(IMapper mapper, IKdsBoyutTblIlceRepository boyutIlceRepository, ICacheService cacheService, IWebHostEnvironment webHostEnvironment)
        {
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _boyutIlceRepository = boyutIlceRepository;
        }

        public async Task<ResultModel<List<BoyutKonumDto>>> Handle(GetirListeIlceQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirListeIlceQuery)}_{request.IlKod}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
                return new ResultModel<List<BoyutKonumDto>>(JsonConvert.DeserializeObject<List<BoyutKonumDto>>(redisCache));

            var liste = _mapper.Map<List<BoyutKonumDto>>(_boyutIlceRepository.GetAllQueryable(x => x.Aktif == true && x.IlKod == request.IlKod).OrderBy(x => x.Ad).ToList());

            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(liste), TimeSpan.FromMinutes(300));

            return await Task.FromResult(new ResultModel<List<BoyutKonumDto>>(liste));
        }
    }
}