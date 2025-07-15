using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeAnaTasinmaz;
using Csb.YerindeDonusum.Application.Models.Takbis;

namespace Csb.YerindeDonusum.Application.Mapping;

public class AnaTasinmazMapping : Profile
{
	public AnaTasinmazMapping()
	{
		CreateMap<AnaTasinmazModel, GetirListeAnaTasinmazQueryResponseModel>().ReverseMap();
	}
}