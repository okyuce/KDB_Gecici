using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeOdemeYapilanServerSide;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirBinaOdemeRapor;

public class GetirListeBinaOdemeRaporQuery : IRequest<ResultModel<List<GetirListeBinaOdemeRaporQueryResponseModel>>>
{
    public int? UavtIlNo { get; set; }

    public class GetirBinaOdemeRaporQueryHandler : IRequestHandler<GetirListeBinaOdemeRaporQuery, ResultModel<List<GetirListeBinaOdemeRaporQueryResponseModel>>>
    {
        private readonly IBinaOdemeRepository _binaOdemeRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GetirBinaOdemeRaporQueryHandler(IKullaniciBilgi kullaniciBilgi, IBinaOdemeRepository binaOdemeRepository)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _binaOdemeRepository = binaOdemeRepository;
        }

        public async Task<ResultModel<List<GetirListeBinaOdemeRaporQueryResponseModel>>> Handle(GetirListeBinaOdemeRaporQuery request, CancellationToken cancellationToken)
        {
            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();

            if (kullaniciBilgi.BirimIlId > 0)
                request.UavtIlNo = kullaniciBilgi.BirimIlId;

            var result = new ResultModel<List<GetirListeBinaOdemeRaporQueryResponseModel>>();

            var query = _binaOdemeRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                        && !x.BinaDegerlendirme.SilindiMi
                                        && x.BinaOdemeDurumId != (int)BinaOdemeDurumEnum.Reddedildi
                                        && x.BinaDegerlendirme.AktifMi == true
                                        && (x.BinaDegerlendirme.BultenNo > 0
                                        || x.BinaDegerlendirme.BinaYapiRuhsatIzinDosyas.Any(y => y.SilindiMi == false && y.AktifMi == true)),
                                        false
                                    ).Include(x => x.BinaDegerlendirme)
                                     .ThenInclude(x => x.Basvurus)
                                     .ThenInclude(x => x.BasvuruImzaVerens)
                                     .Include(x => x.BinaDegerlendirme.BinaMuteahhits)
                                     .Include(x => x.BinaDegerlendirme.BinaYapiRuhsatIzinDosyas)
                                     .AsQueryable();

            if (request.UavtIlNo > 0)
                query = query.Where(x => x.BinaDegerlendirme.UavtIlNo == request.UavtIlNo);


            result.Result = query.Select(s => new GetirListeBinaOdemeRaporQueryResponseModel
            {
                BinaOdemeId = s.BinaOdemeId,
                BinaDegerlendirmeId = s.BinaDegerlendirmeId,
                Seviye = s.Seviye,
                UavtIlNo = s.BinaDegerlendirme.UavtIlNo,
                UavtIlAdi = s.BinaDegerlendirme.UavtIlAdi,
                UavtIlceNo = s.BinaDegerlendirme.UavtIlceNo,
                UavtIlceAdi = s.BinaDegerlendirme.UavtIlceAdi,
                UavtMahalleAdi = s.BinaDegerlendirme.UavtMahalleAdi,
                Ada = s.BinaDegerlendirme.Ada,
                Parsel = s.BinaDegerlendirme.Parsel,
                HasarTespitAskiKodu = s.BinaDegerlendirme.HasarTespitAskiKodu,
                YapiKimlikNo = s.BinaDegerlendirme.YapiKimlikNo,
                BultenNo = s.BinaDegerlendirme.BultenNo,
                IzinBelgesiSayi = s.BinaDegerlendirme.IzinBelgesiSayi,
                IzinBelgesiTarih = s.BinaDegerlendirme.IzinBelgesiTarih,
                HibeOdemeTutari = s.HibeOdemeTutari,
                KrediOdemeTutari = s.KrediOdemeTutari,
                DigerHibeOdemeTutari = s.DigerHibeOdemeTutari,
                OdemeTutari = s.OdemeTutari,
                OdemeKimeYapildi = s.BinaOdemeDetays.Select(s => new GetirListeOdemeYapilanServerSideResponseModel
                {
                    Adi = s.Ad,
                    DigerOdemeTutari = s.DigerHibeOdemeTutari,
                    IbanNo = s.Iban,
                    Tipi = s.MuteahhitMi ? "Müteahhit" : "Vatandaş",
                    HibeOdemeTutari = s.HibeOdemeTutari,
                    KrediOdemeTutari = s.KrediOdemeTutari,
                    OdemeTutari = s.OdemeTutari
                }).ToList()
            }).ToList();            

            return await Task.FromResult(result);
        }
    }
}