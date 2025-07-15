using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Text.Json;

namespace Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Queries;

public class GetirOfisKonumListeQuery : IRequest<ResultModel<List<OfisKonumDto>>>
{
    public class GetirOfisKonumListeQueryHandler : IRequestHandler<GetirOfisKonumListeQuery, ResultModel<List<OfisKonumDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IOfisKonumRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;

        public GetirOfisKonumListeQueryHandler(IMapper mapper, IOfisKonumRepository repository, IWebHostEnvironment webHostEnvironment, ICacheService cacheService)
        {
            _mapper = mapper;
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
            _cacheService = cacheService;
        }

        public async Task<ResultModel<List<OfisKonumDto>>> Handle(GetirOfisKonumListeQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirOfisKonumListeQuery)}";
            var r = await _cacheService.GetValueAsync(cacheKey);
            if (r != null)
                return new ResultModel<List<OfisKonumDto>>(JsonConvert.DeserializeObject<List<OfisKonumDto>>(r));

            var result = _repository
                .GetWhere(x => x.AktifMi == true && !x.SilindiMi && x.Konum.GeometryType.Equals("POINT"), true)
                .Select(s => new OfisKonumDto
                {
                    IlAdi = s.IlAdi,
                    IlceAdi = s.IlceAdi,
                    Adres = s.Adres,
                    HaritaUrl = s.HaritaUrl,
                    Enlem = s.Konum.InteriorPoint.X,
                    Boylam = s.Konum.InteriorPoint.Y
                })
                .OrderBy(x => x.Adres)
                .ToList();

            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(60));

            return await Task.FromResult(new ResultModel<List<OfisKonumDto>>(result));
        }
    }
}