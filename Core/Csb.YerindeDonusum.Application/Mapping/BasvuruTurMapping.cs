using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BasvuruTurCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Entities;
using System.Collections.Generic;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BasvuruTurMapping : Profile
{
    public BasvuruTurMapping()
    {
        CreateMap<BasvuruTur, GetirBasvuruTurListeResponseModel>().ReverseMap();

        CreateMap<GetirBasvuruTurListeResponseModel, BasvuruTurDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BasvuruTurGuid))
            .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => src.Ad))
            .ReverseMap();

        CreateMap<GetirBasvuruTurListeResponseModel, BasvuruTurCsbDto>()
            .ForMember(dest => dest.BasvuruTurId, opt => opt.MapFrom(src => src.BasvuruTurId))
            .ForMember(dest => dest.BasvuruTurGuid, opt => opt.MapFrom(src => src.BasvuruTurGuid))
            .ForMember(dest => dest.Ad, opt => opt.MapFrom(src => src.Ad))
            .ReverseMap();

        CreateMap<ResultModel<List<GetirBasvuruTurListeResponseModel>>, ResultModel<List<BasvuruTurDto>>>();
        CreateMap<ResultModel<List<GetirBasvuruTurListeResponseModel>>, ResultModel<List<BasvuruTurCsbDto>>>();
        
    }
}