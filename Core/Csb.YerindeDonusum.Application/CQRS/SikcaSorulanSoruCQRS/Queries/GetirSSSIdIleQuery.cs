using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.StringAddons;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS;

public class GetirSSSIdIleQuery : IRequest<ResultModel<SikcaSorulanSoruDto>>
{
    public long? SikcaSorulanSoruId { get; set; }

    public class GetirIdIleQueryHandler : IRequestHandler<GetirSSSIdIleQuery, ResultModel<SikcaSorulanSoruDto>>
    {
        private readonly IMapper _mapper;
        private readonly ISikcaSorulanSoruRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;

        public GetirIdIleQueryHandler(IMapper mapper, ISikcaSorulanSoruRepository repository, IWebHostEnvironment webHostEnvironment, ICacheService cacheService)
        {
            _mapper = mapper;
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
            _cacheService = cacheService;
        }

        public async Task<ResultModel<SikcaSorulanSoruDto>> Handle(GetirSSSIdIleQuery request, CancellationToken cancellationToken)
        {
            //var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirIdIleQuery)}_{request?.Arama}";
            //var r = await _cacheService.GetValueAsync(cacheKey);
            //if (r != null)
            //    return new ResultModel<List<SikcaSorulanSoruDto>>(JsonConvert.DeserializeObject<List<SikcaSorulanSoruDto>>(r));

            var model = await _repository.GetWhere(x => x.SikcaSorulanSoruId == request.SikcaSorulanSoruId).FirstOrDefaultAsync();

            //await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromMinutes(60));

            return await Task.FromResult(new ResultModel<SikcaSorulanSoruDto>(_mapper.Map<SikcaSorulanSoruDto>(model)));
        }
    }
}