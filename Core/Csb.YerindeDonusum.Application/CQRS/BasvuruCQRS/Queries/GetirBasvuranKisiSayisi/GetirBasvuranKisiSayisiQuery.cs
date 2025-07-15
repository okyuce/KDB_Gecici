using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuranKisiSayisi;

public class GetirBasvuranKisiSayisiQuery : IRequest<ResultModel<GetirBasvuranKisiSayisiQueryResponseModel>>
{
    public class GetirBasvuranKisiSayisiQueryHandler : IRequestHandler<GetirBasvuranKisiSayisiQuery, ResultModel<GetirBasvuranKisiSayisiQueryResponseModel>>
    {
        private readonly IBasvuruRepository _basvuruRepository;

        public GetirBasvuranKisiSayisiQueryHandler(IBasvuruRepository basvuruRepository)
        {
            _basvuruRepository = basvuruRepository;
        }

        public async Task<ResultModel<GetirBasvuranKisiSayisiQueryResponseModel>> Handle(GetirBasvuranKisiSayisiQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirBasvuranKisiSayisiQueryResponseModel>();
            try
            {
                var query = _basvuruRepository.GetAllQueryable(x =>
                            x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzAlinmistir
                            &&
                            x.AktifMi == true
                            &&
                            !x.SilindiMi
                    );

                var basvuranGercekKisiSayisi = query.Where(x => x.TuzelKisiTipId == null).GroupBy(x => x.TcKimlikNo).Count();
                var basvuranTuzelKisiSayisi = query.Where(x => x.TuzelKisiTipId != null).GroupBy(x => x.TuzelKisiMersisNo).Count();

                result.Result = new GetirBasvuranKisiSayisiQueryResponseModel {
                    BasvuranGercekKisiSayisi = basvuranGercekKisiSayisi,
                    BasvuranTuzelKisiSayisi = basvuranTuzelKisiSayisi
                };
            }
            catch (Exception ex)
            {
                result.Exception(ex, "Başvuran Kişi Sayısı Alınırken Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}