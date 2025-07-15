using AutoMapper;
using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.CQRS.AydinlatmaMetniCQRS.Queries;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Queries.GetirBasvuruImzaByBasvuruId;

public class GetirBasvuruImzaByBasvuruIdQuery : IRequest<ResultModel<BasvuruImzaVerenDto>>
{
    public long? BasvuruId { get; set; }
    public long? BasvuruKamuUstlenecekId { get; set; }

    public class GetirBasvuruImzaByBasvuruIdQueryHandler : IRequestHandler<GetirBasvuruImzaByBasvuruIdQuery, ResultModel<BasvuruImzaVerenDto>>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IAfadBasvuruTekilRepository _afadBasvuruTekilRepository;
        private readonly IBasvuruKamuUstlenecekRepository _basvuruKamuUstlenecekRepository;
        private readonly IBasvuruImzaVerenRepository _basvuruImzaVerenRepository;
        private readonly IBinaKotUstuSayiRepository _binaKotUstuSayiRepository;

        public GetirBasvuruImzaByBasvuruIdQueryHandler(IAfadBasvuruTekilRepository afadBasvuruTekilRepository, IBasvuruKamuUstlenecekRepository basvuruKamuUstlenecekRepository, IMapper mapper, IBasvuruRepository basvuruRepository, IBasvuruImzaVerenRepository basvuruImzaVerenRepository, IBinaKotUstuSayiRepository binaKotUstuSayiRepository, IMediator mediator)
        {
            _basvuruKamuUstlenecekRepository = basvuruKamuUstlenecekRepository;
            _mapper = mapper;
            _basvuruRepository = basvuruRepository;
            _basvuruImzaVerenRepository = basvuruImzaVerenRepository;
            _binaKotUstuSayiRepository = binaKotUstuSayiRepository;
            _afadBasvuruTekilRepository = afadBasvuruTekilRepository;
            _mediator = mediator;
        }

        public Task<ResultModel<BasvuruImzaVerenDto>> Handle(GetirBasvuruImzaByBasvuruIdQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<BasvuruImzaVerenDto>();

            Basvuru? basvuru = null;
            BasvuruKamuUstlenecek? basvuruKamuUstlenecek = null;
            if (request?.BasvuruId > 0)
            {
                basvuru = _basvuruRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                            && x.BasvuruId == request.BasvuruId,
                                    true,
                                    x => x.BinaDegerlendirme,
                                    x => x.BinaDegerlendirme.BinaYapiRuhsatIzinDosyas.Where(x => x.SilindiMi == false && x.AktifMi == true)
                                ).FirstOrDefault();

                if (basvuru == null)
                {
                    result.ErrorMessage("Başvuru bulunamadı.");
                    return Task.FromResult(result);
                }

                var hibelist = _basvuruRepository.GetWhere(x => x.AktifMi == true
                                         && x.SilindiMi == false
                                         && (
                                                x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruReddedilmistir
                                            )
                                            && (x.BasvuruDestekTurId == BasvuruDestekTurEnum.Hibe || x.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi)
                                            && (x.BasvuruTurId == BasvuruTurEnum.Konut || x.BasvuruTurId == BasvuruTurEnum.AhirliKonut)
                                         && ((basvuru.TuzelKisiTipId == null && x.TcKimlikNo == basvuru.TcKimlikNo) || (basvuru.TuzelKisiTipId != null && (x.TuzelKisiVergiNo == basvuru.TuzelKisiVergiNo || x.TuzelKisiMersisNo == basvuru.TuzelKisiMersisNo)))
                );

                if (basvuru.TuzelKisiTipId != null)
                {
                    if (basvuru.TuzelKisiVergiNo != null)
                        hibelist = hibelist.Where(x => x.TuzelKisiVergiNo == basvuru.TuzelKisiVergiNo);
                    else if (basvuru.TuzelKisiMersisNo != null)
                        hibelist = hibelist.Where(x => x.TuzelKisiMersisNo == basvuru.TuzelKisiMersisNo);
                    else
                    {
                        result.ErrorMessage("Tüzel kişi vergi no ya da mersis no olmadığından işleme devam edilemiyor!");
                        return Task.FromResult(result);
                    }
                }
                // gerçek kişi ise:
                else
                    hibelist = hibelist.Where(x => x.TcKimlikNo == basvuru.TcKimlikNo && x.TuzelKisiTipId == null);


                var hibeSehirSayisi = hibelist.Select(x => x.UavtIlNo).Distinct().Count();

                var ayniSahsinKonutTurundekiHibeBasvuruSayisi = hibelist.Count();

                
                if (ayniSahsinKonutTurundekiHibeBasvuruSayisi > 1)
                {
                    if (hibeSehirSayisi > 1)
                    {
                        result.ErrorMessage("TC Kimlik Numarasına ya da Vergi Kimlik Numarasına bağlı konut veya ahırlı konut tiplerinde başka şehirlerde toplam hibe başvuru sayısı 1' den fazla olduğu için işleme devam edilemiyor. Lütfen önce "
                                                + (ayniSahsinKonutTurundekiHibeBasvuruSayisi - 1) + " adet başvurunuzu iptal ediniz.");
                        return Task.FromResult(result);
                    }
                    else
                    {
                        result.ErrorMessage("TC Kimlik Numarasına ya da Vergi Kimlik Numarasına bağlı konut veya ahırlı konut tiplerinde toplam hibe başvuru sayısı 1' den fazla olduğu için işleme devam edilemiyor. Lütfen önce "
                                           + (ayniSahsinKonutTurundekiHibeBasvuruSayisi - 1) + " adet başvurunuzu iptal ediniz.");
                        return Task.FromResult(result);
                    }                   
                }

                var ayar = _mediator.Send(new GetirAyarQuery()).Result;
                if (ayar.IsError)
                {
                    result.ErrorMessage("Ayarlar Okunamadığı için işleme devam edilemiyor.");

                    return Task.FromResult(result);
                }
                var binaKotUstSayisi = _binaKotUstuSayiRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                                                && x.UavtIlId == basvuru.UavtIlNo
                                                                                && (x.UavtIlceId == null || x.UavtIlceId == basvuru.UavtIlceNo)
                                                                                && (x.UavtMahalleId == null || x.UavtMahalleId == basvuru.UavtMahalleNo)
                                                                                && (x.Ada == null || x.Ada == basvuru.TapuAda)
                                                                                && (x.Parsel == null || x.Parsel == basvuru.TapuParsel)
                                                                                , false).FirstOrDefault()?
                                                                                //.KotUstuKatSayisi ?? AyarConstants.KotUstKatSayisi;
                                                                                .KotUstuKatSayisi ?? ayar.Result.Basvuru.KotUstKatSayisi;

