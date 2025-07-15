using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeMahalleByTakbisIlceIdQuery;
using Csb.YerindeDonusum.Application.Models.Takbis;

namespace Csb.YerindeDonusum.Application.Mapping;

public class MahalleTakbisMapping : Profile
{
	public MahalleTakbisMapping()
	{
		CreateMap<MahalleModel, GetirListeMahalleByTakbisIlceIdQueryResponseModel>().ReverseMap();
	}
}
