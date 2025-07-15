using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirRezervAlanListeAfadIcin;

public class GetirRezervAlanListeAfadIcinQuery : IRequest<ResultModel<GetirRezervAlanListeAfadIcinQueryResponseModel>>
{
    public int Offset { get; set; }

    public class GetirRezervAlanListeAfadIcinQueryHandler : IRequestHandler<GetirRezervAlanListeAfadIcinQuery, ResultModel<GetirRezervAlanListeAfadIcinQueryResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IKdsYerindedonusumRezervAlanlarRepository _kdsYerindedonusumRezervAlanlarRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;

        public GetirRezervAlanListeAfadIcinQueryHandler(IMapper mapper, IWebHostEnvironment webHostEnvironment, ICacheService cacheService, IKdsYerindedonusumRezervAlanlarRepository kdsYerindedonusumRezervAlanlarRepository)
        {
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _cacheService = cacheService;
            _kdsYerindedonusumRezervAlanlarRepository = kdsYerindedonusumRezervAlanlarRepository;
        }

        public async Task<ResultModel<GetirRezervAlanListeAfadIcinQueryResponseModel>> Handle(GetirRezervAlanListeAfadIcinQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirRezervAlanListeAfadIcinQueryResponseModel>();

            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirRezervAlanListeAfadIcinQuery)}_{request.Offset}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
            {
                result.Result = JsonConvert.DeserializeObject<GetirRezervAlanListeAfadIcinQueryResponseModel>(redisCache);
                return await Task.FromResult(result);
            }

            try
            {
                var query = _kdsYerindedonusumRezervAlanlarRepository.GetAllQueryable();

                var toplamRezervAlanSayisi = await query.CountAsync();

                if (toplamRezervAlanSayisi < request.Offset)
                    result.ErrorMessage($"Talep Edilen Offset Sayısı Kadar Rezerv Alan Bulunmamaktadır!");

                result.Result = new GetirRezervAlanListeAfadIcinQueryResponseModel
                {
                    ToplamRezervAlanSayisi = toplamRezervAlanSayisi,
                    RezervAlanListe = _mapper.Map<List<GetirRezervAlanListeAfadIcinQueryResponseModelDetay>>(query.OrderBy(o => o.Id).Skip(request.Offset).Take(1000))
                };

                await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result.Result), TimeSpan.FromHours(1));
            }
            catch (Exception ex)
            {
                result.Exception(ex, "Rezerv Alan Listesi Alınırken Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}