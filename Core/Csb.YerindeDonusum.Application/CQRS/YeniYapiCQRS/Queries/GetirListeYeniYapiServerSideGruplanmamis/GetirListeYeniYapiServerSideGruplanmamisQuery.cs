using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruListeServerSide;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Csb.YerindeDonusum.Application.Extensions.FluentValidationExtension;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiServerSideGruplanmamis;

public class GetirListeYeniYapiServerSideGruplanmamisQuery : DataTableModel, IRequest<ResultModel<DataTableResponseModel<List<GetirListeYeniYapiServerSideGruplanmamisResponseModel>>>>
{
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public int? TapuAda { get; set; }
    public int? TapuParsel { get; set; }
    public List<long>? BinaDegerlendirmeDurumId { get; set; }
    public string? VergiKimlikNo { get; set; }
    public string? IbanNo { get; set; }
    public string? YetkiBelgeNo { get; set; }
    public long? OlusturanKullaniciId { get; set; }
    public long? GuncelleyenKullaniciId { get; set; }

    public class GetirListeYeniYapiServerSideGruplanmamisQueryHandler : IRequestHandler<GetirListeYeniYapiServerSideGruplanmamisQuery, ResultModel<DataTableResponseModel<List<GetirListeYeniYapiServerSideGruplanmamisResponseModel>>>>
    {
        private readonly IMapper _mapper;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GetirListeYeniYapiServerSideGruplanmamisQueryHandler(IKullaniciBilgi kullaniciBilgi, IMapper mapper, IBinaDegerlendirmeRepository binaDegerlendirmeRepository, IKullaniciRepository kullaniciRepository)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _mapper = mapper;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _kullaniciRepository = kullaniciRepository;
        }

        public async Task<ResultModel<DataTableResponseModel<List<GetirListeYeniYapiServerSideGruplanmamisResponseModel>>>> Handle(GetirListeYeniYapiServerSideGruplanmamisQuery request, CancellationToken cancellationToken)
        {
            request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);

            var binaDegerlendirmeQuery = _binaDegerlendirmeRepository.GetAllQueryable(x => x.AktifMi == true && x.SilindiMi == false
                                                                                        && x.BinaDegerlendirmeDurumId != (long)BinaDegerlendirmeDurumEnum.Reddedildi
                                            ).Include(i => i.Basvurus.Where(y => y.SilindiMi == false && y.AktifMi == true && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir))
                                                .ThenInclude(i => i.BasvuruImzaVerens.Where(y => y.SilindiMi == false && y.AktifMi == true && y.BagimsizBolumAlani > 0))
                                                    .ThenInclude(i => i.BasvuruImzaVerenDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true))
                                            .Include(i => i.BasvuruKamuUstleneceks.Where(y => y.SilindiMi == false && y.AktifMi == true && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir))
                                                .ThenInclude(i => i.BasvuruImzaVerens.Where(y => y.SilindiMi == false && y.AktifMi == true && y.BagimsizBolumAlani > 0))
                                                    .ThenInclude(i => i.BasvuruImzaVerenDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true))
                                            .Include(i => i.BinaDegerlendirmeDurum)
                                            .Include(i => i.BinaMuteahhits.Where(y => y.SilindiMi == false && y.AktifMi == true))
                                            .Include(i => i.BinaDegerlendirmeDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true))
                                            .Include(i => i.BinaYapiRuhsatIzinDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true))
                                            .AsQueryable();

            if (NotEmpty(request?.UavtIlNo))
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => request.UavtIlNo == x.UavtIlNo);

            if (NotEmpty(request?.UavtIlceNo))
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => request.UavtIlceNo == x.UavtIlceNo);

            if (NotEmpty(request?.UavtMahalleNo))
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => request.UavtMahalleNo == x.UavtMahalleNo);

            if (NotWhiteSpace(request?.HasarTespitAskiKodu))
            {
                request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => request.HasarTespitAskiKodu == x.HasarTespitAskiKodu);
            }

            if (NotEmpty(request?.BinaDegerlendirmeDurumId))
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => request.BinaDegerlendirmeDurumId.Any(y => y == x.BinaDegerlendirmeDurumId));

            if (NotEmpty(request?.TapuAda))
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => x.Ada == request.TapuAda.ToString());

            if (NotEmpty(request?.TapuParsel))
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => x.Parsel == request.TapuParsel.ToString());

            if (NotWhiteSpace(request?.VergiKimlikNo))
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => x.BinaMuteahhits.Select(p => p.VergiKimlikNo).Contains(request.VergiKimlikNo));

            if(NotWhiteSpace(request?.IbanNo))
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => x.BinaMuteahhits.Select(p => p.IbanNo).Contains(request.IbanNo));

            if (NotWhiteSpace(request?.YetkiBelgeNo))
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => x.BinaMuteahhits.Select(p => p.YetkiBelgeNo).Contains(request.YetkiBelgeNo));

            if (NotEmpty(request?.OlusturanKullaniciId))
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => request.OlusturanKullaniciId == x.OlusturanKullaniciId);

            if (NotEmpty(request?.GuncelleyenKullaniciId))

                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => request.GuncelleyenKullaniciId == x.GuncelleyenKullaniciId);

            var paginateData = await binaDegerlendirmeQuery.OrderBy(o => o.UavtIlNo)
                                               .ThenBy(o => o.UavtIlceAdi)
                                               .ThenBy(o => o.UavtMahalleAdi)
                                               .Paginate<GetirListeYeniYapiServerSideGruplanmamisResponseModel, BinaDegerlendirme>(request, _mapper);

            var olusturanKullaniciIds = paginateData.Result.data.Select(p => p.OlusturanKullaniciId).Distinct().ToList();
            var guncelleyenKullaniciIds = paginateData.Result.data.Where(p => p.GuncelleyenKullaniciId.HasValue).Select(p => p.GuncelleyenKullaniciId.Value).Distinct().ToList();
            olusturanKullaniciIds.AddRange(guncelleyenKullaniciIds);
            var kullanicilar = _kullaniciRepository.GetWhere(x => olusturanKullaniciIds.Contains(x.KullaniciId)).Distinct().ToList();

            paginateData.Result.data.ForEach(x =>
            {
                x.OlusturanKullaniciAdi = kullanicilar.Where(p => p.KullaniciId == x.OlusturanKullaniciId).FirstOrDefault()?.KullaniciAdi;
                x.GuncelleyenKullaniciAdi = kullanicilar.Where(p => p.KullaniciId == x.GuncelleyenKullaniciId).FirstOrDefault()?.KullaniciAdi;
            });

            return paginateData;
        }
    }
}