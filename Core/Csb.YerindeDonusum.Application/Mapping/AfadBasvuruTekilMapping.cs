using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirAfadBasvuruById;
using Csb.YerindeDonusum.Application.CQRS.AfadBasvuruCQRS.Queries.GetirListeAfadBasvuruServerSide;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class AfadBasvuruTekilMapping : Profile
{
    public AfadBasvuruTekilMapping()
    {
        CreateMap<GetirAfadTopluBasvuruDto, AfadBasvuruTekil>()
            .ForMember(dest => dest.CsbAktifMi, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CsbSilindiMi, opt => opt.MapFrom(src => false))
            .ReverseMap();

        CreateMap<AfadBasvuruTekil, GetirAfadBasvuruByIdQueryResponseModel>()
            .ForMember<string>(dest => dest.Tckn, opt => opt.MapFrom(src => StringAddon.ToMaskedWord(src.Tckn, 3)))
            .ForMember<string>(dest => dest.EbeveynTckn, opt => opt.MapFrom(src => StringAddon.ToMaskedWord(src.EbeveynTckn, 3)))
            .ForMember<string>(dest => dest.Telefon, opt => opt.MapFrom(src => StringAddon.ToClearPhone(src.Telefon)));

        CreateMap<AfadBasvuruTekil, GetirListeAfadBasvuruServerSideQueryResponseModel>()
            .ForMember<string>(dest => dest.Tckn, opt => opt.MapFrom(src => StringAddon.ToMaskedWord(src.Tckn, 3)))
            .ForMember<string>(dest => dest.Telefon, opt => opt.MapFrom(src => StringAddon.ToClearPhone(src.Telefon)));

        CreateMap<AfadBasvuruTekil, AfadBasvuru>()
            .ReverseMap();
    }
}