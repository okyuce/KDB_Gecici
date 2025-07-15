using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

/// <summary>
/// http-s://docs.automapper.org/en/latest/Projection.html
/// </summary>
public class AydinlatmaMetniMapping : Profile
{
    public AydinlatmaMetniMapping()
    {
        CreateMap<AydinlatmaMetni, AydinlatmaMetniDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AydinlatmaMetniGuid))
            .ForMember(dest => dest.Icerik, opt => opt.MapFrom(src => src.Icerik))
            .ReverseMap();
    }
}