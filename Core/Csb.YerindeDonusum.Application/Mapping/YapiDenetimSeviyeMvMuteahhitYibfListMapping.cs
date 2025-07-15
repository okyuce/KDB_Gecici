using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.YapiDenetimSeviyeCQRS.Queries;
using Csb.YerindeDonusum.Domain.Entities.YapiDenetimSeviye;

namespace Csb.YerindeDonusum.Application.Mapping;

public class YapiDenetimSeviyeMvMuteahhitYibfListMapping : Profile
{
    public YapiDenetimSeviyeMvMuteahhitYibfListMapping()
    {
        CreateMap<MvMuteahhitYibfList, GetirYapiDenetimSeviyeByYibfNoQueryResponseModel>()
            .ReverseMap();
    }
}