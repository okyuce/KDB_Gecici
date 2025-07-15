using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeBinaOdemeServerSide;

public class GetirListeBinaOdemeServerSideQuery : DataTableModel, IRequest<ResultModel<DataTableResponseModel<List<BinaOdemeDto>>>>
{
    public decimal? OdemeTutari { get; set; }
    public decimal? HibeOdemeTutari { get; set; }
    public decimal? KrediOdemeTutari { get; set; }
    public decimal? DigerHibeOdemeTutari { get; set; }
    public int? Seviye { get; set; }
    public DateTime? OlusturmaTarihi { get; set; }
    public long? BinaOdemeDurumId { get; set; }
    public long? BinaDegerlendirmeId { get; set; }
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? TalepNumarasi { get; set; }
    public string? TalepDurumu { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public DateTime? TalepKapatmaTarihi { get; set; }

    public class GetirListeBinaOdemeQueryHandler : IRequestHandler<GetirListeBinaOdemeServerSideQuery, ResultModel<DataTableResponseModel<List<BinaOdemeDto>>>>
    {
        private readonly IBinaOdemeRepository _binaOdemeRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GetirListeBinaOdemeQueryHandler(IKullaniciBilgi kullaniciBilgi, IBinaDegerlendirmeRepository binaDegerlendirmeRepository, IBinaOdemeRepository binaOdemeRepository, IMapper mapper, IBasvuruRepository appealRepository, ITakbisService takbisService, IMediator mediator)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _binaOdemeRepository = binaOdemeRepository;
        }

        public async Task<ResultModel<DataTableResponseModel<List<BinaOdemeDto>>>> Handle(GetirListeBinaOdemeServerSideQuery request, CancellationToken cancellationToken)
        {
            var birimIlId = _kullaniciBilgi.GetUserInfo().BirimIlId;
            if (birimIlId > 0) 
                request.UavtIlNo = birimIlId;
            
            var query = _binaOdemeRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                        , true
                                        , i => i.BinaDegerlendirme.BinaMuteahhits.Where(o => !o.SilindiMi && o.AktifMi == true)
                                        , i => i.BinaOdemeDurum);

            if (FluentValidationExtension.NotEmpty(request.BinaDegerlendirmeId))
            {
                query = query.Where(x => x.BinaDegerlendirmeId == request.BinaDegerlendirmeId);
            }
            if (FluentValidationExtension.NotEmpty(request.UavtIlNo))
            {
                query = query.Where(x => x.BinaDegerlendirme.UavtIlNo == request.UavtIlNo);
            }
            if (FluentValidationExtension.NotEmpty(request.UavtIlceNo))
            {
                query = query.Where(x => x.BinaDegerlendirme.UavtIlceNo == request.UavtIlceNo);
            }
            if (FluentValidationExtension.NotEmpty(request.UavtMahalleNo))
            {
                query = query.Where(x => x.BinaDegerlendirme.UavtMahalleNo == request.UavtMahalleNo);
            }

            if (FluentValidationExtension.NotWhiteSpace(request.TalepNumarasi))
            {
                query = query.Where(x => x.TalepNumarasi.Contains(request.TalepNumarasi));
            }

            if (FluentValidationExtension.NotWhiteSpace(request.TalepDurumu))
            {
                query = query.Where(x => x.TalepDurumu == request.TalepDurumu);
            }

            if (FluentValidationExtension.NotWhiteSpace(request.HasarTespitAskiKodu))
            {
                request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);
                query = query.Where(x => x.BinaDegerlendirme.HasarTespitAskiKodu == request.HasarTespitAskiKodu);
            }

            if (FluentValidationExtension.NotEmpty(request.TalepKapatmaTarihi))
            {
                query = query.Where(x => x.TalepKapatmaTarihi.Value.Date == request.TalepKapatmaTarihi.Value.Date);
            }
            if (FluentValidationExtension.NotEmpty(request.OdemeTutari))
            {
                query = query.Where(x => x.OdemeTutari == request.OdemeTutari);
            }
            if (FluentValidationExtension.NotEmpty(request.HibeOdemeTutari))
            {
                query = query.Where(x => x.HibeOdemeTutari == request.HibeOdemeTutari);
            }
            if (FluentValidationExtension.NotEmpty(request.KrediOdemeTutari))
            {
                query = query.Where(x => x.KrediOdemeTutari == request.KrediOdemeTutari);
            }
            if (FluentValidationExtension.NotEmpty(request.DigerHibeOdemeTutari))
            {
                query = query.Where(x => x.DigerHibeOdemeTutari == request.DigerHibeOdemeTutari);
            }
            if (FluentValidationExtension.NotEmpty(request.OlusturmaTarihi))
            {
                query = query.Where(x => x.OlusturmaTarihi.Date == request.OlusturmaTarihi.Value.Date);
            }
            if (FluentValidationExtension.NotEmpty(request.BinaOdemeDurumId))
            {
                query = query.Where(x => x.BinaOdemeDurumId == request.BinaOdemeDurumId);
            }
            if (FluentValidationExtension.NotEmpty(request.Seviye))
            {
                query = query.Where(x => x.Seviye == request.Seviye);
            }

            return await query.OrderBy(x => x.BinaOdemeDurumId)
                                    .ThenBy(x => x.OlusturmaTarihi)
                                    .Select(x => new BinaOdemeDto
                                    {
                                        TalepNumarasi = x.TalepNumarasi,
                                        TalepDurumu = x.TalepDurumu,
                                        TalepKapatmaTarihi = x.TalepKapatmaTarihi,
                                        UavtIlNo = x.BinaDegerlendirme.UavtIlNo,
                                        UavtIlAdi = x.BinaDegerlendirme.UavtIlAdi,
                                        UavtIlceNo = x.BinaDegerlendirme.UavtIlceNo,
                                        UavtIlceAdi = x.BinaDegerlendirme.UavtIlceAdi,
                                        UavtMahalleNo = x.BinaDegerlendirme.UavtMahalleNo,
                                        UavtMahalleAdi = x.BinaDegerlendirme.UavtMahalleAdi,
                                        BinaOdemeId = x.BinaOdemeId,
                                        BinaDegerlendirmeId = x.BinaDegerlendirmeId,
                                        Seviye = x.Seviye,
                                        OdemeTutari = x.OdemeTutari,
                                        HibeOdemeTutari = x.HibeOdemeTutari,
                                        KrediOdemeTutari = x.KrediOdemeTutari,
                                        DigerHibeOdemeTutari = x.DigerHibeOdemeTutari,
                                        BinaOdemeDurumId = x.BinaOdemeDurumId,
                                        BinaOdemeDurumAd = x.BinaOdemeDurum.Ad,
                                        OlusturmaTarihi = x.OlusturmaTarihi,
                                        AktifMi = x.AktifMi,
                                        SilindiMi = x.SilindiMi,
                                        OdemeIslemleriButonGoster = x.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.HYSAktarildi,
                                        IlMudurluguneAktarButonGoster =x.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.Bekleniyor || x.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.IstekAlindi,
                                        HasarTespitAskiKodu= x.BinaDegerlendirme.HasarTespitAskiKodu
                                    })
                                    .Paginate(request);
        }
    }
}