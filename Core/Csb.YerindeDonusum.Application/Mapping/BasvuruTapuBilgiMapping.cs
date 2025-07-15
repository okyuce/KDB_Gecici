using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BasvuruTapuBilgiMapping : Profile
{
    public BasvuruTapuBilgiMapping()
    {
        CreateMap<BasvuruTapuBilgi, BasvuruTapuBilgiDto>();

        CreateMap<BasvuruTapuBilgiDto, BasvuruTapuBilgi>()
            .ForMember(x => x.HissePay, opt => opt.MapFrom(src => src.HissePay > 0 ? src.HissePay : src.Pay))
            .ForMember(x => x.HissePayda, opt => opt.MapFrom(src => src.HissePayda > 0 ? src.HissePayda : src.Payda)); ;
    }
}