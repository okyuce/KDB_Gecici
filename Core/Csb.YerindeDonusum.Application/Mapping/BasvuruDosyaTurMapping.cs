using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BasvuruDosyaTurMapping : Profile
{
    public BasvuruDosyaTurMapping()
    {
        CreateMap<BasvuruDosyaTur, BasvuruDosyaTurDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BasvuruDosyaTurGuid))
            .ReverseMap();
    }
}