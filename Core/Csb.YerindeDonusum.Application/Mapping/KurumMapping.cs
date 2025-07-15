using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class KurumMapping : Profile
{
    public KurumMapping()
    {
        CreateMap<Kurum, KurumDto>().ReverseMap();
    }
}