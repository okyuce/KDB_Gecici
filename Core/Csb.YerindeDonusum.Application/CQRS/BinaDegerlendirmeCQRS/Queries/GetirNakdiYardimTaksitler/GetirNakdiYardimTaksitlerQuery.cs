using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirNakdiYardimTaksitler;

public class GetirNakdiYardimTaksitlerQuery : IRequest<ResultModel<List<GetirNakdiYardimTaksitlerQueryResponseModel>>>
{
    public long? BinaDegerlendirmeId { get; set; }

    public class GetirNakdiYardimTaksitlerQueryHandler : IRequestHandler<GetirNakdiYardimTaksitlerQuery, ResultModel<List<GetirNakdiYardimTaksitlerQueryResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IBinaOdemeRepository _binaOdemeRepository;

        public GetirNakdiYardimTaksitlerQueryHandler(IMapper mapper, IBinaDegerlendirmeRepository binaDegerlendirmeRepository, IBinaOdemeRepository binaOdemeRepository)
        {
            _mapper = mapper;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _binaOdemeRepository = binaOdemeRepository;
        }

        public async Task<ResultModel<List<GetirNakdiYardimTaksitlerQueryResponseModel>>> Handle(GetirNakdiYardimTaksitlerQuery request, CancellationToken cancellationToken)
        {
            var result = new List<GetirNakdiYardimTaksitlerQueryResponseModel>();
            
            var binaOdemeListe = await _binaOdemeRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                        && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId
                                               , true
                                               , i => i.BinaOdemeDurum
                                               , i => i.BinaDegerlendirme.BinaMuteahhits.Where(o => !o.SilindiMi && o.AktifMi == true)                                              
                                               ).OrderBy(x=> x.OlusturmaTarihi)
                                                .ThenBy(x=> x.BinaOdemeDurumId).ToListAsync();

            var binaOdemeListeMapped = _mapper.Map<List<GetirNakdiYardimTaksitlerQueryResponseModel>>(binaOdemeListe);

            return await Task.FromResult(new ResultModel<List<GetirNakdiYardimTaksitlerQueryResponseModel>>(binaOdemeListeMapped)); 
        }
    }
}