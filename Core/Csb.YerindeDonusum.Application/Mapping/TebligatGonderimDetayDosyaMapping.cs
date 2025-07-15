using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class TebligatGonderimDetayDosyaMapping : Profile
{
    public TebligatGonderimDetayDosyaMapping()
    {
        CreateMap<TebligatGonderimDetayDosya, TebligatGonderimDetayDosyaDto>().ReverseMap();
    }
}
