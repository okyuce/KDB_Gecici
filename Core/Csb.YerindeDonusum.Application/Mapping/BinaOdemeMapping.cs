using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeOdemeBekleyenDegerlendirmeServerSide;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirNakdiYardimTaksitler;
using Csb.YerindeDonusum.Application.CQRS.BinaOdemeCQRS.Queries.GetirListeBinaOdemeServerSide;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Domain.Entities;
using System.Globalization;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BinaOdemeMapping : Profile
{
    public BinaOdemeMapping()
    {
        CreateMap<BinaOdeme, GetirNakdiYardimTaksitlerQueryResponseModel>()
            .ForMember(dest => dest.BinaOdemeDurumAd, opt => opt.MapFrom(src => src.BinaOdemeDurum.Ad))
            .ForMember(dest => dest.Seviye, opt => opt.MapFrom(src => src.Seviye))
            .ForMember(dest => dest.FenniMesulTc, opt => opt.MapFrom(src => src.BinaDegerlendirme.FenniMesulTc))
            .ForMember(dest => dest.YibfNo, opt => opt.MapFrom(src => src.BinaDegerlendirme.YibfNo))
            .ForMember(dest => dest.MuteahhitIbanNo, opt => opt.MapFrom(src => src.BinaDegerlendirme.BinaMuteahhits.Select(x => x.IbanNo).FirstOrDefault().Replace("-", "")))
            .ForMember(dest => dest.MuteahhitYetkiBelgeNo, opt => opt.MapFrom(src => src.BinaDegerlendirme.BinaMuteahhits.Select(x => x.YetkiBelgeNo).FirstOrDefault()))
            .ForMember(dest => dest.IzinBelgesiSayi, opt => opt.MapFrom(src => src.BinaDegerlendirme.IzinBelgesiSayi))
            .ForMember(dest => dest.YapiKimlikNo, opt => opt.MapFrom(src => src.BinaDegerlendirme.YapiKimlikNo))
         .ReverseMap();

        CreateMap<BinaOdeme, BinaOdemeDto>()
            .ForMember(dest => dest.BinaOdemeDurumAd, opt => opt.MapFrom(src => src.BinaOdemeDurum.Ad))
            .ForMember(dest => dest.Seviye, opt => opt.MapFrom(src => src.Seviye))
            .ForMember(dest => dest.UavtIlNo, opt => opt.MapFrom(src => src.BinaDegerlendirme.UavtIlNo))
            .ForMember(dest => dest.UavtIlAdi, opt => opt.MapFrom(src => src.BinaDegerlendirme.UavtIlAdi))
            .ForMember(dest => dest.UavtIlceNo, opt => opt.MapFrom(src => src.BinaDegerlendirme.UavtIlceNo))
            .ForMember(dest => dest.UavtIlceAdi, opt => opt.MapFrom(src => src.BinaDegerlendirme.UavtIlceAdi))
            .ForMember(dest => dest.UavtMahalleNo, opt => opt.MapFrom(src => src.BinaDegerlendirme.UavtMahalleNo))
            .ForMember(dest => dest.UavtMahalleAdi, opt => opt.MapFrom(src => src.BinaDegerlendirme.UavtMahalleAdi))
            .ForMember(dest => dest.OdemeIslemleriButonGoster, opt => opt.MapFrom(src => src.BinaOdemeDurumId == (int)BinaOdemeDurumEnum.HYSAktarildi))
            .ForMember(dest => dest.HasarTespitAskiKodu, opt => opt.MapFrom(src => src.BinaDegerlendirme.HasarTespitAskiKodu))
            .ForMember(dest => dest.OlusturanKullaniciAdi, opt => opt.MapFrom(src => src.OlusturanKullanici.KullaniciAdi))
            .ForMember(dest => dest.GuncelleyenKullaniciAdi, opt => opt.MapFrom(src => src.GuncelleyenKullanici.KullaniciAdi))
         .ReverseMap();
    }
}