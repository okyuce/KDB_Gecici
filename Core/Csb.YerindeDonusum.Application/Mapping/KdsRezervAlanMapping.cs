using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirRezervAlanListeAfadIcin;
using Csb.YerindeDonusum.Domain.Entities.Kds;

namespace Csb.YerindeDonusum.Application.Mapping;

public class KdsRezervAlanMapping : Profile
{
    public KdsRezervAlanMapping()
    {
        CreateMap<KdsYerindedonusumRezervAlanlar, GetirRezervAlanListeAfadIcinQueryResponseModelDetay>();
    }
}