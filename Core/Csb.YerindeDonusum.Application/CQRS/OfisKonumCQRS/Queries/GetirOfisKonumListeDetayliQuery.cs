using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Text.Json;

namespace Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Queries;

public class GetirOfisKonumListeDetayliQuery : IRequest<ResultModel<List<GetirOfisKonumDetayResponseModel>>>
{
    public class GetirOfisKonumListeDetayliQueryHandler : IRequestHandler<GetirOfisKonumListeDetayliQuery, ResultModel<List<GetirOfisKonumDetayResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IOfisKonumRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;

        public GetirOfisKonumListeDetayliQueryHandler(IMapper mapper, IOfisKonumRepository repository, IWebHostEnvironment webHostEnvironment, ICacheService cacheService)
        {
            _mapper = mapper;
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
            _cacheService = cacheService;
        }

        public async Task<ResultModel<List<GetirOfisKonumDetayResponseModel>>> Handle(GetirOfisKonumListeDetayliQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirOfisKonumListeDetayliQuery)}";
            var r = await _cacheService.GetValueAsync(cacheKey);
            if (r != null)
                return new ResultModel<List<GetirOfisKonumDetayResponseModel>>(JsonConvert.DeserializeObject<List<GetirOfisKonumDetayResponseModel>>(r));

            var result = _repository
                .GetWhere(x => !x.SilindiMi && x.Konum.GeometryType.Equals("POINT"), true)
                .Select(s => new GetirOfisKonumDetayResponseModel
                {
                    OfisKonumId = s.OfisKonumId,
                    IlAdi = s.IlAdi,
                    IlceAdi = s.IlceAdi,
                    Adres = s.Adres,
                    HaritaUrl = s.HaritaUrl,
                    Enlem = s.Konum.InteriorPoint.X,
                    Boylam = s.Konum.InteriorPoint.Y,
                    AktifMi = s.AktifMi
                })
                .OrderBy(x => x.IlAdi)
                .ThenBy(x => x.IlceAdi)
                .ThenBy(x => x.Adres)
                .ToList();

            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(60));

            return await Task.FromResult(new ResultModel<List<GetirOfisKonumDetayResponseModel>>(result));
        }
    }
}