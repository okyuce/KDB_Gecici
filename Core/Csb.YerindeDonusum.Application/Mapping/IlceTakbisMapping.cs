using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirAdaMahalleIdDenQuery;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirListeIlceByTakbisIlId;
using Csb.YerindeDonusum.Application.CQRS.TakbisCQRS.Queries.GetirParselMahalleIdAdaIdDenQuery;
using Csb.YerindeDonusum.Application.Models.Takbis;

namespace Csb.YerindeDonusum.Application.Mapping;

public class IlceTakbisMapping : Profile
{
    public IlceTakbisMapping()
    {
        CreateMap<IlceModel, GetirListeIlceByTakbisIlIdQueryResponseModel>().ReverseMap();
        CreateMap<GetirListeIlceByTakbisIlIdQueryResponseModel, IlceModel>().ReverseMap();
        CreateMap<GetirListeAdaByTakbisMahalleIdQueryResponseModel, AdaModel>().ReverseMap();
        CreateMap<GetirListeParselByTakbisMahalleIdAdaIdQueryResponseModel, ParselModel>().ReverseMap();
    }
}
