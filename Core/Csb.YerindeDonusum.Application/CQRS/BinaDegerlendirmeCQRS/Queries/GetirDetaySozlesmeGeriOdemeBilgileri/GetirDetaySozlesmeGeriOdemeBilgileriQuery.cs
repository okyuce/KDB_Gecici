using AutoMapper;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirDetaySozlesmeGeriOdemeBilgileri
{
    public class GetirDetaySozlesmeGeriOdemeBilgileriQuery : IRequest<ResultModel<GetirDetaySozlesmeGeriOdemeBilgileriQueryResponseModel>>
    {
        public Guid? BasvuruGuid { get; set; }

        public class GetirDetaySozlesmeGeriOdemeBilgileriQueryHandler : IRequestHandler<GetirDetaySozlesmeGeriOdemeBilgileriQuery, ResultModel<GetirDetaySozlesmeGeriOdemeBilgileriQueryResponseModel>>
        {
            private readonly IMapper _mapper;
            private readonly IBasvuruImzaVerenRepository _basvuruImzaVerenRepository;

            public GetirDetaySozlesmeGeriOdemeBilgileriQueryHandler(IMapper mapper, IBasvuruImzaVerenRepository basvuruImzaVerenRepository)
            {
                _mapper = mapper;
                _basvuruImzaVerenRepository = basvuruImzaVerenRepository;
            }

            public Task<ResultModel<GetirDetaySozlesmeGeriOdemeBilgileriQueryResponseModel>> Handle(GetirDetaySozlesmeGeriOdemeBilgileriQuery request, CancellationToken cancellationToken)
            {
                var result = new ResultModel<GetirDetaySozlesmeGeriOdemeBilgileriQueryResponseModel>();

                var basvuruImzaVeren = _basvuruImzaVerenRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                        && (x.Basvuru == null 
                                                        || (x.Basvuru.BasvuruGuid == request.BasvuruGuid
                                                            && x.Basvuru.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                            && x.Basvuru.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir))
                                                        && (x.BasvuruKamuUstlenecek == null 
                                                        || (x.BasvuruKamuUstlenecek.BasvuruKamuUstlenecekGuid == request.BasvuruGuid
                                                            && x.BasvuruKamuUstlenecek.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                            && x.BasvuruKamuUstlenecek.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir))
                                                        ,  true,
                                                        i => i.Basvuru,
                                                        i => i.BasvuruKamuUstlenecek,
                                                        i => i.BasvuruImzaVerenDosyas
                                                    ).FirstOrDefault();

                if (basvuruImzaVeren == null)
                {
                    result.ErrorMessage("Başvuru imza veren detayı bulunamadı!");
                    return Task.FromResult(result);
                }                  

                result.Result = _mapper.Map<GetirDetaySozlesmeGeriOdemeBilgileriQueryResponseModel>(basvuruImzaVeren);
                return Task.FromResult(result);
            }

        }
    }
}
