using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BilgilendirmeMesajCQRS.Queries.GetirBilgilendirmeMesajById;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BilgilendirmeMesajMapping : Profile
{
    public BilgilendirmeMesajMapping()
    {
        CreateMap<BilgilendirmeMesaj, GetirBilgilendirmeMesajByIdQueryResponseModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BilgilendirmeMesajId))
            .ForMember(dest => dest.Anahtar, opt => opt.MapFrom(src => src.Anahtar))
            .ForMember(dest => dest.Deger, opt => opt.MapFrom(src => src.Deger))
            .ReverseMap();
    }
}
