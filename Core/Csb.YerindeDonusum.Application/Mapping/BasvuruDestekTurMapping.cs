using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.DestekTurCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BasvuruDestekTurMapping : Profile
{
    public BasvuruDestekTurMapping()
    {
        CreateMap<BasvuruDestekTur, GetirBasvuruDestekTurListeResponseModel>().ReverseMap();

        CreateMap<GetirBasvuruDestekTurListeResponseModel, BasvuruDestekTurDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BasvuruDestekTurGuid))
            .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => src.Ad))
            .ReverseMap();

        CreateMap<GetirBasvuruDestekTurListeResponseModel, BasvuruDestekTurCsbDto>()
            .ForMember(dest => dest.BasvuruDestekTurId, opt => opt.MapFrom(src => src.BasvuruDestekTurId))
            .ForMember(dest => dest.BasvuruDestekTurGuid, opt => opt.MapFrom(src => src.BasvuruDestekTurGuid))
            .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => src.Ad))
            .ReverseMap();

        CreateMap<ResultModel<List<GetirBasvuruDestekTurListeResponseModel>>, ResultModel<List<BasvuruDestekTurDto>>>();
        CreateMap<ResultModel<List<GetirBasvuruDestekTurListeResponseModel>>, ResultModel<List<BasvuruDestekTurCsbDto>>>();
    }
}