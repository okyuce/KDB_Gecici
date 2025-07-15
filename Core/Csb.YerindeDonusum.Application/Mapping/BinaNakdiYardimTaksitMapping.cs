using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirNakdiYardimTaksitler;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BinaNakdiYardimTaksitMapping : Profile
{
    public BinaNakdiYardimTaksitMapping()
    {
        CreateMap<BinaNakdiYardimTaksit, GetirNakdiYardimTaksitlerQueryResponseModel>()
            //.ForMember(dest => dest.TaksitYuzdesi, opt => opt.MapFrom(src => src.TaksitYuzdesi))
            //.ForMember(dest => dest.DosyaAdi, opt => opt.MapFrom(src => src.BinaNakdiYardimTaksitDosyas.Select(x => x.DosyaAdi).FirstOrDefault()))
            //.ForMember(dest => dest.BinaNakdiYardimTaksitDosyaId, opt => opt.MapFrom(src => src.BinaNakdiYardimTaksitDosyas.Select(x => x.BinaNakdiYardimTaksitDosyaId).FirstOrDefault()))
            //.ForMember(dest => dest.BinaNakdiYardimTaksitDosyaGuid, opt => opt.MapFrom(src => src.BinaNakdiYardimTaksitDosyas.Select(x => x.BinaNakdiYardimTaksitDosyaGuid).FirstOrDefault()))
            .ReverseMap();

    }
}
