using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazByTakbisTasinmazId;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.Takbis;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListePasifMalikler;

public class GetirListePasifMaliklerQuery : IRequest<ResultModel<List<GetirListePasifMaliklerQueryResponseModel>>>
{
    public int? Take { get; set; }
    public bool? JobMuCalisiyor { get; set; } = true;
    public int? UavtMahalleNo { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }

    public class GetirListePasifMaliklerQueryHandler : IRequestHandler<GetirListePasifMaliklerQuery, ResultModel<List<GetirListePasifMaliklerQueryResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IBasvuruKamuUstlenecekRepository _basvuruKamuUstlenecekRepository;
        private readonly ITakbisService _takbisService;
        private readonly IMediator _mediator;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GetirListePasifMaliklerQueryHandler(IKullaniciBilgi kullaniciBilgi, IMapper mapper, IBasvuruRepository basvuruRepository, IBasvuruKamuUstlenecekRepository basvuruKamuUstlenecekRepository, ITakbisService takbisService, IMediator mediator)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _mapper = mapper;
            _basvuruRepository = basvuruRepository;
            _basvuruKamuUstlenecekRepository = basvuruKamuUstlenecekRepository;
            _takbisService = takbisService;
            _mediator = mediator;
        }

        public async Task<ResultModel<List<GetirListePasifMaliklerQueryResponseModel>>> Handle(GetirListePasifMaliklerQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<List<GetirListePasifMaliklerQueryResponseModel>>();

            request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            int birimIlId = kullaniciBilgi.BirimIlId;

            try
            {
                var basvuruQuery = _basvuruRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                                           && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                           && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                           && x.TapuMahalleId > 0
                                                           && x.TapuTasinmazId > 0
                                   , true);

                if (FluentValidationExtension.NotEmpty(request?.UavtMahalleNo))
                {
                    basvuruQuery = basvuruQuery.Where(x => request.UavtMahalleNo == x.UavtMahalleNo);
                }
                if (FluentValidationExtension.NotWhiteSpace(request?.HasarTespitAskiKodu))
                {
                    basvuruQuery = basvuruQuery.Where(x => request.HasarTespitAskiKodu == x.HasarTespitAskiKodu);
                }
                if (FluentValidationExtension.NotEmpty(request?.TapuAda))
                {
                    basvuruQuery = basvuruQuery.Where(x => request.TapuAda == x.TapuAda);
                }
                if (FluentValidationExtension.NotEmpty(request?.TapuParsel))
                {
                    basvuruQuery = basvuruQuery.Where(x => request.TapuParsel == x.TapuParsel);
                }
                var basvuruListe = basvuruQuery.GroupBy(x => new { x.UavtMahalleNo, x.HasarTespitAskiKodu })
                                                .Select(x => x.FirstOrDefault()!)
                                                .ToList();
                long bakilanSonBasvuruId = 0;
                List<GetirListePasifMaliklerQueryResponseModel> basvuruKamuUstlenecekListe = new();
                foreach (var basvuru in basvuruListe)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    bakilanSonBasvuruId = basvuru.BasvuruId;

                    var bagimsizBolumQuery = new GetirBagimsizBolumModel
                    {
                        AdaNo = basvuru.TapuAda,
                        ParselNo = basvuru.TapuParsel,
                        TapuBolumDurum = TapuBolumDurumEnum.Pasif,
                        MahalleIds = new decimal[] { basvuru.TapuMahalleId!.Value },
                        //Blok = s.TapuBlok
                    };
                    // binanin bulundugu ada, parsel, mahalleid bilgisine gore bagimsiz bolumler aliniyor.
                    var bagimsizBolumListe = await _takbisService.GetirBagimsizBolumAsync(bagimsizBolumQuery);

                    var datetimeNow = DateTime.Now;
                    //await Parallel.ForEachAsync(bagimsizBolumListe, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = 5 }, async (bagimsizBolum, cancellationToken) =>
                    foreach (var bagimsizBolum in bagimsizBolumListe)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        string? katStr = bagimsizBolum.BagimsizBolum?.Kat?.ToLower();
                        int kat = 0;
                        int.TryParse(katStr, out kat);
                        if (katStr == "bodrum") kat = -1;
                        else if (katStr == "zemin") kat = 0;



                        var hisseListe = await _takbisService.GetirHisseByTakbisTasinmazIdAsync(new GetirHisseTasinmazIdDenQueryModel
                        {
                            TakbisTasinmazId = (int)bagimsizBolum.Id,
                            TapuBolumDurum = TapuBolumDurumEnum.Pasif.ToString(),
                        });
                        //47- MALİYE HAZİNESİ Malik ID Değeri
                        //await Parallel.ForEachAsync(hisseListe, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = 100 }, async (hisse, cancellationToken) =>
                        foreach (var hisse in hisseListe)
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            if (!basvuruListe.Any(w => w.TcKimlikNo == hisse.MalikTCNo.ToString() && w.TapuTasinmazId == (int)hisse.TasinmazId))
                            {
                                bool gercekKisiMi = hisse.MalikTip == TakbisMalikTipEnum.GercekKisi;
                                if (hisse.MalikTip == TakbisMalikTipEnum.TuzelKisi)
                                {
                                    hisse.MalikAd = hisse.MalikUnvan;
                                    hisse.MalikSoyad = hisse.MalikUnvan;
                                }

                                var malikTcNo = hisse.MalikTCNo?.ToString() ?? "";
                                var basvuruKamuUstlenecekVeri = basvuruKamuUstlenecekListe.FirstOrDefault(x => x.TcKimlikNo == malikTcNo && x.TuzelKisiVergiNo == hisse.MalikVergiNo && x.TuzelKisiAdi == hisse.MalikUnvan && x.TapuAnaTasinmazId == (int)hisse.Id && (x.TapuTasinmazId == 0 || x.TapuTasinmazId == (int)hisse.TasinmazId) && ((!gercekKisiMi && x.TuzelKisiTipId != null) || (gercekKisiMi && x.TuzelKisiTipId == null)));
                                //basvuru ve kamuUstlenecek tablosunda yok ise eklenecek
                                if (basvuruKamuUstlenecekVeri == null)
                                {
                                    var basvuruKamuUstlenecek = new GetirListePasifMaliklerQueryResponseModel
                                    {
                                        BasvuruAfadDurumId = (long)BasvuruAfadDurumEnum.BasvuruYok,
                                        BasvuruDurumId = (long)BasvuruDurumEnum.BasvurunuzDegerlendirmeAsamasindadir,
                                        BasvuruDestekTurId = BasvuruDestekTurEnum.HibeVeKredi,
                                        BasvuruTurId = gercekKisiMi ? BasvuruTurEnum.Konut : BasvuruTurEnum.Ticarethane,
                                        TuzelKisiTipId = gercekKisiMi ? null : 1,
                                        AktifMi = true,
                                        SilindiMi = false,
                                        Ad = hisse.MalikAd,
                                        Soyad = hisse.MalikSoyad,
                                        TcKimlikNo = StringAddon.ToMaskedWord(malikTcNo, 3),
                                        TcKimlikNoRaw = malikTcNo,
                                        TuzelKisiVergiNo = hisse.MalikVergiNo,
                                        TuzelKisiAdi = hisse.MalikUnvan,
                                        TapuTasinmazId = (int)hisse.TasinmazId,
                                        TapuAnaTasinmazId = (int)hisse.Id,
                                        TapuAda = basvuru.TapuAda,
                                        TapuParsel = basvuru.TapuParsel,
                                        TapuArsaPay = HisseninArsaIcinPayiniHesapla(basvuru.TapuArsaPay, hisse.Pay, hisse.Payda),
                                        TapuArsaPayda = basvuru.TapuArsaPayda,
                                        TapuMahalleId = basvuru.TapuMahalleId,
                                        TapuMahalleAdi = bagimsizBolum.Mahalle,
                                        TapuKat = kat,
                                        TapuIlceAdi = bagimsizBolum.Ilce,
                                        TapuIlceId = basvuru.TapuIlceId,
                                        TapuIstirakNo = (int)hisse.IstirakNo,
                                        TapuBlok = bagimsizBolum.BagimsizBolum?.Blok,
                                        TapuTasinmazTipi = bagimsizBolum.Tip.ToString(),
                                        TapuBagimsizBolumNo = bagimsizBolum.BagimsizBolum?.No,
                                        TapuNitelik = bagimsizBolum.Nitelik,
                                        TapuIlAdi = bagimsizBolum.Il,
                                        TapuIlId = basvuru.TapuIlId,
                                        UavtMahalleAdi = basvuru.UavtMahalleAdi,
                                        UavtIlAdi = basvuru.UavtIlAdi,
                                        UavtIlNo = basvuru.UavtIlNo,
                                        UavtIlKodu = basvuru.UavtIlKodu,
                                        UavtIlceAdi = bagimsizBolum.Ilce,
                                        UavtIlceNo = basvuru.UavtIlceNo,
                                        UavtIlceKodu = basvuru.UavtIlceKodu,
                                        UavtMahalleNo = basvuru.UavtMahalleNo,
                                        UavtMahalleKodu = basvuru.UavtMahalleKodu,
                                        OlusturmaTarihi = DateTime.Now,
                                        OlusturanKullaniciId = 1
                                    };
                                    basvuruKamuUstlenecekListe.Add(basvuruKamuUstlenecek);
                                }
                            }
                        }
                        //);
                    }
                }
                result.Result = basvuruKamuUstlenecekListe;
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

            return await Task.FromResult(result);
        }
        private long? HisseninArsaIcinPayiniHesapla(long? arsaPay, string hissePay, string hissePayda)
        {
            if (arsaPay == null) return null;

            try
            {
                return (long)Math.Round((arsaPay ?? 0) * (double.Parse(hissePay.Replace(".000", "")) / double.Parse(hissePayda.Replace(".000", ""))), 3);
            }
            catch { }

            return null;
        }
    }
}