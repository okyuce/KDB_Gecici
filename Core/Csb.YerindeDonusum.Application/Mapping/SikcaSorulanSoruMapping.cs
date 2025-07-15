using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class SikcaSorulanSoruMapping : Profile
{
    public SikcaSorulanSoruMapping()
    {
        CreateMap<SikcaSorulanSoru, SikcaSorulanSoruDto>()
            .ReverseMap();

        CreateMap<SikcaSorulanSoru, SikcaSorulanSoruServerSideDto>()
            .ReverseMap();
    }
}