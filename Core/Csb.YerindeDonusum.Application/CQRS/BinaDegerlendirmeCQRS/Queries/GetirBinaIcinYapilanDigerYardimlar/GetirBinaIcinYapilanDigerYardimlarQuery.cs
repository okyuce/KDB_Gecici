using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaIcinYapilanDigerYardimlar;

public class GetirBinaIcinYapilanDigerYardimlarQuery : IRequest<ResultModel<List<GetirBinaIcinYapilanDigerYardimlarQueryResponseModel>>>
{
    public long? BinaDegerlendirmeId { get; set; }

    public class GetirBinaIcinYapilanDigerYardimlarQueryHandler : IRequestHandler<GetirBinaIcinYapilanDigerYardimlarQuery, ResultModel<List<GetirBinaIcinYapilanDigerYardimlarQueryResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;

        public GetirBinaIcinYapilanDigerYardimlarQueryHandler(IMapper mapper, IBinaDegerlendirmeRepository binaDegerlendirmeRepository)
        {
            _mapper = mapper;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
        }

        public Task<ResultModel<List<GetirBinaIcinYapilanDigerYardimlarQueryResponseModel>>> Handle(GetirBinaIcinYapilanDigerYardimlarQuery request, CancellationToken cancellationToken)
        {
            var binaDegerlendirme = _binaDegerlendirmeRepository.GetWhere(x => x.AktifMi == true && x.SilindiMi == false
                                                       && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId   
                                                  , true
                                                ).Include(x => x.BinaAdinaYapilanYardims.Where(x => x.AktifMi == true && x.SilindiMi == false))
                                                    .ThenInclude(x => x.BinaAdinaYapilanYardimTipi)
                                                .FirstOrDefault();

            if (binaDegerlendirme == null)
            {
                return Task.FromResult(new ResultModel<List<GetirBinaIcinYapilanDigerYardimlarQueryResponseModel>>
                                                    (new List<GetirBinaIcinYapilanDigerYardimlarQueryResponseModel>()));
            }

            var result = _mapper.Map<List<GetirBinaIcinYapilanDigerYardimlarQueryResponseModel>>(binaDegerlendirme.BinaAdinaYapilanYardims);
            return Task.FromResult(new ResultModel<List<GetirBinaIcinYapilanDigerYardimlarQueryResponseModel>>(result));
        }
    }
}