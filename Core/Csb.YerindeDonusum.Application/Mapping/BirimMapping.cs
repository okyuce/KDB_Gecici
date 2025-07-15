using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BirimMapping : Profile
{
    public BirimMapping()
    {
        CreateMap<Birim, BirimDto>().ReverseMap();
    }
}