using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using Csb.YerindeDonusum.Domain.Entities.Maks;
using System.Globalization;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BoyutMapping : Profile
{
    public BoyutMapping()
    {
        CreateMap<TblIl, BoyutKonumDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IlKod))
            .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => src.Ad.ToUpper(new CultureInfo("tr-TR", false))));

        CreateMap<TblIlce, BoyutKonumDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IlceKod))
            .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => src.Ad.ToUpper(new CultureInfo("tr-TR", false))));

        CreateMap<TblMahalle, BoyutKonumDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MahalleKod))
            .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => string.Concat(src.KoyKodNavigation.Ad.ToLower() == "merkez" ? "" : $"{src.KoyKodNavigation.Ad} - ", src.Ad).ToUpper(new CultureInfo("tr-TR", false))));

        CreateMap<TblCsbm, BoyutKonumDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CsbmKod))
            .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => $"{src.Ad}{(src.TipKodNavigation != null ? string.Concat(" ", src.TipKodNavigation.Ad) : "")}".ToUpper(new CultureInfo("tr-TR", false))));

        CreateMap<TblNumarataj, BoyutKonumDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.NumaratajKod))
            .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.KapiNo) ? "-" : src.KapiNo.ToUpper(new CultureInfo("tr-TR", false))));

        CreateMap<TblBagimsizBolum, BoyutKonumDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BagimsizBolumKod))
            .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.IcKapiNo) ? "-" : src.IcKapiNo.ToUpper(new CultureInfo("tr-TR", false))));

        CreateMap<TopluNumarataj, BoyutKonumDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.NumaratajNo))
           .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Kapino) ? "-" : src.Kapino.ToUpper(new CultureInfo("tr-TR", false))));

        CreateMap<TopluBagimsizbolum, BagimsizBolumDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BagNo))
           .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Bagimsizbolumno) ? "-" : src.Bagimsizbolumno.ToUpper(new CultureInfo("tr-TR", false))));
    }
}