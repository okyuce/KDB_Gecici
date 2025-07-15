using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BilgilendirmeMesajCQRS.Queries.GetirBilgilendirmeMesajById;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.BilgilendirmeMesajCQRS.Queries.GetirListeBilgilendirmeMesaj;

public class GetirListeBilgilendirmeMesajQuery : IRequest<ResultModel<List<GetirBilgilendirmeMesajByIdQueryResponseModel>>>
{
    internal class GetirListeBilgilendirmeMesajQueryHandler : IRequestHandler<GetirListeBilgilendirmeMesajQuery, ResultModel<List<GetirBilgilendirmeMesajByIdQueryResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IBilgilendirmeMesajRepository _bilgilendirmeMesajRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;

        public GetirListeBilgilendirmeMesajQueryHandler(IMapper mapper, IBilgilendirmeMesajRepository bilgilendirmeMesajRepository, ICacheService cacheService, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _bilgilendirmeMesajRepository = bilgilendirmeMesajRepository;
            _cacheService = cacheService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ResultModel<List<GetirBilgilendirmeMesajByIdQueryResponseModel>>> Handle(GetirListeBilgilendirmeMesajQuery request, CancellationToken cancellationToken)
        {
            ResultModel<List<GetirBilgilendirmeMesajByIdQueryResponseModel>> result = new();

            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirListeBilgilendirmeMesajQuery)}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
            {
                result.Result = JsonConvert.DeserializeObject<List<GetirBilgilendirmeMesajByIdQueryResponseModel>>(redisCache);
                return result;
            }

            try
            {
                var queryResult = await _bilgilendirmeMesajRepository.GetWhere(x => !x.SilindiMi).ToListAsync(cancellationToken);

                if (queryResult != null && queryResult.Count > 0)
                {
                    result.Result = _mapper.Map<List<GetirBilgilendirmeMesajByIdQueryResponseModel>>(queryResult);

                    await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result.Result), TimeSpan.FromMinutes(10));
                }
            }
            catch (Exception ex)
            {
                result.Exception(ex, "Bilgilendirme Mesajı Bilgisi Alınırken Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz. ERR-GLBMBAQ-GLBMBAQH-1000");
            }

            return result;
        }
    }
}