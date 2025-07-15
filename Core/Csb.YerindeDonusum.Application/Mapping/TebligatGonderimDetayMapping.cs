using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.TebligatCQRS.Queries.GetirTebligatGonderimDetayById;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class TebligatGonderimDetayMapping : Profile
{
    public TebligatGonderimDetayMapping()
    {
        CreateMap<TebligatGonderimDetay, GetirTebligatGonderimDetayByIdQueryResponseModel>().ReverseMap();
    }
}