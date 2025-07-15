using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.KaydetPasifMalikKamuUstelenecek;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BasvuruKamuUstlenecekMapping : Profile
{
    public BasvuruKamuUstlenecekMapping()
    {
        CreateMap<KaydetPasifMalikKamuUstelenecekCommandRequestModel,BasvuruKamuUstlenecek >()

            .ForMember<string>(dest => dest.TcKimlikNo, opt => opt.MapFrom(src => src.TcKimlikNoRaw!=null ? src.TcKimlikNoRaw:""))
            .ReverseMap();       
    }
}