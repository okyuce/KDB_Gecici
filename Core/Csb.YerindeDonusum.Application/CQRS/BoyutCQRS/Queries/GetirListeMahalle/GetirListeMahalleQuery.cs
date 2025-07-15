using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.BoyutCQRS.Queries.GetirListeIl;

public class GetirListeMahalleQuery : IRequest<ResultModel<List<BoyutKonumDto>>>
{
    public int? IlceKod { get; set; }
    public class GetirListeMahalleQueryHandler : IRequestHandler<GetirListeMahalleQuery, ResultModel<List<BoyutKonumDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IKdsBoyutTblMahalleRepository _boyutMahalleRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;
        public GetirListeMahalleQueryHandler(IMapper mapper, IKdsBoyutTblMahalleRepository boyutMahalleRepository, ICacheService cacheService, IWebHostEnvironment webHostEnvironment)
        {
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _boyutMahalleRepository = boyutMahalleRepository;
        }

        public async Task<ResultModel<List<BoyutKonumDto>>> Handle(GetirListeMahalleQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirListeMahalleQuery)}_{request.IlceKod}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
                return new ResultModel<List<BoyutKonumDto>>(JsonConvert.DeserializeObject<List<BoyutKonumDto>>(redisCache));

            var query = _boyutMahalleRepository.GetAllQueryable(x => x.Aktif == true && x.IlceKod == request.IlceKod)
                .AsQueryable()
                .Include(x => x.KoyKodNavigation);

            var liste = _mapper.Map<List<BoyutKonumDto>>(query.ToList()).OrderBy(o => o.Ad).ToList();

            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(liste), TimeSpan.FromMinutes(300));

            return await Task.FromResult(new ResultModel<List<BoyutKonumDto>>(liste));
        }
    }
}