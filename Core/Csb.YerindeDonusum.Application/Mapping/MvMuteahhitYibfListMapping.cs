using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.YapiDenetimSeviyeCQRS.Queries;
using Csb.YerindeDonusum.Domain.Entities.YapiDenetimSeviye;

namespace Csb.YerindeDonusum.Application.Mapping;

public class MvMuteahhitYibfListMapping : Profile
{
    public MvMuteahhitYibfListMapping()
    {
        CreateMap<MvMuteahhitYibfList, GetirYapiDenetimSeviyeByYibfNoQueryResponseModel>()
           .ForMember(dest => dest.Seviye, opt => opt.MapFrom(src => src.Seviye))
           .ReverseMap();
    }
}