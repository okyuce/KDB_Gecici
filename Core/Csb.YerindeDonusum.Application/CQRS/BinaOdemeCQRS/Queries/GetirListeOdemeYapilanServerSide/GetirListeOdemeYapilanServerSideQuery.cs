using AutoMapper;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeOdemeYapilanServerSide;

public class GetirListeOdemeYapilanServerSideQuery : DataTableModel, IRequest<ResultModel<DataTableResponseModel<List<GetirListeOdemeYapilanServerSideResponseModel>>>>
{
    public long BinaOdemeId { get; set; }

    public class GetirListeOdemeYapilanServerSideQueryHandler : IRequestHandler<GetirListeOdemeYapilanServerSideQuery, ResultModel<DataTableResponseModel<List<GetirListeOdemeYapilanServerSideResponseModel>>>>
    {
        private readonly IBinaOdemeRepository _binaOdemeRepository;

        public GetirListeOdemeYapilanServerSideQueryHandler(IBinaDegerlendirmeRepository binaDegerlendirmeRepository, IBinaOdemeRepository binaOdemeRepository, IMapper mapper, IBasvuruRepository appealRepository, ITakbisService takbisService, IMediator mediator)
        {
            _binaOdemeRepository = binaOdemeRepository;
        }

        public async Task<ResultModel<DataTableResponseModel<List<GetirListeOdemeYapilanServerSideResponseModel>>>> Handle(GetirListeOdemeYapilanServerSideQuery request, CancellationToken cancellationToken)
        {
            var query = _binaOdemeRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                                && x.BinaDegerlendirme.BinaDegerlendirmeDurumId != (long)BinaDegerlendirmeDurumEnum.Reddedildi
                                                && (x.BinaDegerlendirme.BultenNo > 0 || x.BinaDegerlendirme.IzinBelgesiSayi > 0)
                                                && x.BinaOdemeId == (long)request.BinaOdemeId
                                            , true)
                    .Include(x => x.BinaDegerlendirme)
                        .ThenInclude(x => x.Basvurus)
                            .ThenInclude(x => x.BasvuruImzaVerens)
                    .Include(x => x.BinaDegerlendirme.BinaMuteahhits)
                    .Include(x => x.BinaOdemeDurum)
                    .OrderBy(x => x.BinaOdemeDurumId)
                    .ThenBy(x => x.OlusturmaTarihi)
                    .Select(x => x.BinaOdemeDetays.Select(s => new GetirListeOdemeYapilanServerSideResponseModel
                    {
                        Adi = s.Ad,
                        DigerOdemeTutari = s.DigerHibeOdemeTutari,
                        IbanNo = s.Iban,
                        Tipi = s.MuteahhitMi ? "Müteahhit" : "Vatandaş",
                        HibeOdemeTutari = s.HibeOdemeTutari,
                        KrediOdemeTutari = s.KrediOdemeTutari,
                        OdemeTutari = s.OdemeTutari
                    }).ToList()).FirstOrDefault();

            var grouppedQuery = query.GroupBy(g => new { g.Adi, g.IbanNo, g.Tipi })
                                    .Select(s => new GetirListeOdemeYapilanServerSideResponseModel
                                    {
                                        Adi = s.Select(x => x.Adi).FirstOrDefault(),
                                        DigerOdemeTutari = s.Sum(x=>x.DigerOdemeTutari),
                                        IbanNo = s.Select(x => x.IbanNo).FirstOrDefault(),
                                        Tipi = s.Select(x => x.Tipi).FirstOrDefault(),
                                        HibeOdemeTutari = s.Sum(x => x.HibeOdemeTutari),
                                        KrediOdemeTutari = s.Sum(x => x.KrediOdemeTutari),
                                        OdemeTutari = s.Sum(x => x.HibeOdemeTutari) +
                                                      s.Sum(x => x.KrediOdemeTutari) +
                                                      s.Sum(x => x.DigerOdemeTutari)
                                    });
  
            grouppedQuery = grouppedQuery.OrderBy(o => o.Adi);

            return await grouppedQuery.Paginate(request);
        }
    }
}