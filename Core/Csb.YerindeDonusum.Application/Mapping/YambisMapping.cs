using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.YambisCQRS.Queries.GetirYambisYetkiBelgeNo;
using Csb.YerindeDonusum.Application.Models.Santiyem;

namespace Csb.YerindeDonusum.Application.Mapping;

public class YambisMapping : Profile
{
    public YambisMapping()
    {
        CreateMap<YetkiBelgesiBilgi, GetirYambisYetkiBelgeNoQueryResponseModel>().ReverseMap();
    }
}