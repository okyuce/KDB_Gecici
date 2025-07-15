using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.BoyutCQRS.Queries.GetirListeIl;

public class GetirListeDepremIlQuery : IRequest<ResultModel<List<BoyutKonumDto>>>
{
    public class GetirListeDepremIlQueryHandler : IRequestHandler<GetirListeDepremIlQuery, ResultModel<List<BoyutKonumDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IKdsBoyutTblIlRepository _boyutIlRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;
        public GetirListeDepremIlQueryHandler(IMapper mapper, IKdsBoyutTblIlRepository boyutIlRepository, ICacheService cacheService, IWebHostEnvironment webHostEnvironment)
        {
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _boyutIlRepository = boyutIlRepository;
        }

        public async Task<ResultModel<List<BoyutKonumDto>>> Handle(GetirListeDepremIlQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirListeDepremIlQuery)}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
                return new ResultModel<List<BoyutKonumDto>>(JsonConvert.DeserializeObject<List<BoyutKonumDto>>(redisCache));

            var depremIlKodListe = new List<int>() {
                BoyutIlIdEnum.Adana.GetHashCode(),
                BoyutIlIdEnum.Adiyaman.GetHashCode(),
                BoyutIlIdEnum.Diyarbakir.GetHashCode(),
                BoyutIlIdEnum.Elazig.GetHashCode(),
                BoyutIlIdEnum.Gaziantep.GetHashCode(),
                BoyutIlIdEnum.Hatay.GetHashCode(),
                BoyutIlIdEnum.Malatya.GetHashCode(),
                BoyutIlIdEnum.Kahramanmaras.GetHashCode(),
                BoyutIlIdEnum.Sanliurfa.GetHashCode(),
                BoyutIlIdEnum.Kilis.GetHashCode(),
                BoyutIlIdEnum.Osmaniye.GetHashCode(),
                BoyutIlIdEnum.Batman.GetHashCode(),
                BoyutIlIdEnum.Kayseri.GetHashCode(),
                BoyutIlIdEnum.Mardin.GetHashCode(),
                BoyutIlIdEnum.Nigde.GetHashCode(),
                BoyutIlIdEnum.Sivas.GetHashCode(),
                BoyutIlIdEnum.Tunceli.GetHashCode(),
                BoyutIlIdEnum.Bingol.GetHashCode()
            };

            var liste = _mapper.Map<List<BoyutKonumDto>>(_boyutIlRepository.GetAllQueryable(x => x.Aktif == true && depremIlKodListe.Contains(x.IlKod)).OrderBy(x => x.Ad).ToList());

            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(liste), TimeSpan.FromMinutes(300));

            return await Task.FromResult(new ResultModel<List<BoyutKonumDto>>(liste));
        }
    }
}