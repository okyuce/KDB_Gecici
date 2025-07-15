using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeBinaDegerlendirmeServerSide;
using Csb.YerindeDonusum.Domain.Entities.Kds;

namespace Csb.YerindeDonusum.Application.Mapping;

public class KdsYerindedonusumBasvuruMuteahhitListesiMapping : Profile
{
    public KdsYerindedonusumBasvuruMuteahhitListesiMapping()
    {
        CreateMap<KdsYerindedonusumBinabazliOran, GetirListeBinaDegerlendirmeServerSideQueryResponseModel>()
            .ForMember(dest => dest.Oran, opt => opt.MapFrom(src => src.Oran == null ? 0 : Math.Round(src.Oran.Value, 2)));
    }
}