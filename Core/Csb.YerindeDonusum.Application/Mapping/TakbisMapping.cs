using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirAlanByTakbisTasinmazIdQuery;
using Csb.YerindeDonusum.Application.Models.Takbis;

namespace Csb.YerindeDonusum.Application.Mapping;

public class TakbisMapping : Profile
{
	public TakbisMapping()
	{
		CreateMap<AlanModel, GetirAlanByTasinmazIdQueryResponseModel>().ReverseMap();
    }
}
