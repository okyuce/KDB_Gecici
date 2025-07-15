using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;
using Newtonsoft.Json;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Csb.YerindeDonusum.Application.Mapping;

public class AyarMapping : Profile
{
    public AyarMapping()
    {
        CreateMap<Ayar, AyarDto>()
            .ConvertUsing((source) => JsonConvert.DeserializeObject<AyarDto>(source.Deger));
    }
}