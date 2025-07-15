using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeMalikler;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazByTakbisTasinmazId;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.Takbis;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirListeTebligatMalikler;

public class GetirListeTebligatMaliklerQuery : IRequest<ResultModel<List<GetirListeTebligatMaliklerQueryResponseModel>>>
{
    public int? Take { get; set; }
    public bool? JobMuCalisiyor { get; set; } = true;
    public int? UavtMahalleNo { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }

    public class GetirListeTebligatMaliklerQueryHandler : IRequestHandler<GetirListeTebligatMaliklerQuery, ResultModel<List<GetirListeTebligatMaliklerQueryResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IBasvuruKamuUstlenecekRepository _basvuruKamuUstlenecekRepository;
        private readonly ITebligatGonderimDetayRepository _tebligatGonderimDetayRepository;
        private readonly ITebligatGonderimRepository _tebligatGonderimRepository;
        private readonly ITakbisService _takbisService;
        private readonly IMediator _mediator;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GetirListeTebligatMaliklerQueryHandler(IKullaniciBilgi kullaniciBilgi, IMapper mapper, IBasvuruRepository basvuruRepository, ITebligatGonderimDetayRepository tebligatGonderimDetayRepository, ITebligatGonderimRepository tebligatGonderimRepository, IBasvuruKamuUstlenecekRepository basvuruKamuUstlenecekRepository, ITakbisService takbisService, IMediator mediator)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _mapper = mapper;
            _basvuruRepository = basvuruRepository;
            _basvuruKamuUstlenecekRepository = basvuruKamuUstlenecekRepository;
            _tebligatGonderimDetayRepository = tebligatGonderimDetayRepository;
            _tebligatGonderimRepository = tebligatGonderimRepository;
            _takbisService = takbisService;
            _mediator = mediator;
        }

        public async Task<ResultModel<List<GetirListeTebligatMaliklerQueryResponseModel>>> Handle(GetirListeTebligatMaliklerQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<List<GetirListeTebligatMaliklerQueryResponseModel>>();

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
                List<GetirListeTebligatMaliklerQueryResponseModel> basvuruKamuUstlenecekListe = new();
                foreach (var basvuru in basvuruListe)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    bakilanSonBasvuruId = basvuru.BasvuruId;
                    string? hasarTespitHasarDurumu = !string.IsNullOrEmpty(basvuru.HasarTespitGuclendirmeMahkemeSonucu) ?
                        basvuru.HasarTespitGuclendirmeMahkemeSonucu :
                            (!string.IsNullOrEmpty(basvuru.HasarTespitItirazSonucu) && basvuru.HasarTespitItirazSonucu != "Hasara İtiraz Yoktur") ?
                                basvuru.HasarTespitItirazSonucu :
                                basvuru.HasarTespitHasarDurumu;

                        var malikList = (await _mediator.Send(new GetirListeMaliklerQuery()
                        {
                            TapuAda = basvuru.TapuAda,
                            TapuParsel = basvuru.TapuParsel,
                            UavtMahalleNo = basvuru.UavtMahalleNo,
                            HasarTespitAskiKodu = request.HasarTespitAskiKodu,
                            IptalEdilenlerAlinmasin = true
                        })).Result;

                        //Malik listesi getirilirken kamu üstlenecek tablosundan ada parsel eşleşmeyen (mahalleno eşleşen) kayıtlar filtrelenmiştir
                        malikList = malikList.Where(x => x.HasarTespitAskiKodu == request.HasarTespitAskiKodu || (x.TapuAda == basvuru.TapuAda && x.TapuParsel == basvuru.TapuParsel)).ToList();

                        foreach (var item in malikList)
                        {
                            long? istirakTebligatDetayGonderimId = null;
                            long? teslimTebligatDetayGonderimId = null;
                            
                            var tebligatGonderimDetayList = _tebligatGonderimDetayRepository
                                .GetWhere(x =>
                                x.TcKimlikNo == item.TcKimlikNoRaw &&
                                x.TapuIlAdi == basvuru.TapuIlAdi &&
                                x.TapuIlceAdi == basvuru.TapuIlceAdi &&
                                x.TapuMahalleAdi == basvuru.TapuMahalleAdi &&
                                x.HasarTespitAskiKodu == request.HasarTespitAskiKodu &&
                                x.TebligatGonderim.GonderimBasariliMi == true).ToList();

                            var istirakTebligatIds = tebligatGonderimDetayList.Where(x=> x.TebligatTipiId == EDevletTebligatTipiEnum.IstirakKampanyaId.GetHashCode()).Select(x => x.TebligatGonderimDetayId).Distinct();

                            var teslimTebligatIds = tebligatGonderimDetayList.Where(x => x.TebligatTipiId == EDevletTebligatTipiEnum.AnahtarTeslimKampanyaId.GetHashCode()).Select(x => x.TebligatGonderimDetayId).Distinct();

                            if (istirakTebligatIds.Count() > 0 && istirakTebligatIds.FirstOrDefault() > 0)
                            {
                                istirakTebligatDetayGonderimId = istirakTebligatIds.FirstOrDefault();
                            }
                            if (teslimTebligatIds.Count() > 0 && teslimTebligatIds.FirstOrDefault() > 0)
                            {
                                teslimTebligatDetayGonderimId = teslimTebligatIds.FirstOrDefault();
                            }

                            bool gercekKisiMi = item.TuzelKisiVergiNo == null;

                            var basvuruKamuUstlenecek = new GetirListeTebligatMaliklerQueryResponseModel
                            {
                                TcKimlikNo = item.TcKimlikNo,
                                TcKimlikNoRaw = item.TcKimlikNoRaw,
                                BasvuruDurumId = item.BasvuruDurumId,
                                BasvuruDurumAd = item.BasvuruDurumAd,
                                TuzelKisiTipId = gercekKisiMi ? null : 1,
                                Ad = item.Ad,
                                Soyad = item.Soyad,
                                TuzelKisiVergiNo = item.TuzelKisiVergiNo,
                                TuzelKisiAdi = item.TuzelKisiAdi,
                                TapuTasinmazId = item.TapuTasinmazId,
                                TapuAnaTasinmazId = basvuru.TapuAnaTasinmazId,
                                TapuAda = item.TapuAda,
                                TapuParsel = item.TapuParsel,
                                TapuArsaPay = HisseninArsaIcinPayiniHesapla(item.TapuArsaPay, item.HissePay.ToString(), item.HissePayda.ToString()),
                                TapuArsaPayda = item.TapuArsaPayda,
                                TapuMahalleId = basvuru.TapuMahalleId,
                                TapuMahalleAdi = basvuru.TapuMahalleAdi,
                                TapuKat = basvuru.TapuKat,
                                TapuIlceAdi = basvuru.TapuIlceAdi,
                                TapuIlceId = basvuru.TapuIlceId,
                                TapuIstirakNo = basvuru.TapuIstirakNo,
                                TapuBlok = item.TapuBlok,
                                TapuTasinmazTipi = basvuru.TapuTasinmazTipi,
                                TapuBagimsizBolumNo = item.BagimsizBolumNo,
                                TapuNitelik = basvuru.TapuNitelik,
                                TapuIlAdi = basvuru.TapuIlAdi,
                                TapuIlId = basvuru.TapuIlId,
                                AskiKodu = request.HasarTespitAskiKodu,
                                HasarTespitHasarDurumu = hasarTespitHasarDurumu,
                                BasvuruId = item.BasvuruId,
                                BasvuruKamuUstlenecekGuid = item.BasvuruKamuUstlenecekGuid,
                                BasvuruKamuUstlenecekId = item.BasvuruKamuUstlenecekId,
                                BinaDegerlendirmeId = item.BinaDegerlendirmeId,
                                CepTelefonu = item.CepTelefonu,
                                Eposta = item.Eposta,
                                TapuGirisBilgisi = basvuru.TapuGirisBilgisi,
                                TapuRehinDurumu = basvuru.TapuRehinDurumu,
                                TuzelKisiAdres = basvuru.TuzelKisiAdres,
                                TuzelKisiMersisNo = item.TuzelKisiMersisNo,
                                TuzelKisiYetkiTuru = basvuru.TuzelKisiYetkiTuru,
                                PasifMalikMi = false,
                                IstirakTebligatGonderimDetayId = istirakTebligatDetayGonderimId,
                                TeslimTebligatGonderimDetayId = teslimTebligatDetayGonderimId
                            };
                            basvuruKamuUstlenecekListe.Add(basvuruKamuUstlenecek);
                        }
                  

                    var bagimsizBolumQuery = new GetirBagimsizBolumModel
                    {
                        AdaNo = basvuru.TapuAda,
                        ParselNo = basvuru.TapuParsel,
                        TapuBolumDurum = TapuBolumDurumEnum.Pasif,
                        MahalleIds = new decimal[] { basvuru.TapuMahalleId!.Value }
                    };
                    // binanin bulundugu ada, parsel, mahalleid bilgisine gore bagimsiz bolumler aliniyor.
                    var bagimsizBolumListe = await _takbisService.GetirBagimsizBolumAsync(bagimsizBolumQuery);

                    var datetimeNow = DateTime.Now;
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
                                var basvuruKamuUstlenecekVeri = basvuruKamuUstlenecekListe
                                    .FirstOrDefault(x => 
                                        x.TcKimlikNoRaw == malikTcNo && 
                                        x.TuzelKisiVergiNo == hisse.MalikVergiNo && 
                                        x.TuzelKisiAdi == hisse.MalikUnvan && 
                                        x.TapuAnaTasinmazId == (int)hisse.Id && 
                                        (x.TapuTasinmazId == 0 || x.TapuTasinmazId == (int)hisse.TasinmazId) && 
                                        ((!gercekKisiMi && x.TuzelKisiTipId != null) || (gercekKisiMi && x.TuzelKisiTipId == null)));
                                //basvuru ve kamuUstlenecek tablosunda yok ise eklenecek
                                if (basvuruKamuUstlenecekVeri == null)
                                {
                                    long? istirakTebligatDetayGonderimId = null;
                                    long? teslimTebligatDetayGonderimId = null;

                                    var tebligatGonderimDetayList = _tebligatGonderimDetayRepository
                                        .GetWhere(x =>
                                            x.TcKimlikNo == malikTcNo &&
                                            x.TapuIlAdi == bagimsizBolum.Il &&
                                            x.TapuIlceAdi == bagimsizBolum.Ilce &&
                                            x.TapuMahalleAdi == bagimsizBolum.Mahalle &&
                                            x.HasarTespitAskiKodu == request.HasarTespitAskiKodu &&
                                            x.TebligatGonderim.GonderimBasariliMi == true).ToList();

                                    var istirakTebligatIds = tebligatGonderimDetayList.Where(x => x.TebligatTipiId == EDevletTebligatTipiEnum.IstirakKampanyaId.GetHashCode()).Select(x => x.TebligatGonderimDetayId).Distinct();

                                    var teslimTebligatIds = tebligatGonderimDetayList.Where(x => x.TebligatTipiId == EDevletTebligatTipiEnum.AnahtarTeslimKampanyaId.GetHashCode()).Select(x => x.TebligatGonderimDetayId).Distinct();

                                    if (istirakTebligatIds.Count() > 0 && istirakTebligatIds.FirstOrDefault() > 0)
                                    {
                                        istirakTebligatDetayGonderimId = istirakTebligatIds.FirstOrDefault();
                                    }
                                    if (teslimTebligatIds.Count() > 0 && teslimTebligatIds.FirstOrDefault() > 0)
                                    {
                                        teslimTebligatDetayGonderimId = teslimTebligatIds.FirstOrDefault();
                                    }
                                    var basvuruKamuUstlenecek = new GetirListeTebligatMaliklerQueryResponseModel
                                    {
                                        BasvuruDurumId = (long)BasvuruDurumEnum.BasvurunuzDegerlendirmeAsamasindadir,
                                        BasvuruDurumAd = BasvuruDurumEnum.BasvurunuzDegerlendirmeAsamasindadir.GetDisplayName(),
                                        TuzelKisiTipId = gercekKisiMi ? null : 1,
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
                                        BinaDegerlendirmeId = basvuru.BinaDegerlendirmeId,
                                        AskiKodu = request.HasarTespitAskiKodu,
                                        HasarTespitHasarDurumu = hasarTespitHasarDurumu,
                                        PasifMalikMi = true,
                                        IstirakTebligatGonderimDetayId = istirakTebligatDetayGonderimId,
                                        TeslimTebligatGonderimDetayId = teslimTebligatDetayGonderimId
                                    };
                                    basvuruKamuUstlenecekListe.Add(basvuruKamuUstlenecek);
                                }
                            }
                        }
                    }                
                }
                result.Result = basvuruKamuUstlenecekListe;
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