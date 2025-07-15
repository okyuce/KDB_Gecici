using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class RolMapping : Profile
{
    public RolMapping()
    {
        CreateMap<Rol, RolDto>().ReverseMap();
    }
}