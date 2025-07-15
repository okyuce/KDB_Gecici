using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirYapiDenetimSeviyeTespitBilgiler;

public class GetirYapiDenetimSeviyeTespitBilgilerQuery : IRequest<ResultModel<List<GetirYapiDenetimSeviyeTespitBilgilerQueryResponseModel>>>
{
    public long? BinaDegerlendirmeId { get; set; }

    public class GetirYapiDenetimSeviyeTespitBilgilerQueryHandler : IRequestHandler<GetirYapiDenetimSeviyeTespitBilgilerQuery, ResultModel<List<GetirYapiDenetimSeviyeTespitBilgilerQueryResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;

        public GetirYapiDenetimSeviyeTespitBilgilerQueryHandler(IMapper mapper, IBinaDegerlendirmeRepository binaDegerlendirmeRepository)
        {
            _mapper = mapper;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
        }

        public Task<ResultModel<List<GetirYapiDenetimSeviyeTespitBilgilerQueryResponseModel>>> Handle(GetirYapiDenetimSeviyeTespitBilgilerQuery request, CancellationToken cancellationToken)
        {
            var binaDegerlendirme = _binaDegerlendirmeRepository.GetWhere(x => x.AktifMi == true && x.SilindiMi == false 
                                                                && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId
                                                        , true)
                                                   .Include(x => x.BinaYapiDenetimSeviyeTespits.Where(x => x.AktifMi == true && x.SilindiMi == false))
                                                        .ThenInclude(x => x.BinaYapiDenetimSeviyeTespitDosyas.Where(x => x.AktifMi == true && x.SilindiMi == false))
                                                   .FirstOrDefault();

            if(binaDegerlendirme == null)
            {
                return Task.FromResult(new ResultModel<List<GetirYapiDenetimSeviyeTespitBilgilerQueryResponseModel>>
                                                    (new List<GetirYapiDenetimSeviyeTespitBilgilerQueryResponseModel>()));
            }

            var result = _mapper.Map<List<GetirYapiDenetimSeviyeTespitBilgilerQueryResponseModel>>(binaDegerlendirme.BinaYapiDenetimSeviyeTespits);
            return Task.FromResult(new ResultModel<List<GetirYapiDenetimSeviyeTespitBilgilerQueryResponseModel>>(result));
        }
    }
}