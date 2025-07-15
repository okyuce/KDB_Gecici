using AutoMapper;
using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.CQRS.AydinlatmaMetniCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVeren;

public class KaydetBasvuruImzaVerenCommand : IRequest<ResultModel<BasvuruImzaVerenDto>>
{
    public long? BasvuruId { get; set; }
    public long? BasvuruKamuUstlenecekId { get; set; }
    public long? BinaDegerlendirmeId { get; set; }
    public long? BasvuruImzaVerenId { get; set; }
    public long? BasvuruDestekTurId { get; set; } // dosya validasyonları için gerekli.
    public double? BagimsizBolumAlani { get; set; }
    public string? BagimsizBolumNo { get; set; }
    public bool? AhirliKonutMu { get; set; }
    public bool? KonutMu { get; set; }
    public bool? IsyeriMi { get; set; }
    public bool? KamuUstlenecekMi { get; set; }
    public DateOnly? SozlesmeTarihi { get; set; }
    public string? IbanNo { get; set; }
    public bool IbanGirildiMi { get; set; }
    public bool KendimKarsilamakIstiyorum { get; set; }
    public int? HissePay { get; set; }
    public int? HissePayda { get; set; }
    public int? HibeOdemeTutar { get; set; }
    public int? KrediOdemeTutar { get; set; }

    public class KaydetBasvuruImzaVerenCommandHandler : IRequestHandler<KaydetBasvuruImzaVerenCommand, ResultModel<BasvuruImzaVerenDto>>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IBasvuruKamuUstlenecekRepository _basvuruKamuUstlenecekRepository;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IBasvuruImzaVerenRepository _basvuruImzaVerenRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IConfiguration _configuration;
        private readonly IBinaKotUstuSayiRepository _binaKotUstuSayiRepository;

        public KaydetBasvuruImzaVerenCommandHandler(IMapper mapper, IBasvuruKamuUstlenecekRepository basvuruKamuUstlenecekRepository, IConfiguration configuration, IBasvuruRepository basvuruRepository, IBinaDegerlendirmeRepository binaDegerlendirmeRepository, IBasvuruImzaVerenRepository basvuruImzaVerenRepository, IKullaniciBilgi kullaniciBilgi, IBinaKotUstuSayiRepository binaKotUstuSayiRepository, IMediator mediator)
        {
            _mapper = mapper;
            _basvuruKamuUstlenecekRepository = basvuruKamuUstlenecekRepository;
            _configuration = configuration;
            _basvuruRepository = basvuruRepository;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _basvuruImzaVerenRepository = basvuruImzaVerenRepository;
            _kullaniciBilgi = kullaniciBilgi;
            _binaKotUstuSayiRepository = binaKotUstuSayiRepository;
            _mediator = mediator;
        }

        public async Task<ResultModel<BasvuruImzaVerenDto>> Handle(KaydetBasvuruImzaVerenCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<BasvuruImzaVerenDto>();

            Basvuru? basvuru = null;
            BasvuruKamuUstlenecek? basvuruKamuUstlenecek = null;
            if (request?.BasvuruId > 0)
            {
                basvuru = _basvuruRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                        && x.BasvuruId == request.BasvuruId,
                                        true
                                    ).FirstOrDefault();

                if (basvuru == null)
                {
                    result.ErrorMessage("Başvuru bulunamadı.");
                    return await Task.FromResult(result);
                }

                if (basvuru.BasvuruAfadDurumId == (int)BasvuruAfadDurumEnum.Kabul)
                {
                    result.ErrorMessage("Lütfen aktif AFAD başvurunuzu iptal ederek yeniden deneyiniz.");
                    return await Task.FromResult(result);
                }

                var krediList = _basvuruRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                        && x.BasvuruDurumId != (int)BasvuruDurumEnum.BasvuruIptalEdildi
                                                        && x.BasvuruDurumId != (int)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                        && (x.BasvuruDestekTurId == (int)BasvuruDestekTurEnum.HibeVeKredi
                                                                || x.BasvuruDestekTurId == (int)BasvuruDestekTurEnum.Kredi)
                                                        );
                // tüzel kişi ise:
                if (basvuru.TuzelKisiTipId != null)
                {
                    if (basvuru.TuzelKisiVergiNo != null)
                        krediList = krediList.Where(x => x.TuzelKisiVergiNo == basvuru.TuzelKisiVergiNo);
                    else if (basvuru.TuzelKisiMersisNo != null)
                        krediList = krediList.Where(x => x.TuzelKisiMersisNo == basvuru.TuzelKisiMersisNo);
                    else
                    {
                        result.ErrorMessage("Tüzel kişi vergi no ya da mersis no olmadığından işleme devam edilemiyor!");
                        return await Task.FromResult(result);
                    }
                }
                // gerçek kişi ise:
                else
                    krediList = krediList.Where(x => x.TcKimlikNo == basvuru.TcKimlikNo);

