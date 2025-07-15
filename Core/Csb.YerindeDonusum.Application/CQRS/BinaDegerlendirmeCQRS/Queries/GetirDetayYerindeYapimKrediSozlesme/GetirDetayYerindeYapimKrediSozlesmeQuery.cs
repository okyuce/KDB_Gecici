using AutoMapper;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirDetayYerindeYapimKrediSozlesme;

public class GetirDetayYerindeYapimKrediSozlesmeQuery : IRequest<ResultModel<GetirDetayYerindeYapimKrediSozlesmeQueryResponseModel>>
{
    public Guid? BasvuruGuid { get; set; }

    public class GetirDetayYerindeYapimKrediSozlesmeQueryHandler : IRequestHandler<GetirDetayYerindeYapimKrediSozlesmeQuery, ResultModel<GetirDetayYerindeYapimKrediSozlesmeQueryResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruImzaVerenRepository _basvuruImzaVerenRepository;

        public GetirDetayYerindeYapimKrediSozlesmeQueryHandler(IMapper mapper, IBasvuruImzaVerenRepository basvuruImzaVerenRepository)
        {
            _mapper = mapper;
            _basvuruImzaVerenRepository = basvuruImzaVerenRepository;
        }

        public Task<ResultModel<GetirDetayYerindeYapimKrediSozlesmeQueryResponseModel>> Handle(GetirDetayYerindeYapimKrediSozlesmeQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirDetayYerindeYapimKrediSozlesmeQueryResponseModel>();

            var basvuru = _basvuruImzaVerenRepository.GetWhere(x =>
                                                    !x.SilindiMi
                                                    &&
                                                    x.AktifMi == true
                                                    &&
                                                    x.Basvuru.BasvuruGuid == request.BasvuruGuid
                                                    &&
                                                    x.Basvuru.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                    &&
                                                    x.Basvuru.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir,
                                                    true,
                                                    i => i.Basvuru
                                                ).FirstOrDefault();

            if (basvuru == null)
                result.ErrorMessage("Başvuru imza veren detayı bulunamadı!");

            result.Result = _mapper.Map<GetirDetayYerindeYapimKrediSozlesmeQueryResponseModel>(basvuru);

            return Task.FromResult(result);
        }
    }
}