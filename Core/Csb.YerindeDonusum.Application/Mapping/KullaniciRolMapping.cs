using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class KullaniciRolMapping : Profile
{
    public KullaniciRolMapping()
    {
        CreateMap<KullaniciRol, KullaniciRolDto>().ReverseMap();
    }
}