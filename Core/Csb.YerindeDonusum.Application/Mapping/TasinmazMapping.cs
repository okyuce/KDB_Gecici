using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazByTakbisTasinmazId;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirTasinmazTcDenQuery;
using Csb.YerindeDonusum.Application.Models.Takbis;

namespace Csb.YerindeDonusum.Application.Mapping;

public class TasinmazMapping : Profile
{
    public TasinmazMapping()
    {
        CreateMap<AnaTasinmazModel, GetirTasinmazByTakbisTasinmazIdQueryResponseModel>()
            .ForMember(dest => dest.Teferruat, opt => opt.MapFrom(src => src.Teferruat))
            .ForMember(dest => dest.TapuBolumDurum, opt => opt.MapFrom(src => src.TapuBolumDurum))
            .ForMember(dest => dest.DaimiMustakilHak, opt => opt.MapFrom(src => src.DaimiMustakilHak))
            .ForMember(dest => dest.BagimsizBolum, opt => opt.MapFrom(src => src.BagimsizBolum));
        //.ForMember(dest => dest.TerkinIslem, opt => opt.MapFrom(src => src.TerkinIslem))
        //.ForMember(dest => dest.TesisIslem, opt => opt.MapFrom(src => src.TesisIslem));


        CreateMap<TasinmazModel, GetirTasinmazTcDenQueryResponseModel>()
           .ForMember(dest => dest.Teferruat, opt => opt.MapFrom(src => src.Teferruat))
           .ForMember(dest => dest.TapuBolumDurum, opt => opt.MapFrom(src => src.TapuBolumDurum))
           .ForMember(dest => dest.DaimiMustakilHak, opt => opt.MapFrom(src => src.DaimiMustakilHak))
           .ForMember(dest => dest.BagimsizBolum, opt => opt.MapFrom(src => src.BagimsizBolum))
           .ForMember(dest => dest.HisseListe, opt => opt.MapFrom(src => src.HisseListe));
    }
}