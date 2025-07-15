using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Queries.GetirBasvuruImzaByBasvuruId;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class DosyaMapping : Profile
{
    public DosyaMapping()
    {
        CreateMap<BasvuruImzaVerenDosya, DosyaIceriksizDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BasvuruImzaVerenDosyaGuid))
            .ForMember(dest => dest.DbId, opt => opt.MapFrom(src => src.BasvuruImzaVerenId))
            .ForMember(dest => dest.DosyaTurAdi, opt => opt.MapFrom(src => src.BasvuruImzaVerenDosyaTur.Ad))
            .ForMember(dest => dest.DosyaTurDbId, opt => opt.MapFrom(src => src.BasvuruImzaVerenDosyaTurId)).ReverseMap();
    }
}