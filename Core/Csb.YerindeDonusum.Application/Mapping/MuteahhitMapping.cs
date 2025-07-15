using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.MuteahhitCQRS.Queries.GetirYetkiBelgeNoIle;
using Csb.YerindeDonusum.Application.Models.Santiyem;

namespace Csb.YerindeDonusum.Application.Mapping;

public class MuteahhitMapping : Profile
{
    public MuteahhitMapping()
    {
        CreateMap<GetirYetkiBelgeNoIleQueryResponseModel, YetkiBelgesiBilgi>()
            .ForMember(dest => dest.CepTelefon, opt => opt.MapFrom(src => src.CepTelefonu))
            .ReverseMap();
        //CreateMap<GetirMuteahhitBilgileriQueryResponseModel, BasvuruDegerlendirme>()
        //    .ForMember(dest => dest.Adsoyadunvan, opt => opt.MapFrom(src => src.AdSoyadUnvan))
        //    .ReverseMap();
    }
}