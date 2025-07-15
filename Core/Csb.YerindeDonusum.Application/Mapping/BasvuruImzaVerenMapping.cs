using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVeren;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruImzaVerenSozlesme;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Queries.GetirBasvuruImzaByBasvuruId;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirDetayHibeTaahhutnameSozlesme;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirDetaySozlesmeGeriOdemeBilgileri;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirDetayYerindeYapimKrediSozlesme;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;
using Csb.YerindeDonusum.Domain.Entities.Kds;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BasvuruImzaVerenMapping : Profile
{
    public BasvuruImzaVerenMapping()
    {
        CreateMap<BasvuruImzaVeren, GetirDetayHibeTaahhutnameSozlesmeQueryResponseModel>()
            .ForMember(dest => dest.UavtIlAdi, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.UavtIlAdi
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.UavtIlAdi
                                                                            : default))
            .ForMember(dest => dest.UavtIlceAdi, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.UavtIlceAdi
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.UavtIlceAdi
                                                                            : default))
            .ForMember(dest => dest.UavtMahalleAdi, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.UavtMahalleAdi
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.UavtMahalleAdi
                                                                            : default))
            .ForMember(dest => dest.TapuAda, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.TapuAda
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.TapuAda
                                                                            : default))
            .ForMember(dest => dest.TapuParsel, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.TapuParsel
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.TapuParsel
                                                                            : default))
            .ForMember(dest => dest.UavtDisKapiNo, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.UavtDisKapiNo
                                                                        : default))
            .ForMember(dest => dest.TcKimlikNo, opt => opt.MapFrom(src => src.Basvuru != null
                                                            ? (
                                                                    src.Basvuru.TuzelKisiTipId == null ?
                                                                    StringAddon.ToMaskedWord(src.Basvuru.TcKimlikNo.ToString(), 3)
                                                                    :
                                                                    src.Basvuru.TuzelKisiVergiNo
                                                                )
                                                            : src.BasvuruKamuUstlenecek != null
                                                                ?
                                                                (
                                                                    src.BasvuruKamuUstlenecek.TuzelKisiTipId == null ?
                                                                    StringAddon.ToMaskedWord(src.BasvuruKamuUstlenecek.TcKimlikNo.ToString(), 3)
                                                                    :
                                                                    src.BasvuruKamuUstlenecek.TuzelKisiVergiNo
                                                                )
                                                                : default))
            .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? (
                                                                                src.Basvuru.TuzelKisiTipId == null ?
                                                                                src.Basvuru.Ad
                                                                                :
                                                                                src.Basvuru.TuzelKisiAdi
                                                                            )
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? (
                                                                                src.BasvuruKamuUstlenecek.TuzelKisiTipId == null ?
                                                                                src.BasvuruKamuUstlenecek.Ad
                                                                                :
                                                                                src.BasvuruKamuUstlenecek.TuzelKisiAdi
                                                                            )
                                                                            : default

                                                                            ))
            .ForMember(dest => dest.Soyad, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? (
                                                                                src.Basvuru.TuzelKisiTipId == null ?
                                                                                src.Basvuru.Soyad
                                                                                :
                                                                                default
                                                                            )
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? (
                                                                                src.BasvuruKamuUstlenecek.TuzelKisiTipId == null ?
                                                                                src.BasvuruKamuUstlenecek.Soyad
                                                                                :
                                                                                default
                                                                            )
                                                                            : default))
            .ForMember(dest => dest.CepTelefonu, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.CepTelefonu
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.CepTelefonu
                                                                            : default))
            .ForMember(dest => dest.BagimsizBolumNo, opt => opt.MapFrom(src => src.BagimsizBolumNo))
            .ForMember(dest => dest.AskiKodu, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.HasarTespitAskiKodu
                                                                        : default))
            .ForMember(dest => dest.DigerHibeOdemeTutar, opt => opt.MapFrom(src => src.Basvuru != null
                                                                         ? src.Basvuru.BinaDegerlendirme.BinaOdemes.Sum(x => x.DigerHibeOdemeTutari)
                                                                         : default));

        CreateMap<BasvuruImzaVeren, GetirDetayYerindeYapimKrediSozlesmeQueryResponseModel>()
            .ForMember(dest => dest.UavtIlAdi, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.UavtIlAdi
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.UavtIlAdi
                                                                            : default))
            .ForMember(dest => dest.UavtIlceAdi, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.UavtIlceAdi
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.UavtIlceAdi
                                                                            : default))
            .ForMember(dest => dest.UavtMahalleAdi, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.UavtMahalleAdi
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.UavtMahalleAdi
                                                                            : default))
            .ForMember(dest => dest.TapuAda, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.TapuAda
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.TapuAda
                                                                            : default))
            .ForMember(dest => dest.TapuParsel, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.TapuParsel
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.TapuParsel
                                                                            : default))
            .ForMember(dest => dest.Eposta, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.Eposta
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.Eposta
                                                                            : default))
            .ForMember(dest => dest.AskiKodu, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.HasarTespitAskiKodu
                                                                        : default))
            .ForMember(dest => dest.BagimsizBolumNo, opt => opt.MapFrom(src => src.BagimsizBolumNo))
            .ForMember(dest => dest.TcKimlikNo, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? (
                                                                                src.Basvuru.TuzelKisiTipId == null ?
                                                                                StringAddon.ToMaskedWord(src.Basvuru.TcKimlikNo.ToString(), 3)
                                                                                :
                                                                                src.Basvuru.TuzelKisiVergiNo
                                                                            )
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ?
                                                                            (
                                                                                src.BasvuruKamuUstlenecek.TuzelKisiTipId == null ?
                                                                                StringAddon.ToMaskedWord(src.BasvuruKamuUstlenecek.TcKimlikNo.ToString(), 3)
                                                                                :
                                                                                src.BasvuruKamuUstlenecek.TuzelKisiVergiNo
                                                                            )
                                                                            : default))
            .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? (
                                                                                src.Basvuru.TuzelKisiTipId == null ?
                                                                                src.Basvuru.Ad
                                                                                :
                                                                                src.Basvuru.TuzelKisiAdi
                                                                            )
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? (
                                                                                src.BasvuruKamuUstlenecek.TuzelKisiTipId == null ?
                                                                                src.BasvuruKamuUstlenecek.Ad
                                                                                :
                                                                                src.BasvuruKamuUstlenecek.TuzelKisiAdi
                                                                            )
                                                                            : default

                                                                            ))
            .ForMember(dest => dest.Soyad, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? (
                                                                                src.Basvuru.TuzelKisiTipId == null ?
                                                                                src.Basvuru.Soyad
                                                                                :
                                                                                default
                                                                            )
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? (
                                                                                src.BasvuruKamuUstlenecek.TuzelKisiTipId == null ?
                                                                                src.BasvuruKamuUstlenecek.Soyad
                                                                                :
                                                                                default
                                                                            )
                                                                            : default))
            .ForMember(dest => dest.CepTelefonu, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.CepTelefonu
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.CepTelefonu
                                                                            : default))
            .ForMember(dest => dest.KrediOdemeTutar, opt => opt.MapFrom(src => src.KrediOdemeTutar))
            .ForMember(dest => dest.IlkTaksitTarihi, opt => opt.MapFrom(src => src.SozlesmeTarihi));

        CreateMap<BasvuruImzaVeren, GetirDetaySozlesmeGeriOdemeBilgileriQueryResponseModel>()
           .ForMember(dest => dest.TcKimlikNo, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? StringAddon.ToMaskedWord(src.Basvuru.TcKimlikNo.ToString(), 3)
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? StringAddon.ToMaskedWord(src.BasvuruKamuUstlenecek.TcKimlikNo.ToString(), 3)
                                                                            : default))
           .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => src.Basvuru != null
                                                                       ? src.Basvuru.Ad
                                                                       : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.Ad
                                                                            : default))
           .ForMember(dest => dest.Soyad, opt => opt.MapFrom(src => src.Basvuru != null
                                                                       ? src.Basvuru.Soyad
                                                                       : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.Soyad
                                                                            : default))
           .ForMember(dest => dest.SozlesmeTarihi, opt => opt.MapFrom(src => src.SozlesmeTarihi))
           .ForMember(dest => dest.YapiKimlikNo, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.BinaDegerlendirme.YapiKimlikNo
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.BinaDegerlendirme.YapiKimlikNo
                                                                            : default))
           .ForMember(dest => dest.KrediOdemeTutar, opt => opt.MapFrom(src => src.KrediOdemeTutar))
           .ForMember(dest => dest.BinaDegerlendirmeId, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.BinaDegerlendirmeId
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.BinaDegerlendirmeId
                                                                            : default))
           .ForMember(dest => dest.IzinBelgeNo, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.BinaDegerlendirme.IzinBelgesiSayi
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.BinaDegerlendirme.IzinBelgesiSayi
                                                                            : default));

        CreateMap<BasvuruImzaVeren, BasvuruImzaVerenDto>()
            .ForMember(dest => dest.TcKimlikNo, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? StringAddon.ToMaskedWord(src.Basvuru.TcKimlikNo.ToString(), 3)
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? StringAddon.ToMaskedWord(src.BasvuruKamuUstlenecek.TcKimlikNo.ToString(), 3)
                                                                            : default))
            .ForMember(dest => dest.AdSoyad, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? string.Concat(src.Basvuru.Ad, " ", src.Basvuru.Soyad)
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? string.Concat(src.BasvuruKamuUstlenecek.Ad, " ", src.BasvuruKamuUstlenecek.Soyad)
                                                                            : default))
            .ForMember(dest => dest.BasvuruTurIdOrjinal, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.BasvuruTurId
                                                                        : src.BasvuruKamuUstlenecek != null
                                                                            ? src.BasvuruKamuUstlenecek.BasvuruTurId
                                                                            : default))
            .ForMember(dest => dest.BasvuruGuid, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.BasvuruGuid
                                                                        : default))
            .ForMember(dest => dest.BasvuruId, opt => opt.MapFrom(src => src.Basvuru != null
                                                                        ? src.Basvuru.BasvuruId
                                                                        : default))
            .ForMember(dest => dest.BasvuruKamuUstlenecekId, opt => opt.MapFrom(src => src.BasvuruKamuUstlenecek != null
                                                                        ? src.BasvuruKamuUstlenecek.BasvuruKamuUstlenecekId
                                                                        : default))
            .ForMember(dest => dest.BasvuruKamuUstlenecekGuid, opt => opt.MapFrom(src => src.BasvuruKamuUstlenecek != null
                                                                        ? src.BasvuruKamuUstlenecek.BasvuruKamuUstlenecekGuid
                                                                        : default))
            .ForMember(dest => dest.TuzelKisiAdi, opt => opt.MapFrom(src => src.BasvuruKamuUstlenecek != null
                                                                        ? src.BasvuruKamuUstlenecek.TuzelKisiAdi
                                                                        : default))
            .ForMember(dest => dest.TuzelKisiYetkiTuru, opt => opt.MapFrom(src => src.BasvuruKamuUstlenecek != null
                                                                        ? src.BasvuruKamuUstlenecek.TuzelKisiYetkiTuru
                                                                        : default))
            .ForMember(dest => dest.TuzelKisiAdres, opt => opt.MapFrom(src => src.BasvuruKamuUstlenecek != null
                                                                        ? src.BasvuruKamuUstlenecek.TuzelKisiAdres
                                                                        : default))
            .ForMember(dest => dest.TuzelKisiMersisNo, opt => opt.MapFrom(src => src.BasvuruKamuUstlenecek != null
                                                                        ? src.BasvuruKamuUstlenecek.TuzelKisiMersisNo
                                                                        : default))
            .ForMember(dest => dest.TuzelKisiTipId, opt => opt.MapFrom(src => src.BasvuruKamuUstlenecek != null
                                                                        ? src.BasvuruKamuUstlenecek.TuzelKisiTipId
                                                                        : default))
            .ForMember(dest => dest.TuzelKisiVergiNo, opt => opt.MapFrom(src => src.BasvuruKamuUstlenecek != null
                                                                        ? src.BasvuruKamuUstlenecek.TuzelKisiVergiNo
                                                                        : default))
            .ReverseMap();
    }
}