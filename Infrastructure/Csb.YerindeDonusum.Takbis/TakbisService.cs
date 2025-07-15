using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirAlanByTakbisTasinmazIdQuery;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeAnaTasinmaz;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazByTakbisTasinmazId;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models.Takbis;
using Csb.YerindeDonusum.Takbis.Models;
using CSB.Core.LogHandler.Abstraction;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.ServiceModel;
using Takbis;

namespace Csb.YerindeDonusum.Takbis;

public class TakbisService : ITakbisService
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly TakbisAuthenticationOptionModel? _authDto;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ICacheService _cacheService;
    private readonly IIntegrationLogService _logService;

    public TakbisService(IConfiguration configuration, IMapper mapper, IWebHostEnvironment webHostEnvironment, ICacheService cacheService, IIntegrationLogService logService)
    {
        _configuration = configuration;
        _mapper = mapper;
        _authDto = _configuration.GetSection("TakbisAuth").Get<TakbisAuthenticationOptionModel>();
        _webHostEnvironment = webHostEnvironment;
        _cacheService = cacheService;
        _logService = logService;
    }

    public async Task<List<AlanModel>> GetirListeAlanByTakbisTasinmazIdAsync(GetirAlanByTakbisTasinmazIdQueryModel request)
    {
        List<AlanModel> result = new();

        using var service = Initialize();

        if (request is not null)
        {
            var liste = await _logService.WrapServiceAsnyc<GetirAlanByTakbisTasinmazIdQueryModel, Alan[]>(request, async () => { return await service.GetirAlanTasinmazIDDenAsync(request.TakbisTasinmazId, (TapuBolumDurum)request.TapuBolumDurum); });
            result = liste.Select(q => new AlanModel
            {
                Id = q.ID,
                TapuBolumDurum = (TapuBolumDurumEnum)q.TapuBolumDurum,
                TerkinIslem = new IslemModel
                {
                    Id = q.TerkinIslem.ID,
                    IslemDurum = (IslemDurumEnum)q.TerkinIslem.IslemDurum,
                    BaslamaSekilAd = q.TerkinIslem.BaslamaSekilAd,
                    IslemTanimAd = q.TerkinIslem.IslemTanimAd,
                    IptalAciklama = q.TerkinIslem.IptalAciklama
                },
                TesisIslem = new IslemModel
                {
                    Id = q.TesisIslem.ID,
                    IslemDurum = (IslemDurumEnum)q.TesisIslem.IslemDurum,
                    BaslamaSekilAd = q.TesisIslem.BaslamaSekilAd,
                    IptalAciklama = q.TesisIslem.IptalAciklama,
                    IslemTanimAd = q.TesisIslem.IslemTanimAd
                },
                YuzOlcum = q.Yuzolcum
            }).ToList();
        }

        return result;
    }


    // Todo: Test edilecek.
    public List<AnaTasinmazModel> GetirAnaTasinmaz(GetirAnaTasinmazQueryModel request)
    {
        List<AnaTasinmazModel> result = new();

        using var service = Initialize();

        #region ...: Old Code :...
        //result = service.GetirAnaTasinmaz(request.MahalleIds, request.AdaNo, request.ParselNo, (TapuBolumDurum)request.TapuBolumDurum)
        //    .Select(q => new AnaTasinmazModel
        //    {
        //        AltTasinmazId = q.AltTasinmazID,
        //        Ada = q.Ada,
        //        BagimsizBolum = new BagimsizBolumModel
        //        {
        //            ArsaPay = q.BagimsizBolum.ArsaPay,
        //            ArsaPayda = q.BagimsizBolum.ArsaPayda,
        //            Blok = q.BagimsizBolum.Blok,
        //            Giris = q.BagimsizBolum.Giris,
        //            Id = q.BagimsizBolum.ID,
        //            Kat = q.BagimsizBolum.Kat,
        //            No = q.BagimsizBolum.No,
        //            Tip = (BagimsizBolumTipEnum)q.BagimsizBolum.Tip,
        //        },
        //        CiltNo = q.CiltNo,
        //        Tip = (TasinmazTipEnum)q.Tip,
        //        Id = q.ID,
        //        DaimiMustakilHak = new DaimiMustakilHakModel
        //        {
        //            Id = q.DaimiMustakilHak.ID,
        //            BaslamaTarihi = q.DaimiMustakilHak.BaslamaTarihi,
        //            BitisTarihi = q.DaimiMustakilHak.BitisTarihi,
        //            Cumle = q.DaimiMustakilHak.Cumle,
        //            SureAciklama = q.DaimiMustakilHak.SureAciklama,
        //            TesisBicim = q.DaimiMustakilHak.TesisBicim
        //        },
        //        Pafta = q.Pafta,
        //        Eklenti = q.Eklenti,
        //        Il = q.Il,
        //        Ilce = q.Ilce,
        //        Kurum = q.Kurum,
        //        Mahalle = q.Mahalle,
        //        Mevkii = q.Mevkii,
        //        Muhdesat = q.Muhdesat,
        //        Nitelik = q.Nitelik,
        //        Parsel = q.Parsel,
        //        SayfaNo = q.SayfaNo,
        //        TapuBolumDurum = (TapuBolumDurumEnum)q.TapuBolumDurum,
        //        Teferruat = q.Teferruat,
        //        TerkinIslem = new IslemModel
        //        {
        //            Id = q.TerkinIslem.ID,
        //            IslemDurum = (IslemDurumEnum)q.TerkinIslem.IslemDurum,
        //            BaslamaSekilAd = q.TerkinIslem.BaslamaSekilAd,
        //            IslemTanimAd = q.TerkinIslem.IslemTanimAd,
        //            IptalAciklama = q.TerkinIslem.IptalAciklama
        //        },
        //        TesisIslem = new IslemModel
        //        {
        //            Id = q.TesisIslem.ID,
        //            IslemDurum = (IslemDurumEnum)q.TesisIslem.IslemDurum,
        //            BaslamaSekilAd = q.TesisIslem.BaslamaSekilAd,
        //            IptalAciklama = q.TesisIslem.IptalAciklama,
        //            IslemTanimAd = q.TesisIslem.IslemTanimAd
        //        },
        //        YuzOlcum = q.Yuzolcum
        //    }).ToList();
        #endregion

        // Todo: mahalle id bilgileri sonrasi deneme yapilarak acilacaktir

        var sonuc = _logService.WrapService<GetirAnaTasinmazQueryModel, Tasinmaz[]>(request, () => { return service.GetirAnaTasinmaz(request.MahalleIds, request.AdaNo, request.ParselNo, (TapuBolumDurum)request.TapuBolumDurum); });
        result = _mapper.Map<List<AnaTasinmazModel>>(sonuc.ToList());

        return result;
    }

    public async Task<List<TasinmazModel>> GetirBagimsizBolumAsync(GetirBagimsizBolumModel request)
    {
        List<TasinmazModel> result = new();

        var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirBagimsizBolumAsync)}_{JsonConvert.SerializeObject(request)}";
        var cache = await _cacheService.GetValueAsync(cacheKey);
        if (cache != null)
            return JsonConvert.DeserializeObject<List<TasinmazModel>>(cache);

        using var service = Initialize();
        var sonuc = _logService.WrapService<GetirBagimsizBolumModel, Tasinmaz[]>(request, () => { return service.GetirBagimsizBolum(request.MahalleIds, request.AdaNo ?? "", request.ParselNo ?? "", (TapuBolumDurum)request.TapuBolumDurum, request.Bbno ?? "", request.Kat ?? "", request.Blok ?? "", request.Giris ?? ""); });
        result = _mapper.Map<List<TasinmazModel>>(sonuc.ToList());

        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromDays(1));

        return result;
    }

    public async Task<List<IlModel>> GetirListeTakbisIlAsnyc()
    {
        List<IlModel> result = new();
        var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirListeTakbisIlAsnyc)}";
        var r = await _cacheService.GetValueAsync(cacheKey);
        if (r != null)
        {
            return JsonConvert.DeserializeObject<List<IlModel>>(r);
        }

        using var service = Initialize();
        var sonuc = _logService.WrapService<object?, Il[]>(null, () => { return service.GetirIlTum(); });
        result = sonuc
            .Select(q => new IlModel
            {
                Ad = q.Ad,
                Id = q.ID
            })
            .OrderBy(o => o.Ad)
            .ToList();
        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromDays(30));
        return result;
    }

    public async Task<List<IlModel>> GetirListeTakbisDepremIlAsnyc()
    {
        List<IlModel> result = new();
        var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirListeTakbisDepremIlAsnyc)}";
        var r = await _cacheService.GetValueAsync(cacheKey);
        if (r != null)
        {
            return JsonConvert.DeserializeObject<List<IlModel>>(r);
        }

        List<long> sorgulanabilirIlIdListesi = new List<long>() {
            TakbisIlIdEnum.Adana,
            TakbisIlIdEnum.Adiyaman,
            TakbisIlIdEnum.Diyarbakir,
            TakbisIlIdEnum.Elazig,
            TakbisIlIdEnum.Gaziantep,
            TakbisIlIdEnum.Hatay,
            TakbisIlIdEnum.Malatya,
            TakbisIlIdEnum.Kahramanmaras,
            TakbisIlIdEnum.Sanliurfa,
            TakbisIlIdEnum.Kilis,
            TakbisIlIdEnum.Osmaniye,
            TakbisIlIdEnum.Batman,
            TakbisIlIdEnum.Kayseri,
            TakbisIlIdEnum.Mardin,
            TakbisIlIdEnum.Nigde,
            TakbisIlIdEnum.Sivas,
            TakbisIlIdEnum.Tunceli,
            TakbisIlIdEnum.Bingol
        };

        using var service = Initialize();
        var sonuc = _logService.WrapService<object?, Il[]>(null, () => { return service.GetirIlTum(); });
        result = sonuc
            .Where(x => sorgulanabilirIlIdListesi.Contains((long)x.ID))
            .Select(q => new IlModel
            {
                Ad = q.Ad,
                Id = q.ID
            })
            .OrderBy(o => o.Ad)
            .ToList();
        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromDays(30));
        return result;
    }

    public async Task<List<IlceModel>> GetirListeTakbisIlceByTakbisIlIdAsync(int takbisIlId)
    {
        var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirListeTakbisIlceByTakbisIlIdAsync)}_{takbisIlId}";
        var r = await _cacheService.GetValueAsync(cacheKey);
        if (r != null)
        {
            return JsonConvert.DeserializeObject<List<IlceModel>>(r);
        }

        using var service = Initialize();
        var sonuc = await _logService.WrapServiceAsnyc<int, Ilce[]>(takbisIlId, async () => { return await service.GetirIlceIlIDDenAsync(takbisIlId); });
        List<IlceModel> result = _mapper.Map<List<IlceModel>>(sonuc);

        result = result.OrderBy(o => o.Ad).ToList();

        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromDays(30));

        return result;
    }

    public async Task<List<MahalleModel>> GetirListeTakbisMahalleByTakbisIlceIdAsync(int takbisIlceId)
    {
        var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirListeTakbisMahalleByTakbisIlceIdAsync)}_{takbisIlceId}";
        var r = await _cacheService.GetValueAsync(cacheKey);
        if (r != null)
        {
            return JsonConvert.DeserializeObject<List<MahalleModel>>(r);
        }

        using var service = Initialize();
        var sonuc = await _logService.WrapServiceAsnyc<int, Mahalle[]>(takbisIlceId, async () => { return await service.GetirMahalleIlceIDDenAsync(takbisIlceId, Durum.Aktif); });
        List<MahalleModel> result = _mapper.Map<List<MahalleModel>>(sonuc.ToList());

        result = result.OrderBy(o => o.Ad).ToList();

        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromDays(30));
        return result;
    }

    public async Task<AnaTasinmazModel> GetirTasinmazByTakbisTasinmazIdAsync(GetirTasinmazByTakbisTasinmazIdQueryModel request)
    {
        using var service = Initialize();
        var sonuc = await _logService.WrapServiceAsnyc<GetirTasinmazByTakbisTasinmazIdQueryModel, Tasinmaz>(request, async () => { return await service.GetirTasinmazIDDenAsync(request.TakbisTasinmazId); });
        AnaTasinmazModel result = _mapper.Map<AnaTasinmazModel>(sonuc);

        return result;
    }

    public async Task<List<AdaModel>> GetirListeTakbisAdaByTakbisMahalleIdAsync(int takbisMahalleId)
    {
        var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirListeTakbisAdaByTakbisMahalleIdAsync)}_{takbisMahalleId}";
        var r = await _cacheService.GetValueAsync(cacheKey);
        if (r != null)
        {
            return JsonConvert.DeserializeObject<List<AdaModel>>(r);
        }
        using var service = Initialize();
        var sonuc = await _logService.WrapServiceAsnyc<int, string[]>(takbisMahalleId, async () => { return await service.GetirAdaNumaralariAsync(takbisMahalleId); });

        List<AdaModel> result = (sonuc).Select(x => new AdaModel { AdaNo = string.IsNullOrWhiteSpace(x) ? "0" : x }).ToList();
        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromDays(2));

        return result;
    }
    public async Task<List<ParselModel>> GetirListeTakbisParselByTakbisMahalleIdAdaNoAsync(int takbisMahalleId, string adaNo)
    {
        var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirListeTakbisParselByTakbisMahalleIdAdaNoAsync)}_{takbisMahalleId}_{adaNo}";
        var r = await _cacheService.GetValueAsync(cacheKey);
        if (r != null)
        {
            return JsonConvert.DeserializeObject<List<ParselModel>>(r);
        }
        using var service = Initialize();
        var sonuc = await _logService.WrapServiceAsnyc<object, string[]>(new { takbisMahalleId = takbisMahalleId, adaNo = adaNo }, async () => { return await service.GetirParselNumaralariAsync(takbisMahalleId, adaNo); });

        List<ParselModel> result = (sonuc).Where(x => x != "DOP").Select(x => new ParselModel { ParselNo = x }).ToList();
        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromDays(2));

        return result;
    }

    public async Task<List<HisseModel>> GetirHisseByTakbisTasinmazIdAsync(GetirHisseTasinmazIdDenQueryModel request)
    {

        var cacheKey = $"{_webHostEnvironment.EnvironmentName}_" + $"{nameof(GetirHisseByTakbisTasinmazIdAsync)}_{request.TakbisTasinmazId}_{request.TapuBolumDurum}";
        var cache = await _cacheService.GetValueAsync(cacheKey);
        if (cache != null)
            return JsonConvert.DeserializeObject<List<HisseModel>>(cache);

        using var service = Initialize();
        TapuBolumDurum tapuBolumDurum = TapuBolumDurum.Aktif;
        if (request.TapuBolumDurum == "Aktif")
        {
            tapuBolumDurum = TapuBolumDurum.Aktif;
        }
        else if (request.TapuBolumDurum == "Pasif")
        {
            tapuBolumDurum = TapuBolumDurum.Pasif;
        }
        else if (request.TapuBolumDurum == "Hepsi")
        {
            tapuBolumDurum = TapuBolumDurum.Hepsi;
        }
        else if (request.TapuBolumDurum == "Taslak")
        {
            tapuBolumDurum = TapuBolumDurum.Taslak;
        }
        var sonuc = await _logService.WrapServiceAsnyc<GetirHisseTasinmazIdDenQueryModel, Hisse[]>(request, async () => { return await service.GetirHisseTasinmazIDDenAsync(request.TakbisTasinmazId, tapuBolumDurum); });

        List<HisseModel> hisseListe = (sonuc).Select(x => new HisseModel
        {
            Id = x.ID,
            TasinmazId = x.TasinmazID,
            Pay = x.Pay.Replace(".000", ""),
            Payda = x.Payda.Replace(".000", ""),
            IstirakNo = x.IstirakNo,
            YevmiyeNo = x.TesisIslem?.Yevmiye == null ? 0 : x.TesisIslem.Yevmiye.YevmiyeNo,
            YevmiyeTarihi = x.TesisIslem?.Yevmiye == null ? DateTime.MinValue : x.TesisIslem.Yevmiye.Tarih,
            MalikId = x.Malik.ID,
            MalikAd = x.Malik.MalikOzetBilgi, //malik tip gerçek veya tüzelden farklı gelirse ad yerine özet bilgiyi göstermek için eklendi
            MalikTip = (TakbisMalikTipEnum)x.Malik.MalikTip,
            TapuBolumDurum = (TapuBolumDurumEnum)x.TapuBolumDurum
        }).ToList();

        //asenkron yapılmadı çünkü web servis tarafında aynı anda birçok istek yapınca policy falsified hatası alınıyor
        foreach (var hisse in hisseListe)
        {
            if (hisse.MalikTip == TakbisMalikTipEnum.GercekKisi)
            {
                var malik = await GetirGercekKisiIDDenAsync(hisse.MalikId);
                if (malik != null)
                {
                    hisse.MalikAd = malik.Ad;
                    hisse.MalikSoyad = malik.Soyad;
                    hisse.MalikTCNo = malik.TcKimlikNo;
                }
            }
            else if (hisse.MalikTip == TakbisMalikTipEnum.TuzelKisi)
            {
                var malik = await GetirTuzelKisiIDDenAsync(hisse.MalikId);
                if (malik != null)
                {
                    hisse.MalikUnvan = malik.Ad;
                    hisse.MalikVergiNo = malik.VergiNo;
                }

                hisse.MalikAd = null;
            }
        }

        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(hisseListe), TimeSpan.FromDays(1));

        return hisseListe;
    }

    public async Task<List<GercekKisiModel>> GetirGercekKisiAsync(string tcKimlikNo)
    {
        var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirGercekKisiAsync)}_{tcKimlikNo}";

        //await _cacheService.Clear(cacheKey);

        var r = await _cacheService.GetValueAsync(cacheKey);
        if (r != null)
        {
            return JsonConvert.DeserializeObject<List<GercekKisiModel>>(r);
        }

        using var service = Initialize();
        var sonuc = await _logService.WrapServiceAsnyc<string, GercekKisi[]>(tcKimlikNo, async () => { return await service.GetirGercekKisiAsync("", "", tcKimlikNo, "", ""); });

        //Aynı tc ye ait birden fazla gerçek kişi bilgisi gelen durumla karşılaşıldığı için liste haline çevrildi
        var result = (sonuc).Select(x => new GercekKisiModel
        {
            Id = x.ID,
            TcKimlikNo = x.TCKimlikNo,
            Ad = x.Ad,
            Soyad = x.Soyad,
            BabaAd = x.BabaAd,
            AnaAd = x.AnaAd,
            DogumTarih = x.DogumTarih,
            DogumYer = x.DogumYer,
            Cilt = x.Cilt,
            Sira = x.Sira,
            NufusCuzdaniSeriNo = x.NufusCuzdaniSeriNo,
            OlumTarih = x.OlumTarih,
            Durum = (DurumEnum)x.Durum
        }).ToList();

        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromDays(180)); //gerçek kişi olduğu için cache için 180 gün kalabilir

        return result;
    }

    public async Task<GercekKisiModel?> GetirGercekKisiIDDenAsync(decimal id)
    {
        var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirGercekKisiIDDenAsync)}_{id}";

        var redisCache = await _cacheService.GetValueAsync(cacheKey);
        if (redisCache != null)
            return JsonConvert.DeserializeObject<GercekKisiModel>(redisCache);

        using var service = Initialize();

        var takbisGercekKisiSorgu = await _logService.WrapServiceAsnyc<decimal, GercekKisi>(id, async () => { return await service.GetirGercekKisiIDDenAsync(id); });
        if (takbisGercekKisiSorgu == null)
            return null;

        var gercekKisi = new GercekKisiModel
        {
            Id = takbisGercekKisiSorgu.ID,
            TcKimlikNo = takbisGercekKisiSorgu.TCKimlikNo,
            Ad = takbisGercekKisiSorgu.Ad,
            Soyad = takbisGercekKisiSorgu.Soyad,
            BabaAd = takbisGercekKisiSorgu.BabaAd,
            AnaAd = takbisGercekKisiSorgu.AnaAd,
            DogumTarih = takbisGercekKisiSorgu.DogumTarih,
            DogumYer = takbisGercekKisiSorgu.DogumYer,
            Cilt = takbisGercekKisiSorgu.Cilt,
            Sira = takbisGercekKisiSorgu.Sira,
            NufusCuzdaniSeriNo = takbisGercekKisiSorgu.NufusCuzdaniSeriNo,
            OlumTarih = takbisGercekKisiSorgu.OlumTarih,
            Durum = (DurumEnum)takbisGercekKisiSorgu.Durum
        };

        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(gercekKisi), TimeSpan.FromDays(180)); //gerçek kişi olduğu için cache için 180 gün kalabilir

        return gercekKisi;
    }

    public async Task<TuzelKisiModel?> GetirTuzelKisiIDDenAsync(decimal id)
    {
        var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirTuzelKisiIDDenAsync)}_{id}";

        var redisCache = await _cacheService.GetValueAsync(cacheKey);
        if (redisCache != null)
            return JsonConvert.DeserializeObject<TuzelKisiModel>(redisCache);

        using var service = Initialize();

        var takbisTuzelKisiSorgu = await _logService.WrapServiceAsnyc<decimal, TuzelKisi>(id, async () => { return await service.GetirTuzelKisiIDDenAsync(id); });
        if (takbisTuzelKisiSorgu == null)
            return null;

        var tuzelKisi = new TuzelKisiModel
        {
            Id = takbisTuzelKisiSorgu.ID,
            Ad = takbisTuzelKisiSorgu.Ad,
            VergiNo = takbisTuzelKisiSorgu.VergiNo,
            TuzelKisiTipi = takbisTuzelKisiSorgu.TuzelKisiTipi,
            SicilNo = takbisTuzelKisiSorgu.SicilNo,
            Durum = (DurumEnum)takbisTuzelKisiSorgu.Durum
        };

        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(tuzelKisi), TimeSpan.FromDays(180)); //gerçek kişi olduğu için cache için 180 gün kalabilir

        return tuzelKisi;
    }

    public async Task<List<HisseModel>> GetirHisseMalikIDDenAsync(decimal malikId)
    {
        var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirHisseMalikIDDenAsync)}_{malikId}";
        var r = await _cacheService.GetValueAsync(cacheKey);
        if (r != null)
        {
            return JsonConvert.DeserializeObject<List<HisseModel>>(r);
        }

        using var service = Initialize();

        var sonuc = await _logService.WrapServiceAsnyc<decimal, Hisse[]>(malikId, async () => { return await service.GetirHisseMalikIDDenAsync(new decimal[] { }, malikId, MalikTip.GercekKisi, TapuBolumDurum.Aktif); });
        List<HisseModel> hisseListe = (sonuc).Select(x => new HisseModel
        {
            Id = x.ID,
            TasinmazId = x.TasinmazID,
            Pay = x.Pay.Replace(".000", ""),
            Payda = x.Payda.Replace(".000", ""),
            IstirakNo = x.IstirakNo,
            YevmiyeNo = x.TesisIslem?.Yevmiye == null ? 0 : x.TesisIslem.Yevmiye.YevmiyeNo,
            YevmiyeTarihi = x.TesisIslem?.Yevmiye == null ? DateTime.MinValue : x.TesisIslem.Yevmiye.Tarih,
            MalikId = x.Malik.ID,
            MalikAd = x.Malik.MalikOzetBilgi, //malik tip gerçek veya tüzelden farklı gelirse ad yerine özet bilgiyi göstermek için eklendi
            MalikTip = (TakbisMalikTipEnum)x.Malik.MalikTip
        }).ToList();

        //asenkron yapılmadı çünkü web servis tarafında aynı anda birçok istek yapınca policy falsified hatası alınıyor
        foreach (var hisse in hisseListe)
        {
            if (hisse.MalikTip == TakbisMalikTipEnum.GercekKisi)
            {
                var malik = await GetirGercekKisiIDDenAsync(hisse.MalikId);
                if (malik != null)
                {
                    hisse.MalikAd = malik.Ad;
                    hisse.MalikSoyad = malik.Soyad;
                    hisse.MalikTCNo = malik.TcKimlikNo;
                }
            }
            else if (hisse.MalikTip == TakbisMalikTipEnum.TuzelKisi)
            {
                var malik = await GetirTuzelKisiIDDenAsync(hisse.MalikId);
                if (malik != null)
                {
                    hisse.MalikUnvan = malik.Ad;
                    hisse.MalikVergiNo = malik.VergiNo;
                }
            }
        }

        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(hisseListe), TimeSpan.FromDays(1));

        return hisseListe;
    }

    public async Task<List<TasinmazModel>> GetirTasinmazHiseliMalikIDDenAsync(decimal malikId)
    {
        var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirTasinmazHiseliMalikIDDenAsync)}_{malikId}";
        var r = await _cacheService.GetValueAsync(cacheKey);
        if (r != null)
        {
            return JsonConvert.DeserializeObject<List<TasinmazModel>>(r);
        }

        using var service = Initialize();

        var sonuc = await _logService.WrapServiceAsnyc<decimal, Tasinmaz[]>(malikId, async () => { return await service.GetirTasinmazMalikBilgisindenAsync(new decimal[] { }, malikId, MalikTip.GercekKisi, TapuBolumDurum.Aktif); });
        List<TasinmazModel> result = (sonuc).Select(x => new TasinmazModel
        {
            AltTasinmazId = x.AltTasinmazID,
            Ada = x.Ada,
            BagimsizBolum = x.BagimsizBolum == null ? null : new BagimsizBolumModel
            {
                ArsaPay = x.BagimsizBolum.ArsaPay,
                ArsaPayda = x.BagimsizBolum.ArsaPayda,
                Blok = x.BagimsizBolum.Blok,
                Giris = x.BagimsizBolum.Giris,
                Id = x.BagimsizBolum.ID,
                Kat = x.BagimsizBolum.Kat,
                No = x.BagimsizBolum.No,
                Tip = (BagimsizBolumTipEnum)x.BagimsizBolum.Tip,
            },
            CiltNo = x.CiltNo,
            Tip = (TasinmazTipEnum)x.Tip,
            Id = x.ID,
            DaimiMustakilHak = x.DaimiMustakilHak == null ? null : new DaimiMustakilHakModel
            {
                Id = x.DaimiMustakilHak.ID,
                BaslamaTarihi = x.DaimiMustakilHak.BaslamaTarihi,
                BitisTarihi = x.DaimiMustakilHak.BitisTarihi,
                Cumle = x.DaimiMustakilHak.Cumle,
                SureAciklama = x.DaimiMustakilHak.SureAciklama,
                TesisBicim = x.DaimiMustakilHak.TesisBicim
            },
            Pafta = x.Pafta,
            Eklenti = x.Eklenti,
            Il = x.Il,
            Ilce = x.Ilce,
            Kurum = x.Kurum,
            KurumId = x.KurumID,
            Mahalle = x.Mahalle,
            MahalleId = x.MahalleID,
            Mevkii = x.Mevkii,
            Muhdesat = x.Muhdesat,
            Nitelik = x.Nitelik,
            Parsel = x.Parsel,
            SayfaNo = x.SayfaNo,
            TapuBolumDurum = (TapuBolumDurumEnum)x.TapuBolumDurum,
            Teferruat = x.Teferruat,
            TerkinIslem = x.TerkinIslem == null ? null : new IslemModel
            {
                Id = x.TerkinIslem.ID,
                IslemDurum = (IslemDurumEnum)x.TerkinIslem.IslemDurum,
                BaslamaSekilAd = x.TerkinIslem.BaslamaSekilAd,
                IslemTanimAd = x.TerkinIslem.IslemTanimAd,
                IptalAciklama = x.TerkinIslem.IptalAciklama
            },
            TesisIslem = x.TesisIslem == null ? null : new IslemModel
            {
                Id = x.TesisIslem.ID,
                IslemDurum = (IslemDurumEnum)x.TesisIslem.IslemDurum,
                BaslamaSekilAd = x.TesisIslem.BaslamaSekilAd,
                IptalAciklama = x.TesisIslem.IptalAciklama,
                IslemTanimAd = x.TesisIslem.IslemTanimAd
            },
            YuzOlcum = x.Yuzolcum,
            HisseListe = GetirHisseMalikIDDenAsync(malikId).GetAwaiter().GetResult()
        }).ToList();

        await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result), TimeSpan.FromDays(1));

        return result;
    }

    private ServiceSoapClient Initialize()
    {
        ServiceSoapClient serviceSoapClient = new ServiceSoapClient(ServiceSoapClient.EndpointConfiguration.ServiceSoap);

        if (_authDto != null)
        {
            serviceSoapClient.ClientCredentials.UserName.UserName = _authDto.UserName;
            serviceSoapClient.ClientCredentials.UserName.Password = _authDto.Password;
            serviceSoapClient.Endpoint.Address = new EndpointAddress(new Uri(_authDto.Url));
        }

        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
        binding.MaxBufferSize = 100000000;
        binding.MaxReceivedMessageSize = 100000000;
        binding.SendTimeout = TimeSpan.FromMinutes(5);
        binding.ReceiveTimeout = TimeSpan.FromMinutes(5);
        serviceSoapClient.Endpoint.Binding = binding;
        if (serviceSoapClient.InnerChannel.State == System.ServiceModel.CommunicationState.Faulted)
        {
            serviceSoapClient = new ServiceSoapClient(ServiceSoapClient.EndpointConfiguration.ServiceSoap);

            if (_authDto != null)
            {
                serviceSoapClient.ClientCredentials.UserName.UserName = _authDto.UserName;
                serviceSoapClient.ClientCredentials.UserName.Password = _authDto.Password;
                serviceSoapClient.Endpoint.Address = new EndpointAddress(new Uri(_authDto.Url));
            }

            binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            binding.MaxBufferSize = 100000000;
            binding.MaxReceivedMessageSize = 100000000;
            serviceSoapClient.Endpoint.Binding = binding;
        }

        return serviceSoapClient;
    }

    private ServiceSoapClient Initialize(ServiceSoapClient serviceSoapClient)
    {
        serviceSoapClient.ClientCredentials.UserName.UserName = _authDto.UserName;
        serviceSoapClient.ClientCredentials.UserName.Password = _authDto.Password;

        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
        binding.ReceiveTimeout = TimeSpan.FromMinutes(5);
        binding.SendTimeout = TimeSpan.FromMinutes(5);
        serviceSoapClient.Endpoint.Binding = binding;

        return serviceSoapClient;
    }
}