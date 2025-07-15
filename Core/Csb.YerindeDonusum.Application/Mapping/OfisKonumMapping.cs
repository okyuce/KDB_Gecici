using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.DestekTurCQRS.Queries;
using Csb.YerindeDonusum.Application.CQRS.OfisKonumCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class OfisKonumMapping : Profile
{
    public OfisKonumMapping()
    {
        CreateMap<OfisKonum, GetirOfisKonumDetayResponseModel>()
            .ForMember(dest => dest.Enlem, opt => opt.MapFrom(src => src.Konum.InteriorPoint.X))
            .ForMember(dest => dest.Boylam, opt => opt.MapFrom(src => src.Konum.InteriorPoint.Y))
            .ReverseMap();

        CreateMap<GetirOfisKonumDetayResponseModel, OfisKonumDto>().ReverseMap();

        CreateMap<ResultModel<List<GetirOfisKonumDetayResponseModel>>, ResultModel<List<OfisKonumDto>>>();
    }
}