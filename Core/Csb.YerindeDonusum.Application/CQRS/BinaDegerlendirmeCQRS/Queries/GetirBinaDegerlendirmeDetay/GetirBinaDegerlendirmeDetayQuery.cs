using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeDetay;

public class GetirBinaDegerlendirmeDetayQuery : IRequest<ResultModel<GetirBinaDegerlendirmeDetayQueryResponseModel>>
{
    public long? BinaDegerlendirmeId { get; set; }

    public class GetirBinaDegerlendirmeDetayQueryHandler : IRequestHandler<GetirBinaDegerlendirmeDetayQuery, ResultModel<GetirBinaDegerlendirmeDetayQueryResponseModel>>
    {
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IMapper _mapper;


        public GetirBinaDegerlendirmeDetayQueryHandler(IBinaDegerlendirmeRepository binaDegerlendirmeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
        }

        public async Task<ResultModel<GetirBinaDegerlendirmeDetayQueryResponseModel>> Handle(GetirBinaDegerlendirmeDetayQuery request, CancellationToken cancellationToken)
        {
            ResultModel<GetirBinaDegerlendirmeDetayQueryResponseModel> result = new();

            var binaDegerlendirme = await _binaDegerlendirmeRepository.GetWhere(x => x.AktifMi == true && x.SilindiMi == false
                                                && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId
                                            , true,
                                            i => i.BinaMuteahhits,
                                            i => i.BinaDegerlendirmeDosyas,
                                            i => i.BinaYapiRuhsatIzinDosyas
                                        ).FirstOrDefaultAsync();

            result.Result = _mapper.Map<GetirBinaDegerlendirmeDetayQueryResponseModel>(binaDegerlendirme);

            return await Task.FromResult(result);
        }
    }
}