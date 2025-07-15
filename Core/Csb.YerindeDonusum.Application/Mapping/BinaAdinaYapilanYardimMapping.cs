using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaIcinYapilanDigerYardimlar;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BinaAdinaYapilanYardimMapping : Profile
{
    public BinaAdinaYapilanYardimMapping()
    {
        CreateMap<BinaAdinaYapilanYardim, GetirBinaIcinYapilanDigerYardimlarQueryResponseModel>()
            .ForMember(dest => dest.Adi, opt => opt.MapFrom(src => src.BinaAdinaYapilanYardimTipi.Adi))
            .ForMember(dest => dest.Tarih, opt => opt.MapFrom(src => src.Tarih))
            .ForMember(dest => dest.Tutar, opt => opt.MapFrom(src => src.Tutar))
            .ReverseMap();
    }
}