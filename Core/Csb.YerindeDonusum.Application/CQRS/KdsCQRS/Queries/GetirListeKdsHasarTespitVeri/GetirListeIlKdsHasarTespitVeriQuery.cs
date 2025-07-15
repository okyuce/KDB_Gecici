using AutoMapper;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.KdsHasarTespitVeriByUid
{
    public class GetirListeIlKdsHasarTespitVeriQuery : IRequest<ResultModel<List<GetirListeIlKdsHasarTespitVeriQueryResponseModel>>>
    {
        public GetirListeIlKdsHasarTespitVeriQueryModel Model { get; set; }

        public class GetirListeKdsHasarTespitVeriQueryHandler : IRequestHandler<GetirListeIlKdsHasarTespitVeriQuery, ResultModel<List<GetirListeIlKdsHasarTespitVeriQueryResponseModel>>>
        {
            private readonly IMapper _mapper;
            private readonly IKdsHaneRepository _kdsHaneRepository;
            private readonly IKdsHasartespitTespitVeriRepository _kdsHasartespitTespitVeriRepository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly ICacheService _cacheService;

            public GetirListeKdsHasarTespitVeriQueryHandler(IMapper mapper, IKdsHaneRepository kdsHaneRepository, IKdsHasartespitTespitVeriRepository kdsHasartespitTespitVeriRepository, IWebHostEnvironment webHostEnvironment, ICacheService cacheService)
            {
                _mapper = mapper;
                _kdsHaneRepository = kdsHaneRepository;
                _kdsHasartespitTespitVeriRepository = kdsHasartespitTespitVeriRepository;
                _webHostEnvironment = webHostEnvironment;
                _cacheService = cacheService;
            }

            public async Task<ResultModel<List<GetirListeIlKdsHasarTespitVeriQueryResponseModel>>> Handle(GetirListeIlKdsHasarTespitVeriQuery request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<List<GetirListeIlKdsHasarTespitVeriQueryResponseModel>>();

                var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirListeIlKdsHasarTespitVeriQuery)}";
                var redisCacheText = await _cacheService.GetValueAsync(cacheKey);
                if (redisCacheText != null)
                {
                    result.Result = JsonConvert.DeserializeObject<List<GetirListeIlKdsHasarTespitVeriQueryResponseModel>>(redisCacheText);
                }
                else
                {
                    result.Result = _kdsHaneRepository.GetWhere(x =>
                        x.IlKod != null
                        &&
                        x.IlAd != null
                        &&
                        x.AfetId == HasarTespitAfetEnum.KahramanmarasPazarcik20230206,
                        true
                    )
                    .GroupBy(g => new { g.IlKod, g.IlAd })
                    .Select(s => new GetirListeIlKdsHasarTespitVeriQueryResponseModel
                    {
                        Id = s.Key.IlKod.Value,
                        Ad = s.Key.IlAd
                    })
                    .AsEnumerable()
                    .Select(s => new GetirListeIlKdsHasarTespitVeriQueryResponseModel
                    {
                        Id = s.Id,
                        Ad = s.Ad.ToUpper(new System.Globalization.CultureInfo("tr-TR"))
                    })
                    .ToList();

                    var hasarTespitIlListe = _kdsHasartespitTespitVeriRepository.GetWhere(x =>
                       x.IlKod != null
                       &&
                       x.IlAd != null
                       &&
                       x.AfetId == HasarTespitAfetEnum.KahramanmarasPazarcik20230206,
                       true
                   )
                   .GroupBy(g => new { g.IlKod, g.IlAd })
                   .Select(s => new GetirListeIlKdsHasarTespitVeriQueryResponseModel
                   {
                       Id = s.Key.IlKod.Value,
                       Ad = s.Key.IlAd
                   })
                   .AsEnumerable()
                   .Select(s => new GetirListeIlKdsHasarTespitVeriQueryResponseModel
                   {
                       Id = s.Id,
                       Ad = s.Ad.ToUpper(new System.Globalization.CultureInfo("tr-TR"))
                   })
                   .Where(x => !result.Result.Any(y => y.Id == x.Id))
                   .ToList();

                    if (hasarTespitIlListe.Any())
                        result.Result.AddRange(hasarTespitIlListe);

                    result.Result = result.Result.OrderBy(o => o.Ad).ToList();

                    await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result.Result), TimeSpan.FromMinutes(60));
                }

                return await Task.FromResult(result);
            }
        }
    }
}