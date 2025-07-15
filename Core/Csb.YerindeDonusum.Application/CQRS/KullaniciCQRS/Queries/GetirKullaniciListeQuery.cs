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

namespace Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS;

public class GetirKullaniciListeQuery : IRequest<ResultModel<List<KullaniciDto>>>
{
    public string? Arama { get; set; }
    public bool? SilindiMi { get; set; } = false;

    public class GetirKullaniciListeQueryHandler : IRequestHandler<GetirKullaniciListeQuery, ResultModel<List<KullaniciDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IKullaniciRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;

        public GetirKullaniciListeQueryHandler(IMapper mapper, IKullaniciRepository repository, IWebHostEnvironment webHostEnvironment, ICacheService cacheService)
        {
            _mapper = mapper;
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
            _cacheService = cacheService;
        }

        public async Task<ResultModel<List<KullaniciDto>>> Handle(GetirKullaniciListeQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirKullaniciListeQuery)}";
            var r = await _cacheService.GetValueAsync(cacheKey);
            if (r != null)
                return new ResultModel<List<KullaniciDto>>(JsonConvert.DeserializeObject<List<KullaniciDto>>(r));

            var result = _mapper.Map<List<KullaniciDto>>(
                                        _repository.GetWhereEnumerable(x => x.SilindiMi == request.SilindiMi &&
                                        (StringAddons.Search(x.KullaniciAdi, request.Arama)
                                                     || StringAddons.Search(x.TcKimlikNo.ToString(), request.Arama))
                                ).ToList()
            );

            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(60));

            return await Task.FromResult(new ResultModel<List<KullaniciDto>>(result));
        }
    }
}