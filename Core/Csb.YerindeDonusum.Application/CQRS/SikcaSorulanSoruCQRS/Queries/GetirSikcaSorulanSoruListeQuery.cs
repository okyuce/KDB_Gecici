using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.StringAddons;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS;

public class GetirSikcaSorulanSoruListeQuery : IRequest<ResultModel<List<SikcaSorulanSoruDto>>>
{
    public string? Arama { get; set; }
    public bool? SilindiMi { get; set; } = false;
    public bool? AktifMi { get; set; } = true;

    public class GetirSikcaSorulanSoruListeQueryHandler : IRequestHandler<GetirSikcaSorulanSoruListeQuery, ResultModel<List<SikcaSorulanSoruDto>>>
    {
        private readonly IMapper _mapper;
        private readonly ISikcaSorulanSoruRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;

        public GetirSikcaSorulanSoruListeQueryHandler(IMapper mapper, ISikcaSorulanSoruRepository repository, IWebHostEnvironment webHostEnvironment, ICacheService cacheService)
        {
            _mapper = mapper;
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
            _cacheService = cacheService;
        }

        public async Task<ResultModel<List<SikcaSorulanSoruDto>>> Handle(GetirSikcaSorulanSoruListeQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirSikcaSorulanSoruListeQuery)}";
            var r = await _cacheService.GetValueAsync(cacheKey);
            if (r != null)
                return new ResultModel<List<SikcaSorulanSoruDto>>(JsonConvert.DeserializeObject<List<SikcaSorulanSoruDto>>(r));

            var result = _mapper.Map<List<SikcaSorulanSoruDto>>(
                                        _repository.GetWhereEnumerable(x => x.SilindiMi == request.SilindiMi && x.AktifMi == request.AktifMi
                                        && (StringAddons.Search(x.Soru, request.Arama) || StringAddons.Search(x.Cevap, request.Arama))
                                ).OrderBy(x=> x.SiraNo).ToList()
            );

            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(60));

            return await Task.FromResult(new ResultModel<List<SikcaSorulanSoruDto>>(result));
        }
    }
}