using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BinaAdinaYapilanYardimTipiMapping : Profile
{
    public BinaAdinaYapilanYardimTipiMapping()
    {
        CreateMap<BinaAdinaYapilanYardimTipi, BinaAdinaYapilanYardimTipiDto>().ReverseMap();
    }
}