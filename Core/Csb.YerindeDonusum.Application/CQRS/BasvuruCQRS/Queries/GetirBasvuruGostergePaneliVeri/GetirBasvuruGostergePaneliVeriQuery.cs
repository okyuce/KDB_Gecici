using Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruGostergePaneliVeri;

public class GetirBasvuruGostergePaneliVeriQuery : IRequest<ResultModel<GetirBasvuruGostergePaneliVeriQueryResponseModel>>
{
    public class GetirBasvuruGostergePaneliVeriQueryHandler : IRequestHandler<GetirBasvuruGostergePaneliVeriQuery, ResultModel<GetirBasvuruGostergePaneliVeriQueryResponseModel>>
    {
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;

        public GetirBasvuruGostergePaneliVeriQueryHandler(IBasvuruRepository basvuruRepository, ICacheService cacheService, IWebHostEnvironment webHostEnvironment)
        {
            _basvuruRepository = basvuruRepository;
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ResultModel<GetirBasvuruGostergePaneliVeriQueryResponseModel>> Handle(GetirBasvuruGostergePaneliVeriQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirBasvuruGostergePaneliVeriQueryResponseModel>();

            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirBasvuruGostergePaneliVeriQueryHandler)}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
            {
                result.Result = JsonConvert.DeserializeObject<GetirBasvuruGostergePaneliVeriQueryResponseModel>(redisCache);
                return result;
            }

            var query = _basvuruRepository.GetWhere(x => x.AktifMi == true && !x.SilindiMi, false);

            result.Result = new GetirBasvuruGostergePaneliVeriQueryResponseModel
            {
                ToplamBasvuruSayisi = query.Count(),
                AktifBasvuruSayisi = query.Count(x => !(x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruIptalEdildi || x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir)),
                HibeKrediBasvuruSayisi = query.Count(x => x.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi),
                HibeBasvuruSayisi = query.Count(x => x.BasvuruDestekTurId == BasvuruDestekTurEnum.Hibe),
                KrediBasvuruSayisi = query.Count(x => x.BasvuruDestekTurId == BasvuruDestekTurEnum.Kredi),
                BasvuruSayiListe = query
                                    .GroupBy(g => g.OlusturmaTarihi.Date)
                                    .Select(s => new GetirBasvuruGostergePaneliVeriBasvuruTarihModel
                                    {
                                        Tarih = s.Key,
                                        BasvuruSayisi = s.Count()
                                    })
                                    .OrderByDescending(o => o.Tarih) //en yeni 15 veri alınsın, aşağıdaki take ile alınıp tolist yapıldıktan sonra tekrar order yapılıyor
                                    .Take(15)
                                    .ToList()
                                    .OrderBy(o => o.Tarih) //eskiden yeniye sıralansın
                                    .ToList()
            };

            await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result.Result), TimeSpan.FromMinutes(30));

            return await Task.FromResult(result);
        }
    }
}