using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeBinaOdemeServerSide;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.DataTable;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeOdemeTalepleriServerSide;

public class GetirListeOdemeTalepleriServerSideQuery : DataTableModel, IRequest<ResultModel<DataTableResponseModel<List<GetirListeOdemeTalepleriServerSideResponseModel>>>>
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

    public class GetirListeOdemeTalepleriServerSideQueryHandler : IRequestHandler<GetirListeOdemeTalepleriServerSideQuery, ResultModel<DataTableResponseModel<List<GetirListeOdemeTalepleriServerSideResponseModel>>>>
    {
        private readonly IBinaOdemeRepository _binaOdemeRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GetirListeOdemeTalepleriServerSideQueryHandler(IKullaniciBilgi kullaniciBilgi, IBinaDegerlendirmeRepository binaDegerlendirmeRepository, IBinaOdemeRepository binaOdemeRepository, IMapper mapper, IBasvuruRepository appealRepository, ITakbisService takbisService, IMediator mediator)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _binaOdemeRepository = binaOdemeRepository;
        }

        public async Task<ResultModel<DataTableResponseModel<List<GetirListeOdemeTalepleriServerSideResponseModel>>>> Handle(GetirListeOdemeTalepleriServerSideQuery request, CancellationToken cancellationToken)
        {
            var birimIlId = _kullaniciBilgi.GetUserInfo().BirimIlId;
            if(birimIlId > 0)
                request.UavtIlNo = birimIlId;

            var query = _binaOdemeRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                                && x.BinaDegerlendirme.BinaDegerlendirmeDurumId != (long)BinaDegerlendirmeDurumEnum.Reddedildi
                                                && (x.BinaDegerlendirme.BultenNo > 0 || x.BinaDegerlendirme.IzinBelgesiSayi > 0)
                                            //&& x.BinaDegerlendirme.Basvurus.All(y => y.SilindiMi == false && y.AktifMi == true
                                            //    && y.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.Kabul
                                            //    && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                            //    && y.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                            //    && !string.IsNullOrWhiteSpace(y.BasvuruImzaVerens.FirstOrDefault(z => !z.SilindiMi && z.AktifMi == true).BagimsizBolumNo))
                                            , true)
                    .Include(x => x.BinaDegerlendirme)
                        .ThenInclude(x => x.Basvurus.Where(y => y.SilindiMi == false && y.AktifMi == true))
                            .ThenInclude(x => x.BasvuruImzaVerens.Where(y => y.SilindiMi == false && y.AktifMi == true))
                    .Include(x => x.BinaDegerlendirme.BinaMuteahhits.Where(y => y.SilindiMi == false && y.AktifMi == true))
                    .Include(x => x.BinaOdemeDurum)
                    .Include(x => x.OlusturanKullanici)
                    .Include(x => x.GuncelleyenKullanici)
                        .OrderBy(x => x.BinaOdemeDurumId)
                    .ThenBy(x => x.OlusturmaTarihi)
                    .Select(x => new BinaOdemeDto
                    {
                        TalepNumarasi = x.TalepNumarasi,
                        GuncellemeTarihi = x.GuncellemeTarihi,
                        TalepDurumu = x.TalepDurumu,
                        TalepKapatmaTarihi = x.TalepKapatmaTarihi,
                        UavtIlNo = x.BinaDegerlendirme.UavtIlNo,
                        UavtIlAdi = x.BinaDegerlendirme.UavtIlAdi,
                        UavtIlceNo = x.BinaDegerlendirme.UavtIlceNo,
                        UavtIlceAdi = x.BinaDegerlendirme.UavtIlceAdi,
                        UavtMahalleNo = x.BinaDegerlendirme.UavtMahalleNo,
                        UavtMahalleAdi = x.BinaDegerlendirme.UavtMahalleAdi,
                        BinaDisKapiNo = x.BinaDegerlendirme.BinaDisKapiNo,
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
                        TapuAda = x.BinaDegerlendirme.Ada,
                        TapuParsel = x.BinaDegerlendirme.Parsel,
                        AktifMi = x.AktifMi,
                        SilindiMi = x.SilindiMi,
                        HasarTespitAskiKodu = x.BinaDegerlendirme.HasarTespitAskiKodu,
                        OdemeIslemleriButonGoster = x.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.Bekleniyor || x.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.IstekAlindi,
                        VergiKimlikNo = x.BinaDegerlendirme.BinaMuteahhits.Select(p => p.VergiKimlikNo).ToList(),
                        YetkiBelgeNo = x.BinaDegerlendirme.BinaMuteahhits.Select(p => p.YetkiBelgeNo).ToList(),
                        IbanNo = x.BinaDegerlendirme.BinaMuteahhits.Select(p => p.IbanNo).ToList(),
                        OlusturanKullaniciId = x.OlusturanKullanici.KullaniciId,
                        OlusturanKullaniciAdi = x.OlusturanKullanici.KullaniciAdi,
                        GuncelleyenKullaniciId = x.GuncelleyenKullanici.KullaniciId,
                        GuncelleyenKullaniciAdi = x.GuncelleyenKullanici.KullaniciAdi
                    });

            if (FluentValidationExtension.NotEmpty(request.BinaDegerlendirmeId))
                query = query.Where(x => x.BinaDegerlendirmeId == request.BinaDegerlendirmeId);

            if (FluentValidationExtension.NotEmpty(request.UavtIlNo))
                query = query.Where(x => x.UavtIlNo == request.UavtIlNo);

            if (FluentValidationExtension.NotEmpty(request.UavtIlceNo))
                query = query.Where(x => x.UavtIlceNo == request.UavtIlceNo);

            if (FluentValidationExtension.NotEmpty(request.UavtMahalleNo))
                query = query.Where(x => x.UavtMahalleNo == request.UavtMahalleNo);

            if (FluentValidationExtension.NotWhiteSpace(request.TalepNumarasi))
                query = query.Where(x => x.TalepNumarasi.Contains(request.TalepNumarasi));

            if (FluentValidationExtension.NotWhiteSpace(request.TalepDurumu))
                query = query.Where(x => x.TalepDurumu == request.TalepDurumu); 
            
            if (FluentValidationExtension.NotWhiteSpace(request.HasarTespitAskiKodu))
            {
                request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);
                query = query.Where(x => x.HasarTespitAskiKodu == request.HasarTespitAskiKodu);
            }
               
            if (FluentValidationExtension.NotEmpty(request.OdemeTutari))
                query = query.Where(x => x.OdemeTutari == request.OdemeTutari);

            if (FluentValidationExtension.NotEmpty(request.HibeOdemeTutari))
                query = query.Where(x => x.HibeOdemeTutari == request.HibeOdemeTutari);

            if (FluentValidationExtension.NotEmpty(request.KrediOdemeTutari))
                query = query.Where(x => x.KrediOdemeTutari == request.KrediOdemeTutari);

            if (FluentValidationExtension.NotEmpty(request.OlusturmaTarihi))
                query = query.Where(x => x.OlusturmaTarihi.StartOfDay() == request.OlusturmaTarihi.StartOfDay());

            if (FluentValidationExtension.NotEmpty(request.TalepKapatmaTarihi))
                query = query.Where(x => x.TalepKapatmaTarihi.StartOfDay() == request.TalepKapatmaTarihi.StartOfDay());          

            if (FluentValidationExtension.NotEmpty(request.BinaOdemeDurumId))
                query = query.Where(x => x.BinaOdemeDurumId == request.BinaOdemeDurumId);

            if (FluentValidationExtension.NotEmpty(request.Seviye))
                query = query.Where(x => x.Seviye == request.Seviye);

            if (FluentValidationExtension.NotEmpty(request.VergiKimlikNo))
                query = query.Where(x => x.VergiKimlikNo.Contains(request.VergiKimlikNo));

            if (FluentValidationExtension.NotEmpty(request.YetkiBelgeNo))
                query = query.Where(x => x.YetkiBelgeNo.Contains(request.YetkiBelgeNo));

            if (FluentValidationExtension.NotWhiteSpace(request.IbanNo))
                query = query.Where(x => x.IbanNo.Contains(request.IbanNo));

            if (FluentValidationExtension.NotEmpty(request.OlusturanKullaniciId))
                query = query.Where(x => x.OlusturanKullaniciId == request.OlusturanKullaniciId);

            if (FluentValidationExtension.NotEmpty(request.GuncelleyenKullaniciId))
                query = query.Where(x => x.GuncelleyenKullaniciId == request.GuncelleyenKullaniciId);

            var grouppedQuery = query.GroupBy(g => g.UavtMahalleNo)
                                    .Select(s => new GetirListeOdemeTalepleriServerSideResponseModel
                                    {
                                        OdemeIslemleriButonGoster = true,
                                        BinaOdemeId = s.FirstOrDefault()!.BinaOdemeId,
                                        BinaDegerlendirmeId = s.FirstOrDefault()!.BinaDegerlendirmeId,
                                        Seviye = s.FirstOrDefault()!.Seviye,
                                        OdemeTutari = s.FirstOrDefault()!.OdemeTutari,
                                        HibeOdemeTutari = s.FirstOrDefault()!.HibeOdemeTutari,
                                        KrediOdemeTutari = s.FirstOrDefault()!.KrediOdemeTutari,
                                        DigerHibeOdemeTutari = s.FirstOrDefault()!.DigerHibeOdemeTutari,
                                        BinaOdemeDurumId = s.FirstOrDefault()!.BinaOdemeDurumId,
                                        BinaOdemeDurumAd = s.FirstOrDefault()!.BinaOdemeDurumAd,
                                        OlusturmaTarihi = s.FirstOrDefault()!.OlusturmaTarihi,
                                        AktifMi = s.FirstOrDefault()!.AktifMi,
                                        SilindiMi = s.FirstOrDefault()!.SilindiMi,
                                        UavtIlNo = s.FirstOrDefault()!.UavtIlNo,
                                        UavtIlAdi = s.FirstOrDefault()!.UavtIlAdi,
                                        UavtIlceNo = s.FirstOrDefault()!.UavtIlceNo,
                                        UavtIlceAdi = s.FirstOrDefault()!.UavtIlceAdi,
                                        UavtMahalleNo = s.FirstOrDefault()!.UavtMahalleNo,
                                        UavtMahalleAdi = s.FirstOrDefault()!.UavtMahalleAdi,
                                        BinaDisKapiNo = s.FirstOrDefault()!.BinaDisKapiNo,
                                        TalepNumarasi = s.FirstOrDefault()!.TalepNumarasi,
                                        TalepDurumu = s.FirstOrDefault()!.TalepDurumu,
                                        TalepKapatmaTarihi = s.FirstOrDefault()!.TalepKapatmaTarihi,
                                        TapuAda = s.FirstOrDefault()!.TapuAda,
                                        TapuParsel = s.FirstOrDefault()!.TapuParsel,
                                        HasarTespitAskiKodu = s.FirstOrDefault()!.HasarTespitAskiKodu,
                                        BinaOdemeList = s.Where(x=> x.UavtMahalleNo == s.FirstOrDefault()!.UavtMahalleNo).OrderBy(x => x.BinaOdemeDurumId).ToList(),
                                        OlusturanKullaniciAdi = s.FirstOrDefault()!.OlusturanKullaniciAdi,
                                        GuncelleyenKullaniciAdi = s.FirstOrDefault()!.GuncelleyenKullaniciAdi

                                    })
                                    .OrderBy(x => x.BinaOdemeDurumId)
                                        .ThenBy(o => o.UavtIlNo)
                                            .ThenBy(o => o.UavtIlceAdi)
                                                .ThenBy(o => o.UavtMahalleAdi);

            var paginated = await grouppedQuery.Paginate(request);
            StringBuilder builder = new StringBuilder();
            foreach (var row in paginated.Result.data)
            {
                builder.Clear();

                var odemeDurumListesi = row.BinaOdemeList?.GroupBy(y => y.BinaOdemeDurumId).Select(s=> new { Count= s.Count(), Data= s.FirstOrDefault()! })?.ToList();
                if(odemeDurumListesi?.Any() == true)
                {
                    foreach (var odemeDurum in odemeDurumListesi)
                    {
                        if (odemeDurum.Data.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.Onaylandi)
                        {
                            builder.Append("<label class='badge badge-primary me-2'>");
                        }
                        else if (odemeDurum.Data.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.HYSAktarildi)
                        {
                            builder.Append("<label class='badge badge-light-warning me-2'>");
                        }
                        else if (odemeDurum.Data.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.MuteahhiteAktarildi)
                        {
                            builder.Append("<label class='badge badge-success me-2'>");
                        }
                        else if (odemeDurum.Data.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.Reddedildi)
                        {
                            builder.Append("<label class='badge badge-danger me-2'>");
                        }
                        else
                        {
                            builder.Append("<label class='badge badge-secondary me-2'>");
                        }

                        builder.Append(odemeDurum.Count);
                        builder.Append("</label>");
                    }
                    row.BinaOdemeDurumAd = builder.ToString();
                }    
                //odemeDurum.Data.BinaOdemeDurumId = ;
                //.Where(y => y.Count() > 1)?.MaxBy(y => y.Count())?.Key ?? row.BinaOdemeList?.MaxBy(x => x.GuncellemeTarihi)?.BinaOdemeDurumId;
                //row.BinaOdemeDurumAd = row.BinaOdemeList?.GroupBy(y => y.BinaOdemeDurumAd).Where(y => y.Count() > 1)?.MaxBy(y => y.Count())?.Key ?? row.BinaOdemeList?.MaxBy(x => x.GuncellemeTarihi)?.BinaOdemeDurumAd;
            }
            //foreach (var row in paginated.Result.data.Where(row => row.BinaOdemeList?.Count() > 1))
            //{
            //    odemeDurum.Data.BinaOdemeDurumId = row.BinaOdemeList?.GroupBy(y => y.BinaOdemeDurumId).Where(y => y.Count() > 1)?.MaxBy(y => y.Count())?.Key ?? row.BinaOdemeList?.MaxBy(x=> x.GuncellemeTarihi)?.BinaOdemeDurumId;
            //    row.BinaOdemeDurumAd = row.BinaOdemeList?.GroupBy(y => y.BinaOdemeDurumAd).Where(y => y.Count() > 1)?.MaxBy(y => y.Count())?.Key ?? row.BinaOdemeList?.MaxBy(x => x.GuncellemeTarihi)?.BinaOdemeDurumAd;                    
            //}

            return paginated;
        }
    }
}