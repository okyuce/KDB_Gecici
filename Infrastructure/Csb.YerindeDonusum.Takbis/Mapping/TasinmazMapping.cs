using AutoMapper;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Models.Takbis;
using Takbis;

namespace Csb.YerindeDonusum.Takbis.Mapping;

public class TasinmazMapping : Profile
{
    public TasinmazMapping()
    {

        CreateMap<Islem, IslemModel>().ReverseMap();
        CreateMap<BagimsizBolum, BagimsizBolumModel>().ReverseMap();

        CreateMap<Tasinmaz, TasinmazModel>()
            .ForMember(dest => dest.TapuBolumDurum, opt => opt.MapFrom(src => src.TapuBolumDurum))
            .ForMember(dest => dest.DaimiMustakilHak, opt => opt.MapFrom(src => src.DaimiMustakilHak))
            .ForMember(dest => dest.BagimsizBolum, opt => opt.MapFrom(src => src.BagimsizBolum))
            .ForMember(dest => dest.TerkinIslem, opt => opt.MapFrom(src => src.TerkinIslem))
            .ForMember(dest => dest.TesisIslem, opt => opt.MapFrom(src => src.TesisIslem))
            .ForMember(dest => dest.BagimsizBolum, opt => opt.MapFrom(src => src.BagimsizBolum)).ReverseMap();

        CreateMap<Tasinmaz, AnaTasinmazModel>()
            .ForMember(dest => dest.TapuBolumDurum, opt => opt.MapFrom(src => src.TapuBolumDurum))
            .ForMember(dest => dest.DaimiMustakilHak, opt => opt.MapFrom(src => src.DaimiMustakilHak))
            .ForMember(dest => dest.BagimsizBolum, opt => opt.MapFrom(src => src.BagimsizBolum))
            .ForMember(dest => dest.TerkinIslem, opt => opt.MapFrom(src => src.TerkinIslem))
            .ForMember(dest => dest.TesisIslem, opt => opt.MapFrom(src => src.TesisIslem))
            .ForMember(dest => dest.BagimsizBolum, opt => opt.MapFrom(src => src.BagimsizBolum));

        CreateMap<DaimiMustakilHakModel, DaimiMustakilHak>().ReverseMap();
        CreateMap<DurumEnum, Durum>().ReverseMap();

        CreateMap<Mahalle, MahalleModel>().ReverseMap();

        CreateMap<Ilce, IlceModel>().ReverseMap();
    }
}
