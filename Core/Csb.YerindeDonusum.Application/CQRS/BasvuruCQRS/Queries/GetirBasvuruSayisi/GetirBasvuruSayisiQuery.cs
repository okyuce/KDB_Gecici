using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruSayisi;

public class GetirBasvuruSayisiQuery : IRequest<ResultModel<GetirBasvuruSayisiQueryResponseModel>>
{
    public class GetirBasvuruSayisiQueryHandler : IRequestHandler<GetirBasvuruSayisiQuery, ResultModel<GetirBasvuruSayisiQueryResponseModel>>
    {
        private readonly IBasvuruRepository _basvuruRepository;

        public GetirBasvuruSayisiQueryHandler(IBasvuruRepository basvuruRepository)
        {
            _basvuruRepository = basvuruRepository;
        }

        public async Task<ResultModel<GetirBasvuruSayisiQueryResponseModel>> Handle(GetirBasvuruSayisiQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirBasvuruSayisiQueryResponseModel>();
            try
            {

                var basvuruSayisi = _basvuruRepository.Count(x => x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzAlinmistir && x.AktifMi == true && !x.SilindiMi);

                result.Result = new GetirBasvuruSayisiQueryResponseModel { BasvuruSayisi = basvuruSayisi };
            }
            catch (Exception ex)
            {
                result.Exception(ex, "Başvuru Sayısı Alınırken Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}
