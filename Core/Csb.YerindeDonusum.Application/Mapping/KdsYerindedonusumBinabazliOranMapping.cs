using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.GetirListeBinabazliOranServerSide;
using Csb.YerindeDonusum.Domain.Entities.Kds;

namespace Csb.YerindeDonusum.Application.Mapping;

public class KdsYerindedonusumBinabazliOranMapping : Profile
{
    public KdsYerindedonusumBinabazliOranMapping()
    {
        CreateMap<KdsYerindedonusumBinabazliOran, GetirListeBinabazliOranServerSideQueryResponseModel>()
            .ForMember(dest => dest.Oran, opt => opt.MapFrom(src => src.Oran== null ? 0 : Math.Round(src.Oran.Value, 2)));
    }
}