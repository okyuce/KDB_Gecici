using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries;
using Csb.YerindeDonusum.Domain.Entities.Kds;

namespace Csb.YerindeDonusum.Application.Mapping;

public class KdsHasarTespitVeriMapping : Profile
{
    public KdsHasarTespitVeriMapping()
    {
        CreateMap<HasartespitTespitVeri, KdsHaneDetayModel>();
        CreateMap<Hane, KdsHaneDetayModel>();
    }
}