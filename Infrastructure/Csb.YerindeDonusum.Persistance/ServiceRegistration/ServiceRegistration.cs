using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Interfaces.Maks;
using Csb.YerindeDonusum.Application.Interfaces.YapiDenetimSeviye;
using Csb.YerindeDonusum.Persistance.Context;
using Csb.YerindeDonusum.Persistance.Repositories;
using Csb.YerindeDonusum.Persistance.Repositories.Kds;
using Csb.YerindeDonusum.Persistance.Repositories.Maks;
using Csb.YerindeDonusum.Persistance.Repositories.YapiDenetimSeviye;
using Csb.YerindeDonusum.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Csb.YerindeDonusum.Persistance.ServiceRegistration;

public static class ServiceRegistration
{
    public static IServiceCollection AddPersistanceServiceRegistration(this IServiceCollection serviceProviders, IConfiguration configuration)
    {
        // db configuration
        // serviceProviders.AddDbContext<ApplicationDbContext>(opt =>
        //     opt.UseNpgsql("")
        // );

        //serviceProviders.AddDbContext<yarisi_bizdenContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("KentselDonusum").ToString()));
        //serviceProviders.AddDbContext<ApplicationDbContext>();
        serviceProviders.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("YerindeDonusum")?.ToString(), x => x.UseNetTopologySuite())
        );
        serviceProviders.AddDbContext<KdsDbContext>();
        serviceProviders.AddDbContext<YapiDenetimSeviyeDbContext>();
        serviceProviders.AddDbContext<MaksDbContext>();

        // repository dependency injection definition
        serviceProviders.AddScoped<IAydinlatmaMetniRepository, AydinlatmaMetniRepository>();
        serviceProviders.AddScoped<IBasvuruDosyaRepository, BasvuruDosyaRepository>();
        serviceProviders.AddScoped<IBasvuruRepository, BasvuruRepository>();
        serviceProviders.AddScoped<IBasvuruKamuUstlenecekRepository, BasvuruKamuUstlenecekRepository>();
        serviceProviders.AddScoped<IBinaOdemeRepository, BinaOdemeRepository>();
        serviceProviders.AddScoped<IBinaOdemeDurumRepository, BinaOdemeDurumRepository>();
        serviceProviders.AddScoped<IBasvuruTapuBilgiRepository, BasvuruTapuBilgiRepository>();
        serviceProviders.AddScoped<IKullaniciRepository, KullaniciRepository>();
        serviceProviders.AddScoped<IKullaniciGirisKodRepository, KullaniciGirisKodRepository>();
        serviceProviders.AddScoped<IKullaniciGirisKodDenemeRepository, KullaniciGirisKodDenemeRepository>();
        serviceProviders.AddScoped<IKullaniciGirisBasariliRepository, KullaniciGirisBasariliRepository>();
        serviceProviders.AddScoped<IKullaniciGirisHataRepository, KullaniciGirisHataRepository>();
        serviceProviders.AddScoped<IBasvuruDurumRepository, BasvuruDurumRepository>();
        serviceProviders.AddScoped<IBasvuruAfadDurumRepository, BasvuruAfadDurumRepository>();
        serviceProviders.AddScoped<IBasvuruDosyaTurRepository, BasvuruDosyaTurRepository>();
        serviceProviders.AddScoped<IBasvuruTurRepository, BasvuruTurRepository>();
        serviceProviders.AddScoped<IBasvuruDestekTurRepository, BasvuruDestekTurRepository>();
        serviceProviders.AddScoped<ISikcaSorulanSoruRepository, SikcaSorulanSoruRepository>();
        serviceProviders.AddScoped<IAyarRepository, AyarRepository>();
        serviceProviders.AddScoped<IIstisnaAskiKoduRepository, IstisnaAskiKoduRepository>();
        serviceProviders.AddScoped<IIstisnaAskiKoduDosyaRepository, IstisnaAskiKoduDosyaRepository>();
        serviceProviders.AddScoped<IBilgilendirmeMesajRepository, BilgilendirmeMesajRepository>();
        serviceProviders.AddScoped<IOfisKonumRepository, OfisKonumRepository>();
        serviceProviders.AddScoped<IBasvuruKanalRepository, BasvuruKanalRepository>();
        serviceProviders.AddScoped<IBirimRepository, BirimRepository>();
        serviceProviders.AddScoped<IRolRepository, RolRepository>();
        serviceProviders.AddScoped<IKullaniciRolRepository, KullaniciRolRepository>();
        serviceProviders.AddScoped<IKullaniciHesapTipRepository, KullaniciHesapTipRepository>();
        serviceProviders.AddScoped<ITakbisIlRepository, TakbisIlRepository>();
        serviceProviders.AddScoped<ISmsLogRepository, SmsLogRepository>();
        serviceProviders.AddScoped<IAfadGirisBilgiRepository, AfadGirisBilgiRepository>();
        serviceProviders.AddScoped<IBasvuruImzaVerenRepository, BasvuruImzaVerenRepository>();
        serviceProviders.AddScoped<IBasvuruImzaVerenDosyaRepository, BasvuruImzaVerenDosyaRepository>();
        serviceProviders.AddScoped<IBasvuruIptalTurRepository, BasvuruIptalTurRepository>();
        serviceProviders.AddScoped<IAfadBasvuruRepository, AfadBasvuruRepository>();
        serviceProviders.AddScoped<IAfadBasvuruTekilRepository, AfadBasvuruTekilRepository>();
        serviceProviders.AddScoped<ITebligatGonderimRepository, TebligatGonderimRepository>();
        serviceProviders.AddScoped<ITebligatGonderimDetayRepository, TebligatGonderimDetayRepository>();
        serviceProviders.AddScoped<ITebligatGonderimDetayDosyaRepository, TebligatGonderimDetayDosyaRepository>();

        #region Kds
        serviceProviders.AddScoped<IKdsHasartespitTespitVeriRepository, KdsHasartespitTespitVeriRepository>();
        serviceProviders.AddScoped<IKdsHaneRepository, KdsHaneRepository>();
        serviceProviders.AddScoped<IKdsBoyutTblIlRepository, KdsBoyutTblIlRepository>();
        serviceProviders.AddScoped<IKdsBoyutTblIlceRepository, KdsBoyutTblIlceRepository>();
        serviceProviders.AddScoped<IKdsBoyutTblMahalleRepository, KdsBoyutTblMahalleRepository>();
        serviceProviders.AddScoped<IKdsBoyutTblCsbmRepository, KdsBoyutTblCsbmRepository>();
        serviceProviders.AddScoped<IKdsBoyutTblNumaratajRepository, KdsBoyutTblNumaratajRepository>();
        serviceProviders.AddScoped<IKdsBoyutTblBagimsizBolumRepository, KdsBoyutTblBagimsizBolumRepository>();
        serviceProviders.AddScoped<IKdsBoyutTblYapiRepository, KdsBoyutTblYapiRepository>();
        serviceProviders.AddScoped<IKdsYerindedonusumBinabazliOranRepository, KdsYerindedonusumBinabazliOranRepository>();
        serviceProviders.AddScoped<IKdsBasvuruRepository, KdsBasvuruRepository>();
        serviceProviders.AddScoped<IKdsYerindedonusumRezervAlanlarRepository, KdsYerindedonusumRezervAlanlarRepository>();
        #endregion

        #region Başvuru Değerlendirme
        serviceProviders.AddScoped<IBinaDegerlendirmeRepository, BinaDegerlendirmeRepository>();
        serviceProviders.AddScoped<IBinaDegerlendirmeDurumRepository, BinaDegerlendirmeDurumRepository>();
        serviceProviders.AddScoped<IBinaDegerlendirmeDosyaRepository, BinaDegerlendirmeDosyaRepository>();
        serviceProviders.AddScoped<IBinaAdinaYapilanYardimRepository, BinaAdinaYapilanYardimRepository>();
        serviceProviders.AddScoped<IBinaAdinaYapilanYardimTipiRepository, BinaAdinaYapilanYardimTipiRepository>();
        serviceProviders.AddScoped<IBinaNakdiYardimTaksitDosyaRepository, BinaNakdiYardimTaksitDosyaRepository>();
        serviceProviders.AddScoped<IBinaNakdiYardimTaksitRepository, BinaNakdiYardimTaksitRepository>();
        serviceProviders.AddScoped<IBinaYapiDenetimSeviyeTespitDosyaRepository, BinaYapiDenetimSeviyeTespitDosyaRepository>();
        serviceProviders.AddScoped<IBinaYapiDenetimSeviyeTespitRepository, BinaYapiDenetimSeviyeTespitRepository>();
        serviceProviders.AddScoped<IBinaYapiRuhsatIzinDosyaRepository, BinaYapiRuhsatIzinDosyaRepository>();
        serviceProviders.AddScoped<IBinaMuteahhitRepository, BinaMuteahhitRepository>();
        serviceProviders.AddScoped<IBinaKotUstuSayiRepository, BinaKotUstuSayiRepository>();
        #endregion

        #region Yapı Denetim Seviye
        serviceProviders.AddScoped<IYapiDenetimSeviyeMvMuteahhitYibfListRepository, YapiDenetimSeviyeMvMuteahhitYibfListRepository>();
        #endregion

        #region Maks
        serviceProviders.AddScoped<IMaksTopluBagimsizbolumRepository, MaksTopluBagimsizbolumRepository>();
        serviceProviders.AddScoped<IMaksTopluCbsmRepository, MaksTopluCsbmRepository>();
        serviceProviders.AddScoped<IMaksTopluNumartajRepository, MaksTopluNumartajRepository>();
        #endregion

        return serviceProviders;
    }
}