                var sayi = krediList.Count();
                if (sayi > 3)
                {
                    result.ErrorMessage("TC Kimlik Numarası, tüzel kişi vergi no ya da tüzel kişi mersis noya bağlı Kredi sayısı 3' ten fazla olduğu için işleme devam edilemiyor. Lütfen önce "
                                            + (sayi - 3) + " adet başvurunuzu iptal ediniz.");
                    return await Task.FromResult(result);
                }
            }
            else
            {
                basvuruKamuUstlenecek = _basvuruKamuUstlenecekRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                    && x.BasvuruKamuUstlenecekId == request.BasvuruKamuUstlenecekId
                                        , true
                                        ).FirstOrDefault();

                if (basvuruKamuUstlenecek == null)
                {
                    result.ErrorMessage("Başvuru bulunamadı.");
                    return await Task.FromResult(result);
                }
            }

            var binaDegerlendirmeQuery = _binaDegerlendirmeRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                                && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId
                                                        , true
                                                        , x => x.BinaYapiRuhsatIzinDosyas.Where(x => x.SilindiMi == false && x.AktifMi == true));
            if (basvuru != null)
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => x.UavtMahalleNo == basvuru.UavtMahalleNo && x.HasarTespitAskiKodu == basvuru.HasarTespitAskiKodu);
            else if (basvuruKamuUstlenecek != null)
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => x.UavtMahalleNo == basvuruKamuUstlenecek.UavtMahalleNo
                                                                        && x.Ada == basvuruKamuUstlenecek.TapuAda
                                                                        && x.Parsel == basvuruKamuUstlenecek.TapuParsel
                                                                );

            var binaDegerlendirme = binaDegerlendirmeQuery.FirstOrDefault();

            if (binaDegerlendirme != null)
            {
                if (binaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiRuhsatinizOnaylanmistir
                     || binaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiIlerlemeSeviyesiYuzde20
                     || binaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiIlerlemeSeviyesiYuzde60
                     || binaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiTamamlanmistir)
                {
                    result.ErrorMessage("Yapı ruhsatınız onaylandığı için işleme devam edilemiyor.");
                    return await Task.FromResult(result);
                }

                var ayar = await _mediator.Send(new GetirAyarQuery());
                if (ayar.IsError)
                {
                    result.ErrorMessage("Ayarlar Okunamadığı için işleme devam edilemiyor.");

                    return await Task.FromResult(result);
                }
                var binaKotUstSayisi = _binaKotUstuSayiRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                                               && x.UavtIlId == binaDegerlendirme.UavtIlNo
                                                                                && (x.UavtIlceId == null || x.UavtIlceId == binaDegerlendirme.UavtIlceNo)
                                                                                && (x.UavtMahalleId == null || x.UavtMahalleId == binaDegerlendirme.UavtMahalleNo)
                                                                                && (x.Ada == null || x.Ada == binaDegerlendirme.Ada)
                                                                                && (x.Parsel == null || x.Parsel == binaDegerlendirme.Parsel)
                                                                               , false).FirstOrDefault()?
                                                                               //.KotUstuKatSayisi ?? AyarConstants.KotUstKatSayisi;
                                                                               .KotUstuKatSayisi ?? ayar.Result.Basvuru.KotUstKatSayisi;
                // izin belgesi girilmemişse ve BültenNo NULL değilse, KotUstKatSayisi 4' den büyük olanlar işleme devam edemez.
                if (binaDegerlendirme.BinaYapiRuhsatIzinDosyas?.Any(x => x.SilindiMi == false && x.AktifMi == true) != true
                        && binaDegerlendirme.BultenNo > 0
                        && binaDegerlendirme.KotUstKatSayisi > binaKotUstSayisi)
                {
                    result.ErrorMessage("Kot Üstü Kat sayısı " + binaKotUstSayisi + "' den fazla olduğu için işleme devam edilemiyor.");
                    return await Task.FromResult(result);
                }
            }

            var basvuruImzaVerenQuery = _basvuruImzaVerenRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true,
                                                    true,
                                                    i => i.BasvuruImzaVerenDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true));

            if (basvuru != null)
                basvuruImzaVerenQuery = basvuruImzaVerenQuery.Where(x => x.BasvuruId == request.BasvuruId);
            else if (basvuruKamuUstlenecek != null)
                basvuruImzaVerenQuery = basvuruImzaVerenQuery.Where(x => x.BasvuruKamuUstlenecekId == request.BasvuruKamuUstlenecekId);

            var basvuruImzaVeren = basvuruImzaVerenQuery.FirstOrDefault();

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            long.TryParse(kullaniciBilgi.KullaniciId, out long kullaniciId);
            string? ipAdresi = kullaniciBilgi?.IpAdresi;

            basvuruImzaVeren ??= new BasvuruImzaVeren
            {
                SilindiMi = false,
                AktifMi = true,
                OlusturmaTarihi = DateTime.Now,
                OlusturanKullaniciId = kullaniciId,
                OlusturanIp = ipAdresi,
                BasvuruId = basvuru?.BasvuruId,
                BasvuruKamuUstlenecekId = basvuruKamuUstlenecek?.BasvuruKamuUstlenecekId,
                BasvuruDestekTurId = basvuru != null ? basvuru.BasvuruDestekTurId : basvuruKamuUstlenecek.BasvuruDestekTurId,
            };

            basvuruImzaVeren.SozlesmeTarihi = request.SozlesmeTarihi;
            basvuruImzaVeren.BagimsizBolumAlani = request.BagimsizBolumAlani ?? basvuruImzaVeren.BagimsizBolumAlani;
            basvuruImzaVeren.BagimsizBolumNo = request.BagimsizBolumNo;
            basvuruImzaVeren.HissePay = request.HissePay;
            basvuruImzaVeren.HissePayda = request.HissePayda;
            if (request?.AhirliKonutMu == true)
                basvuruImzaVeren.BasvuruTurId = request?.AhirliKonutMu == true && (basvuru?.BasvuruId > 0 || basvuruKamuUstlenecek?.BasvuruKamuUstlenecekId > 0) && basvuru?.BasvuruTurId != BasvuruTurEnum.Ticarethane
                                                ? BasvuruTurEnum.AhirliKonut
                                                : basvuru?.BasvuruTurId ?? basvuruKamuUstlenecek.BasvuruTurId;
            else if (request?.KonutMu == true)
                basvuruImzaVeren.BasvuruTurId = request?.KonutMu == true && (basvuru?.BasvuruId > 0 || basvuruKamuUstlenecek?.BasvuruKamuUstlenecekId > 0)
                                            ? BasvuruTurEnum.Konut
                                            : basvuru?.BasvuruTurId ?? basvuruKamuUstlenecek.BasvuruTurId;
            else if (request?.IsyeriMi == true)
                basvuruImzaVeren.BasvuruTurId = request?.IsyeriMi == true && (basvuru?.BasvuruId > 0 || basvuruKamuUstlenecek?.BasvuruKamuUstlenecekId > 0)
                                            ? BasvuruTurEnum.Ticarethane
                                            : basvuru?.BasvuruTurId ?? basvuruKamuUstlenecek.BasvuruTurId;
            else
                basvuruImzaVeren.BasvuruTurId = basvuru?.BasvuruTurId ?? basvuruKamuUstlenecek.BasvuruTurId;

            if (request.IbanGirildiMi)
            {
                if (FluentValidationExtension.IbanDogrula(request?.IbanNo))
                {
                    basvuruImzaVeren.IbanGirildiMi = request.IbanGirildiMi;
                    basvuruImzaVeren.IbanNo = request?.IbanNo;
                }
                else
                {
                    result.ErrorMessage(string.Format(ValidationMessageConstants.HATALI_VEYA_GECERSIZ, "IBAN No"));
                    return await Task.FromResult(result);
                }
            }
            else
            {
                basvuruImzaVeren.IbanGirildiMi = request.IbanGirildiMi;
                basvuruImzaVeren.IbanNo = request?.IbanNo;
            }

            if (request?.KamuUstlenecekMi != true)
            {
                if (!request.KendimKarsilamakIstiyorum)
                {
                    #region ...: Bağımsız Bölüm Alanına Göre Kredi Hesaplamaları :...
                    if (basvuruImzaVeren.BasvuruDestekTurId == BasvuruDestekTurEnum.Hibe || basvuruImzaVeren.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi)
                    {
                        if (request?.AhirliKonutMu == true)
                        {
                            if (request.HissePay == null && request.HissePayda == null)
                                basvuruImzaVeren.HibeOdemeTutar = BasvuruTurEnum.AhirlikonutHibeTutari;
                            else
                                basvuruImzaVeren.HibeOdemeTutar = BasvuruTurEnum.AhirlikonutHibeTutari * request.HissePay / request.HissePayda;
                        }
                        else
                        {
                            if (request.HissePay == null && request.HissePayda == null)
                                basvuruImzaVeren.HibeOdemeTutar = basvuruImzaVeren.BasvuruTurId == BasvuruTurEnum.Konut
                                                                        ? BasvuruTurEnum.EvHibeTutari : BasvuruTurEnum.TicarethaneHibeTutari;
                            else
                                basvuruImzaVeren.HibeOdemeTutar = basvuruImzaVeren.BasvuruTurId == BasvuruTurEnum.Konut
                                                                        ? BasvuruTurEnum.EvHibeTutari * request.HissePay / request.HissePayda : BasvuruTurEnum.TicarethaneHibeTutari * request.HissePay / request.HissePayda;
                        }
                    }
                    else
                        basvuruImzaVeren.HibeOdemeTutar = null;
                    #endregion

                    #region ...: Bağımsız Bölüm Alanına Göre Kredi Hesaplamaları :...
                    if (basvuruImzaVeren.BasvuruDestekTurId == BasvuruDestekTurEnum.Kredi || basvuruImzaVeren.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi)
                    {
                        if (request?.AhirliKonutMu == true)
                        {
                            if (request.HissePay == null && request.HissePayda == null)
                                basvuruImzaVeren.KrediOdemeTutar = BasvuruTurEnum.AhirlikonutKrediTutari;
                            else
                                basvuruImzaVeren.KrediOdemeTutar = BasvuruTurEnum.AhirlikonutKrediTutari * request.HissePay / request.HissePayda;

                        }
                        else
                        {
                            if (request.HissePay == null && request.HissePayda == null)
                                basvuruImzaVeren.KrediOdemeTutar = basvuruImzaVeren.BasvuruTurId == BasvuruTurEnum.Konut
                                                                    ? BasvuruTurEnum.EvKrediTutari : BasvuruTurEnum.TicarethaneKrediTutari;
                            else
                                basvuruImzaVeren.KrediOdemeTutar = basvuruImzaVeren.BasvuruTurId == BasvuruTurEnum.Konut
                                                                                                ? BasvuruTurEnum.EvKrediTutari * request.HissePay / request.HissePayda : BasvuruTurEnum.TicarethaneKrediTutari * request.HissePay / request.HissePayda;

                        }
                    }
                    else
                        basvuruImzaVeren.KrediOdemeTutar = null;
                    #endregion


                }
            }
            else
            {
                basvuruImzaVeren.HibeOdemeTutar = request?.HibeOdemeTutar;
                basvuruImzaVeren.KrediOdemeTutar = request?.KrediOdemeTutar;
            }

            if (basvuruImzaVeren.BasvuruImzaVerenId > 0)
            {
                basvuruImzaVeren.GuncellemeTarihi = DateTime.Now;
                basvuruImzaVeren.GuncelleyenKullaniciId = kullaniciId;
                basvuruImzaVeren.GuncelleyenIp = kullaniciBilgi.IpAdresi;

                _basvuruImzaVerenRepository.Update(basvuruImzaVeren);
            }
            else
            {
                await _basvuruImzaVerenRepository.AddAsync(basvuruImzaVeren);
            }

            await _basvuruImzaVerenRepository.SaveChanges();

            result.Result = _mapper.Map<BasvuruImzaVerenDto>(basvuruImzaVeren);

            return await Task.FromResult(result);
        }
    }
}