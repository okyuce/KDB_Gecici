using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Csb.YerindeDonusum.Application.Extensions.FluentValidationExtension;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeBinaDegerlendirmeServerSide;

public class GetirListeBinaDegerlendirmeServerSideQuery : DataTableModel, IRequest<ResultModel<DataTableResponseModel<List<GetirListeBinaDegerlendirmeServerSideQueryResponseModel>>>>
{
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }
    public List<long>? BinaDegerlendirmeDurumId { get; set; }

    public class GetirListeBinaDegerlendirmeServerSideQueryQueryHandler : IRequestHandler<GetirListeBinaDegerlendirmeServerSideQuery, ResultModel<DataTableResponseModel<List<GetirListeBinaDegerlendirmeServerSideQueryResponseModel>>>>
    {
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IMapper _mapper;
        private readonly IKdsYerindedonusumBinabazliOranRepository _kdsYerindedonusumBinabazliOranRepository;
        private readonly IKdsBasvuruRepository _kdsBasvuruRepository;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;

        public GetirListeBinaDegerlendirmeServerSideQueryQueryHandler(IMapper mapper,
            IKdsYerindedonusumBinabazliOranRepository kdsYerindedonusumBinabazliOranRepository,
            IKdsBasvuruRepository kdsBasvuruRepository,
            IKullaniciBilgi kullaniciBilgi,
            IBinaDegerlendirmeRepository binaDegerlendirmeRepository)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _mapper = mapper;
            _kdsBasvuruRepository = kdsBasvuruRepository;
            _kdsYerindedonusumBinabazliOranRepository = kdsYerindedonusumBinabazliOranRepository;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
        }

        public async Task<ResultModel<DataTableResponseModel<List<GetirListeBinaDegerlendirmeServerSideQueryResponseModel>>>> Handle(GetirListeBinaDegerlendirmeServerSideQuery request, CancellationToken cancellationToken)
        {
            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();

            if (kullaniciBilgi.BirimIlId > 0)
                request.UavtIlNo = kullaniciBilgi.BirimIlId;     

            var queryBinaBazliOran = _kdsYerindedonusumBinabazliOranRepository.GetWhere(predicate: null, asNoTracking: true);

            if (NotEmpty(request?.UavtIlNo))
            {
                queryBinaBazliOran = queryBinaBazliOran.Where(x => request.UavtIlNo == x.UavtIlNo);
            }
            if (NotEmpty(request?.UavtIlceNo))
            {
                queryBinaBazliOran = queryBinaBazliOran.Where(x => request.UavtIlceNo == x.UavtIlceNo);
            }
            if (NotEmpty(request?.UavtMahalleNo))
            {
                queryBinaBazliOran = queryBinaBazliOran.Where(x => request.UavtMahalleNo == x.UavtMahalleNo);
            }
            if (NotWhiteSpace(request?.HasarTespitAskiKodu))
            {
                request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);
                queryBinaBazliOran = queryBinaBazliOran.Where(x => request.HasarTespitAskiKodu == x.HasarTespitAskiKodu);
            }
                
            if (NotWhiteSpace(request?.TapuAda) || NotWhiteSpace(request?.TapuParsel))
            {
                var queryBasvuru = _kdsBasvuruRepository
                            .GetWhere(x =>
                                x.SilindiMi == false
                                &&
                                x.AktifMi == true
                                &&
                                x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                &&
                                x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                            );

                if (NotWhiteSpace(request?.TapuAda))
                    queryBasvuru = queryBasvuru.Where(x => request.TapuAda.Trim() == x.TapuAda.Trim());
                if (NotWhiteSpace(request?.TapuParsel))
                    queryBasvuru = queryBasvuru.Where(x => request.TapuParsel.Trim() == x.TapuParsel);

                queryBinaBazliOran = (
                    from binaBazliOran in queryBinaBazliOran
                    where queryBasvuru.Any(x => x.UavtMahalleNo == binaBazliOran.UavtMahalleNo && x.HasarTespitAskiKodu == binaBazliOran.HasarTespitAskiKodu)
                    select binaBazliOran
                );
            }

            queryBinaBazliOran = queryBinaBazliOran.OrderBy(o => o.Id);

            var liste = _mapper.Map<List<GetirListeBinaDegerlendirmeServerSideQueryResponseModel>>(queryBinaBazliOran.Skip(request.start).Take(request.length).ToList());

            foreach (var item in liste)
            {

                var binaDegerlendirme = await _binaDegerlendirmeRepository.GetAllQueryable(x => x.AktifMi == true && x.SilindiMi == false
                                                                                            && x.UavtMahalleNo == item.UavtMahalleNo
                                                                                            && x.HasarTespitAskiKodu == item.HasarTespitAskiKodu
                                    ).Include(i => i.Basvurus.Where(y => y.SilindiMi == false && y.AktifMi == true
                                                                    && y.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.Kabul
                                                                    && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                                    && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir))
                                        .ThenInclude(i => i.BasvuruImzaVerens.Where(y => y.SilindiMi == false && y.AktifMi == true && y.BagimsizBolumAlani > 0))
                                    .Include(i => i.BinaDegerlendirmeDurum)
                                    .FirstOrDefaultAsync();

                if (binaDegerlendirme == null)
                {
                    continue;
                }

                item.BinaDegerlendirmeId = binaDegerlendirme?.BinaDegerlendirmeId ?? 0;
                item.BasvuruDegerlendirmeDurumAd = binaDegerlendirme?.BinaDegerlendirmeDurum?.Ad;
                item.BinaDegerlendirmeDurumId = binaDegerlendirme?.BinaDegerlendirmeDurumId;
                item.OdemeDurumAd = "Ödeme Bekleniyor.";
            }

            int toplamKayit = queryBinaBazliOran.Count();

            if (NotEmpty(request?.BinaDegerlendirmeDurumId))
            {
                liste = liste.Where(x => request.BinaDegerlendirmeDurumId.Any(y=> y == x.BinaDegerlendirmeDurumId)).ToList();
            }

            return await Task.FromResult(new ResultModel<DataTableResponseModel<List<GetirListeBinaDegerlendirmeServerSideQueryResponseModel>>>(new DataTableResponseModel<List<GetirListeBinaDegerlendirmeServerSideQueryResponseModel>>
            {
                data = liste,
                draw = request.draw,
                recordsFiltered = toplamKayit,
                recordsTotal = toplamKayit
            }));
        }
    }
}