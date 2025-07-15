using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirYapiDenetimSeviyeTespitBilgiler;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BinaYapiDenetimSeviyeTespitMapping : Profile
{
    public BinaYapiDenetimSeviyeTespitMapping()
    {
        CreateMap<BinaYapiDenetimSeviyeTespit, GetirYapiDenetimSeviyeTespitBilgilerQueryResponseModel>()
            .ForMember(dest => dest.IlerlemeYuzdesi, opt => opt.MapFrom(src => src.IlerlemeYuzdesi))
            .ForMember(dest => dest.DosyaAdi, opt => opt.MapFrom(src => src.BinaYapiDenetimSeviyeTespitDosyas.Select(x => x.DosyaAdi).FirstOrDefault()))
            .ForMember(dest => dest.BinaYapiDenetimSeviyeTespitDosyaId, opt => opt.MapFrom(src => src.BinaYapiDenetimSeviyeTespitDosyas.Select(x => x.BinaYapiDenetimSeviyeTespitDosyaId).FirstOrDefault()))
            .ForMember(dest => dest.BinaYapiDenetimDosyaGuid, opt => opt.MapFrom(src => src.BinaYapiDenetimSeviyeTespitDosyas.Select(x => x.BinaYapiDenetimSeviyeTespitDosyaGuid).FirstOrDefault()))
            .ReverseMap();
    }
}