                // izin belgesi girilmemişse ve BültenNo NULL değilse, KotUstKatSayisi 4' den büyük olanlar işleme devam edemez.
                if (basvuru.BinaDegerlendirme?.BinaYapiRuhsatIzinDosyas?.Any(x => x.SilindiMi == false && x.AktifMi == true) != true
                    && basvuru.BinaDegerlendirme?.BultenNo > 0
                    && basvuru.BinaDegerlendirme?.KotUstKatSayisi > binaKotUstSayisi)
                {
                    result.ErrorMessage("Kot Üstü Kat sayısı " + binaKotUstSayisi + "' den fazla olduğu için işleme devam edilemiyor.");
                    return Task.FromResult(result);
                }

                var krediList = _basvuruRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
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
                        return Task.FromResult(result);
                    }
                }
                // gerçek kişi ise:
                else
                    krediList = krediList.Where(x => x.TcKimlikNo == basvuru.TcKimlikNo && x.TuzelKisiTipId == null);

                var krediSehirSayisi = krediList.Select(x => x.UavtIlNo).Distinct().Count();

                var sayi = krediList.Count();
                if (sayi > 3)
                {
                    if (krediSehirSayisi > 1)
                    {
                        result.ErrorMessage("TC Kimlik Numarası, tüzel kişi vergi no ya da tüzel kişi mersis noya bağlı başka şehirlerde toplam Kredi sayısı 3' ten fazla olduğu için işleme devam edilemiyor. Lütfen önce "
                                           + (sayi - 3) + " adet başvurunuzu iptal ediniz.");
                        return Task.FromResult(result);
                    }
                    else
                    {
                        result.ErrorMessage("TC Kimlik Numarası, tüzel kişi vergi no ya da tüzel kişi mersis noya bağlı Kredi sayısı 3' ten fazla olduğu için işleme devam edilemiyor. Lütfen önce "
                                                                  + (sayi - 3) + " adet başvurunuzu iptal ediniz.");
                        return Task.FromResult(result);
                    }

                }
            }
            else
            {
                basvuruKamuUstlenecek = _basvuruKamuUstlenecekRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                            && x.BasvuruKamuUstlenecekId == request.BasvuruKamuUstlenecekId,
                                    true,
                                    x => x.BinaDegerlendirme,
                                    x => x.BinaDegerlendirme.BinaYapiRuhsatIzinDosyas.Where(x => x.SilindiMi == false && x.AktifMi == true)
                                ).FirstOrDefault();

                if (basvuruKamuUstlenecek == null)
                {
                    result.ErrorMessage("Başvuru bulunamadı.");
                    return Task.FromResult(result);
                }
            }

            var basvuruImzaVerenQuery = _basvuruImzaVerenRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                    , true
                                    , i => i.Basvuru
                                    , i => i.BasvuruKamuUstlenecek
                                    , i => i.BasvuruImzaVerenDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true));

            if (basvuru != null)
                basvuruImzaVerenQuery = basvuruImzaVerenQuery.Where(x => x.BasvuruId == basvuru.BasvuruId);
            else if (basvuruKamuUstlenecek != null)
                basvuruImzaVerenQuery = basvuruImzaVerenQuery.Where(x => x.BasvuruKamuUstlenecekId == basvuruKamuUstlenecek.BasvuruKamuUstlenecekId);

            var basvuruImzaVeren = basvuruImzaVerenQuery.FirstOrDefault();


            result.Result = _mapper.Map<BasvuruImzaVerenDto>(basvuruImzaVeren);

            if (result.Result == null && basvuru != null)
            {
                result.Result = new BasvuruImzaVerenDto()
                {
                    BasvuruTurId = basvuru.BasvuruTurId,
                    BasvuruDestekTurId = basvuru.BasvuruDestekTurId,
                    BasvuruTurIdOrjinal = basvuru.BasvuruTurId,
                    BasvuruGuid = basvuru.BasvuruGuid,
                    BasvuruId = basvuru.BasvuruId,
                    TcKimlikNo = StringAddon.ToMaskedWord(basvuru.TcKimlikNo.ToString(), 3),
                    AdSoyad = string.Concat(basvuru.Ad, " ", basvuru.Soyad),
                };
            }
            else if (result.Result == null && basvuruKamuUstlenecek != null)
            {
                result.Result = new BasvuruImzaVerenDto()
                {
                    BasvuruTurId = basvuruKamuUstlenecek.BasvuruTurId,
                    BasvuruDestekTurId = basvuruKamuUstlenecek.BasvuruDestekTurId,
                    BasvuruTurIdOrjinal = basvuruKamuUstlenecek.BasvuruTurId,
                    BasvuruKamuUstlenecekGuid = basvuruKamuUstlenecek.BasvuruKamuUstlenecekGuid,
                    BasvuruKamuUstlenecekId = basvuruKamuUstlenecek.BasvuruKamuUstlenecekId,
                    TcKimlikNo = StringAddon.ToMaskedWord(basvuruKamuUstlenecek.TcKimlikNo.ToString(), 3),
                    AdSoyad = string.Concat(basvuruKamuUstlenecek.Ad, " ", basvuruKamuUstlenecek.Soyad),
                    TuzelKisiAdi = basvuruKamuUstlenecek.TuzelKisiAdi,
                    TuzelKisiMersisNo = basvuruKamuUstlenecek.TuzelKisiMersisNo,
                    TuzelKisiAdres = basvuruKamuUstlenecek.TuzelKisiAdres,
                    TuzelKisiTipId = basvuruKamuUstlenecek.TuzelKisiTipId,
                    TuzelKisiVergiNo = basvuruKamuUstlenecek.TuzelKisiVergiNo,
                    TuzelKisiYetkiTuru = basvuruKamuUstlenecek.TuzelKisiYetkiTuru,
                };
            }
            //Kendim üstlenmek istiyorum başvurularını atlatmak için basvuruImzaVeren.KrediOdemeTutar != 0 && basvuruImzaVeren.HibeOdemeTutar != 0 konulmuştur.

            if (basvuru != null && basvuru.TapuTasinmazId != null
                && basvuru.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.Iptal
                && basvuru.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.IptalEdilmistir
                && basvuru.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.Red
                && (basvuruImzaVeren == null || (basvuruImzaVeren != null && basvuruImzaVeren.KrediOdemeTutar != 0 && basvuruImzaVeren.HibeOdemeTutar != 0)))
            {
                if (_afadBasvuruTekilRepository.GetAllQueryable().Any(x => x.TasinmazId == basvuru.TapuTasinmazId
                                && x.CsbSilindiMi == false && x.CsbAktifMi == true
                                && x.DegerlendirmeIptalDurumu != null && x.DegerlendirmeIptalDurumu!.ToLower().Trim() != "evet"
                && (
                    (x.ItirazDegerlendirmeSonucu != null && x.ItirazDegerlendirmeSonucu!.ToLower().Replace("afad", "").Trim() != BasvuruAfadDurumEnum.Iptal.GetDisplayName())
                    || (x.ItirazDegerlendirmeSonucu == null && x.DegerlendirmeDurumu!.ToLower().Replace("afad", "").Trim() != BasvuruAfadDurumEnum.Iptal.GetDisplayName())
                )
                && (
                    (x.ItirazDegerlendirmeSonucu != null && x.ItirazDegerlendirmeSonucu!.ToLower().Replace("afad", "").Trim() != BasvuruAfadDurumEnum.IptalEdilmistir.GetDisplayName())
                    || (x.ItirazDegerlendirmeSonucu == null && x.DegerlendirmeDurumu!.ToLower().Replace("afad", "").Trim() != BasvuruAfadDurumEnum.IptalEdilmistir.GetDisplayName())
                )
                && (
                    (x.ItirazDegerlendirmeSonucu != null && x.ItirazDegerlendirmeSonucu!.ToLower().Replace("afad", "").Trim() != BasvuruAfadDurumEnum.Red.GetDisplayName())
                    || (x.ItirazDegerlendirmeSonucu == null && x.DegerlendirmeDurumu!.ToLower().Replace("afad", "").Trim() != BasvuruAfadDurumEnum.Red.GetDisplayName())
                )
                ))
                {
                    result.Result.TasinmazinAfadBasvurusuVarMi = true;
                }
            }
            return Task.FromResult(result);
        }
    }
}