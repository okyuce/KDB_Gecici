using AutoMapper;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirDetayHibeTaahhutnameSozlesme;

public class GetirDetayHibeTaahhutnameSozlesmeQuery : IRequest<ResultModel<GetirDetayHibeTaahhutnameSozlesmeQueryResponseModel>>
{
    public Guid? BasvuruGuid { get; set; }

    public class GetirDetayHibeTaahhutnameSozlesmeQueryHandler : IRequestHandler<GetirDetayHibeTaahhutnameSozlesmeQuery, ResultModel<GetirDetayHibeTaahhutnameSozlesmeQueryResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruImzaVerenRepository _basvuruImzaVerenRepository;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;

        public GetirDetayHibeTaahhutnameSozlesmeQueryHandler(IMapper mapper, IBasvuruImzaVerenRepository basvuruImzaVerenRepository, IBinaDegerlendirmeRepository binaDegerlendirmeRepository)
        {
            _mapper = mapper;
            _basvuruImzaVerenRepository = basvuruImzaVerenRepository;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
        }

        public Task<ResultModel<GetirDetayHibeTaahhutnameSozlesmeQueryResponseModel>> Handle(GetirDetayHibeTaahhutnameSozlesmeQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirDetayHibeTaahhutnameSozlesmeQueryResponseModel>();

            var basvuruImzaVeren = _basvuruImzaVerenRepository.GetWhere(x =>
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
                                                    i => i.Basvuru,
                                                    i => i.Basvuru.BinaDegerlendirme.BinaOdemes

                                                ).FirstOrDefault();

            if (basvuruImzaVeren == null)
                result.ErrorMessage("Başvuru imza veren detayı bulunamadı!");

            result.Result = _mapper.Map<GetirDetayHibeTaahhutnameSozlesmeQueryResponseModel>(basvuruImzaVeren);

            return Task.FromResult(result);
        }
    }
}