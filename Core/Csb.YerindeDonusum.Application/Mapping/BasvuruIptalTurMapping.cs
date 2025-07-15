using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BasvuruIptalTurCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BasvuruIptalTurMapping : Profile
{
    public BasvuruIptalTurMapping()
    {
        CreateMap<BasvuruIptalTur, GetirBasvuruIptalTurListeResponseModel>().ReverseMap();

        CreateMap<GetirBasvuruIptalTurListeResponseModel, BasvuruIptalTurDto>()
            .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => src.Ad))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BasvuruIptalTurId))
            .ReverseMap();

        CreateMap<ResultModel<List<GetirBasvuruIptalTurListeResponseModel>>, ResultModel<List<BasvuruIptalTurDto>>>();
    }
}