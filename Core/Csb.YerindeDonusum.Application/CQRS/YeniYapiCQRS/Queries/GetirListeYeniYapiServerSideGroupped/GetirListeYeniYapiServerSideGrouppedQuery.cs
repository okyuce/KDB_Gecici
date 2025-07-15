using AutoMapper;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Csb.YerindeDonusum.Application.Extensions.FluentValidationExtension;

namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiServerSideGroupped;

public class GetirListeYeniYapiServerSideGrouppedQuery : DataTableModel, IRequest<ResultModel<DataTableResponseModel<List<GetirListeYeniYapiServerSideGrouppedResponseModel>>>>
{
    public int? UavtIlNo { get; set; }
    public int? UavtIlceNo { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public int? TapuAda { get; set; }
    public int? TapuParsel { get; set; }
    public List<long>? BinaDegerlendirmeDurumId { get; set; }
    public string? VergiKimlikNo { get; set; }
    public string? YetkiBelgeNo { get; set; }
    public string? IbanNo { get; set; }
    public long? OlusturanKullaniciId { get; set; }
    public long? GuncelleyenKullaniciId { get; set; }
    public class GetirListeYeniYapiServerSideGrouppedQueryHandler : IRequestHandler<GetirListeYeniYapiServerSideGrouppedQuery, ResultModel<DataTableResponseModel<List<GetirListeYeniYapiServerSideGrouppedResponseModel>>>>
    {
        private readonly IMapper _mapper;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GetirListeYeniYapiServerSideGrouppedQueryHandler(IKullaniciBilgi kullaniciBilgi, IMapper mapper, IBinaDegerlendirmeRepository binaDegerlendirmeRepository, IKullaniciRepository kullaniciRepository)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _mapper = mapper;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _kullaniciRepository = kullaniciRepository;
        }

        public async Task<ResultModel<DataTableResponseModel<List<GetirListeYeniYapiServerSideGrouppedResponseModel>>>> Handle(GetirListeYeniYapiServerSideGrouppedQuery request, CancellationToken cancellationToken)
        {
            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();

            if (kullaniciBilgi.BirimIlId > 0)
                request.UavtIlNo = kullaniciBilgi.BirimIlId;

            request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);

            var binaDegerlendirmeQuery = _binaDegerlendirmeRepository.GetAllQueryable(x => x.AktifMi == true && x.SilindiMi == false
                                                                                        && x.BinaDegerlendirmeDurumId != (long)BinaDegerlendirmeDurumEnum.Reddedildi
                                            )
                .Include(i => i.Basvurus.Where(y => y.SilindiMi == false && y.AktifMi == true && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir))
                    .ThenInclude(i => i.BasvuruImzaVerens.Where(y => y.SilindiMi == false && y.AktifMi == true && y.BagimsizBolumAlani > 0))
                    .ThenInclude(i => i.BasvuruImzaVerenDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true))
                .Include(i => i.BasvuruKamuUstleneceks.Where(y => y.SilindiMi == false && y.AktifMi == true && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir))
                    .ThenInclude(i => i.BasvuruImzaVerens.Where(y => y.SilindiMi == false && y.AktifMi == true && y.BagimsizBolumAlani > 0))
                    .ThenInclude(i => i.BasvuruImzaVerenDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true))
                .Include(i => i.BinaDegerlendirmeDurum)
                .Include(i => i.BinaMuteahhits.Where(y => y.SilindiMi == false && y.AktifMi == true))
                .Include(i => i.BinaDegerlendirmeDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true))
                .Include(i => i.BinaYapiRuhsatIzinDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true))
                .Include(i => i.BinaYapiDenetimSeviyeTespits.Where(y=> y.SilindiMi == false && y.AktifMi == true).OrderByDescending(y=> y.IlerlemeYuzdesi))                
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
            
            if (NotWhiteSpace(request?.YetkiBelgeNo))
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => x.BinaMuteahhits.Select(p => p.YetkiBelgeNo).Contains(request.YetkiBelgeNo));
            
            if (NotWhiteSpace(request?.IbanNo))
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => x.BinaMuteahhits.Select(p => p.IbanNo).Contains(request.IbanNo));

            if (NotEmpty(request?.OlusturanKullaniciId))
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => request.OlusturanKullaniciId == x.OlusturanKullaniciId);

            if (NotEmpty(request?.GuncelleyenKullaniciId))

                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => request.GuncelleyenKullaniciId == x.GuncelleyenKullaniciId);

            var liss = binaDegerlendirmeQuery.ToList();
            var binaDegerlendirmeList = _mapper.Map<List<BinaDegerlendirmeDto>>(binaDegerlendirmeQuery.ToList());

            var olusturanKullaniciIds = liss.Select(p => p.OlusturanKullaniciId).Distinct().ToList();
            var guncelleyenKullaniciIds = liss.Where(p => p.GuncelleyenKullaniciId.HasValue).Select(p => p.GuncelleyenKullaniciId.Value).Distinct().ToList();
            olusturanKullaniciIds.AddRange(guncelleyenKullaniciIds);

            var kullanicilar = _kullaniciRepository.GetWhere(x => olusturanKullaniciIds.Contains(x.KullaniciId)).Distinct().ToList();

            var grouppedQuery = binaDegerlendirmeList.GroupBy(g => new { g.UavtMahalleNo, g.Ada, g.Parsel })
                                    .Select(s => new GetirListeYeniYapiServerSideGrouppedResponseModel
                                    {
                                        BultenNo = s.FirstOrDefault()!.BultenNo,
                                        YibfNo = s.FirstOrDefault()!.YibfNo,
                                        ImzalayanKisiSayisi = s.FirstOrDefault()!.ImzalayanKisiSayisi,
                                        YapiInsaatAlan = s.FirstOrDefault()!.YapiInsaatAlan,
                                        BagimsizBolumSayisi = s.FirstOrDefault()!.BagimsizBolumSayisi,
                                        KotAltKatSayisi = s.FirstOrDefault()!.KotAltKatSayisi,
                                        RuhsatOnayTarihi = s.FirstOrDefault()!.RuhsatOnayTarihi,
                                        KotUstKatSayisi = s.FirstOrDefault()!.KotUstKatSayisi,
                                        FenniMesulTc = s.FirstOrDefault()!.FenniMesulTc,
                                        IzinBelgesiTarih = s.FirstOrDefault()!.IzinBelgesiTarih,
                                        IzinBelgesiSayi = s.FirstOrDefault()!.IzinBelgesiSayi,
                                        BinaDegerlendirmeDosya = s.FirstOrDefault()!.BinaDegerlendirmeDosya,
                                        BinaYapiRuhsatIzinDosya = s.FirstOrDefault()!.BinaYapiRuhsatIzinDosya,
                                        BinaYapiDenetimSeviyeTespit = s.FirstOrDefault()!.BinaYapiDenetimSeviyeTespit,
                                        Muteahhit = s.FirstOrDefault()!.Muteahhit,
                                        BinaDegerlendirmeId = s.FirstOrDefault()!.BinaDegerlendirmeId,
                                        UavtIlAdi = s.FirstOrDefault()!.UavtIlAdi,
                                        UavtIlceAdi = s.FirstOrDefault()!.UavtIlceAdi,
                                        UavtMahalleAdi = s.FirstOrDefault()!.UavtMahalleAdi,
                                        UavtMahalleNo = s.FirstOrDefault()!.UavtMahalleNo,
                                        BinaDisKapiNo = s.FirstOrDefault()!.BinaDisKapiNo,
                                        YapiKimlikNo = s.FirstOrDefault()!.YapiKimlikNo,
                                        HasarTespitAskiKodu = s.FirstOrDefault()!.HasarTespitAskiKodu,
                                        Ada = s.FirstOrDefault()!.Ada,
                                        Parsel = s.FirstOrDefault()!.Parsel,
                                        ToplamKatSayisi = s.FirstOrDefault()!.ToplamKatSayisi,
                                        BasvuruDegerlendirmeDurumAd = s.FirstOrDefault()!.BasvuruDegerlendirmeDurumAd,
                                        BinaDegerlendirmeDurumId = s.FirstOrDefault()!.BinaDegerlendirmeDurumId,
                                        UavtIlNo = s.FirstOrDefault()!.UavtIlNo,
                                        UavtIlceNo = s.FirstOrDefault()!.UavtIlceNo,
                                        EskiTapuAda = s.FirstOrDefault()!.EskiTapuAda,
                                        EskiTapuParsel = s.FirstOrDefault()!.EskiTapuParsel,
                                        AdaParselGuncellemeTipiId = s.FirstOrDefault()!.AdaParselGuncellemeTipiId,
                                        AdaParselGuncellemeTipiAdi = s.FirstOrDefault()!.AdaParselGuncellemeTipiAdi,
                                        AdaParselGuncelleDosyaGuid = s.FirstOrDefault()!.AdaParselGuncelleDosyaGuid,
                                        TapuIlAdi = s.FirstOrDefault()!.TapuIlAdi,
                                        TapuIlceAdi = s.FirstOrDefault()!.TapuIlceAdi,
                                        TapuMahalleAdi = s.FirstOrDefault()!.TapuMahalleAdi,
                                        TapuIlNo = s.FirstOrDefault()!.TapuIlNo,
                                        TapuIlceNo = s.FirstOrDefault()!.TapuIlceNo,
                                        TapuMahalleNo = s.FirstOrDefault()!.TapuMahalleNo,
                                        YeniYapiList = s.Where(y => y.UavtMahalleNo == s.FirstOrDefault()!.UavtMahalleNo
                                                        //&& y.HasarTespitAskiKodu == s.FirstOrDefault()!.HasarTespitAskiKodu
                                                        && y.Ada == s.FirstOrDefault().Ada
                                                        && y.Parsel == s.FirstOrDefault().Parsel
                                                        ).ToList(),
                                         OlusturanKullaniciAdi = kullanicilar.Where(x => x.KullaniciId == s.FirstOrDefault().OlusturanKullaniciId).FirstOrDefault()?.KullaniciAdi,
                                         GuncelleyenKullaniciAdi = kullanicilar.Where(x => x.KullaniciId == s.FirstOrDefault().GuncelleyenKullaniciId).FirstOrDefault()?.KullaniciAdi,
                                    });

            grouppedQuery = grouppedQuery.OrderBy(o => o.UavtIlNo)
                                        .ThenBy(o => o.UavtIlceAdi)
                                        .ThenBy(o => o.UavtMahalleAdi);

            var paginateData = await grouppedQuery.Paginate(request);

            paginateData.Result.data = _mapper.Map<List<GetirListeYeniYapiServerSideGrouppedResponseModel>>(paginateData.Result.data);

            return paginateData;
        }
    }
}