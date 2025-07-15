using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeGruplanmamisOdemeServerSide;

public class GetirListeGruplanmamisOdemeServerSideQuery : DataTableModel, IRequest<ResultModel<DataTableResponseModel<List<BinaOdemeDto>>>>
{
    public long? BinaDegerlendirmeId { get; set; }
    public decimal? OdemeTutari { get; set; }
    public decimal? HibeOdemeTutari { get; set; }
    public decimal? KrediOdemeTutari { get; set; }
    public decimal? DigerHibeOdemeTutari { get; set; }
    public int? Seviye { get; set; }
    public DateTime? OlusturmaTarihi { get; set; }
    public long? BinaOdemeDurumId { get; set; }
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? BinaDisKapiNo { get; set; }
    public string? TalepNumarasi { get; set; }
    public string? TalepDurumu { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public DateTime? TalepKapatmaTarihi { get; set; }
    public string? VergiKimlikNo { get; set; }
    public string? YetkiBelgeNo { get; set; }
    public string? IbanNo { get; set; }
    public long? OlusturanKullaniciId { get; set; }
    public long? GuncelleyenKullaniciId { get; set; }


    public class GetirListeGruplanmamisOdemeQueryHandler : IRequestHandler<GetirListeGruplanmamisOdemeServerSideQuery, ResultModel<DataTableResponseModel<List<BinaOdemeDto>>>>
    {
        private readonly IMapper _mapper;
        private readonly IBinaOdemeRepository _binaOdemeRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GetirListeGruplanmamisOdemeQueryHandler(IKullaniciBilgi kullaniciBilgi, IBinaDegerlendirmeRepository binaDegerlendirmeRepository, IBinaOdemeRepository binaOdemeRepository, IMapper mapper, IBasvuruRepository appealRepository, ITakbisService takbisService, IMediator mediator)
        {
            _mapper = mapper;
            _kullaniciBilgi = kullaniciBilgi;
            _binaOdemeRepository = binaOdemeRepository;
        }

        public async Task<ResultModel<DataTableResponseModel<List<BinaOdemeDto>>>> Handle(GetirListeGruplanmamisOdemeServerSideQuery request, CancellationToken cancellationToken)
        {
            

            var query = _binaOdemeRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                        , true
                                        , i => i.BinaDegerlendirme.BinaMuteahhits.Where(o => !o.SilindiMi && o.AktifMi == true)
                                        , i => i.BinaOdemeDurum
                                        , i => i.OlusturanKullanici
                                        , i => i.GuncelleyenKullanici
                                        );

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
                query = query.Where(x => x.TalepKapatmaTarihi == request.TalepKapatmaTarihi);
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
                query = query.Where(x => x.OlusturmaTarihi == request.OlusturmaTarihi);
            }
            if (FluentValidationExtension.NotEmpty(request.BinaOdemeDurumId))
            {
                query = query.Where(x => x.BinaOdemeDurumId == request.BinaOdemeDurumId);
            }
            if (FluentValidationExtension.NotEmpty(request.Seviye))
            {
                query = query.Where(x => x.Seviye == request.Seviye);
            }

            if (FluentValidationExtension.NotEmpty(request.VergiKimlikNo))
                query = query.Where(x => x.BinaDegerlendirme.BinaMuteahhits.Select(p => p.VergiKimlikNo).Contains(request.VergiKimlikNo));

            if (FluentValidationExtension.NotEmpty(request.YetkiBelgeNo))
                query = query.Where(x => x.BinaDegerlendirme.BinaMuteahhits.Select(p => p.YetkiBelgeNo).Contains(request.YetkiBelgeNo));

            if (FluentValidationExtension.NotWhiteSpace(request.IbanNo))
            {
                query = query.Where(x => x.BinaDegerlendirme.BinaMuteahhits.Select(p => p.IbanNo).Contains(request.IbanNo));
            }

            if (FluentValidationExtension.NotEmpty(request.OlusturanKullaniciId))
            {
                query = query.Where(x => x.OlusturanKullaniciId == request.OlusturanKullaniciId);
            }

            if (FluentValidationExtension.NotEmpty(request.GuncelleyenKullaniciId))
            {
                query = query.Where(x => x.OlusturanKullaniciId == request.GuncelleyenKullaniciId);
            }

            return await query.OrderBy(x => x.BinaDegerlendirme.UavtIlNo)
                                    .ThenBy(o => o.BinaDegerlendirme.UavtIlceAdi)
                                    .ThenBy(o => o.BinaDegerlendirme.UavtMahalleAdi)
                                    .Paginate<BinaOdemeDto, BinaOdeme>(request, _mapper);
        }
    }
}