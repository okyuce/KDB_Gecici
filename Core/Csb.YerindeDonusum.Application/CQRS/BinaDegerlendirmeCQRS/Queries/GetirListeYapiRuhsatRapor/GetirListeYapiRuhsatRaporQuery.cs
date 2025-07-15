using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using static Csb.YerindeDonusum.Application.Extensions.FluentValidationExtension;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeYapiRuhsatRapor;

public class GetirListeYapiRuhsatRaporQuery : IRequest<ResultModel<List<GetirListeYapiRuhsatRaporQueryResponseModel>>>
{
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? HasarTespitAskiKodu { get; set; }

    public class GetirListeYapiRuhsatRaporQueryHandler : IRequestHandler<GetirListeYapiRuhsatRaporQuery, ResultModel<List<GetirListeYapiRuhsatRaporQueryResponseModel>>>
    {
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GetirListeYapiRuhsatRaporQueryHandler(IKullaniciBilgi kullaniciBilgi, IBinaDegerlendirmeRepository binaDegerlendirmeRepository)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
        }

        public async Task<ResultModel<List<GetirListeYapiRuhsatRaporQueryResponseModel>>> Handle(GetirListeYapiRuhsatRaporQuery request, CancellationToken cancellationToken)
        {
            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();

            if (kullaniciBilgi.BirimIlId > 0)
                request.UavtIlNo = kullaniciBilgi.BirimIlId;

            var result = new ResultModel<List<GetirListeYapiRuhsatRaporQueryResponseModel>>();

            var query = _binaDegerlendirmeRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                    && (x.BultenNo > 0 
                                                            || x.BinaYapiRuhsatIzinDosyas.Any(y => !y.SilindiMi && y.AktifMi == true))                                                             
                                                    , true
                                                    , i=> i.BinaYapiRuhsatIzinDosyas.Where(y => !y.SilindiMi && y.AktifMi == true));

            if (NotEmpty(request?.UavtIlNo))
            {
                query = query.Where(x => request.UavtIlNo == x.UavtIlNo);
            }
            if (NotEmpty(request?.UavtIlceNo))
            {
                query = query.Where(x => request.UavtIlceNo == x.UavtIlceNo);
            }
            if (NotEmpty(request?.UavtMahalleNo))
            {
                query = query.Where(x => request.UavtMahalleNo == x.UavtMahalleNo);
            }
            if (NotWhiteSpace(request?.HasarTespitAskiKodu))
            {
                request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);
                query = query.Where(x => request.HasarTespitAskiKodu == x.HasarTespitAskiKodu);
            }
                
            result.Result = query.GroupBy(g => g.UavtIlAdi)
                                .Select(s => new GetirListeYapiRuhsatRaporQueryResponseModel
                                {
                                    UavtIlAdi = s.Key,
                                    YapiRuhsatSayi = s.Count()
                                })
                                .OrderBy(o => o.UavtIlAdi)
                                .ToList();

            return await Task.FromResult(result);
        }
    }
}