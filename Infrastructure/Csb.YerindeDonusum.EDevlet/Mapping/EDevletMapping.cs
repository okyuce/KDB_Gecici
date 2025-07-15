using AutoMapper;
using Csb.YerindeDonusum.Application.Models;
using EDevletBildirimService;

namespace Csb.YerindeDonusum.Takbis.Mapping;

public class EDevletMapping : Profile
{
    public EDevletMapping()
    {
        CreateMap<sendMessageOutputType, EDevletTabligatResult>()
            .ForMember(dest => dest.TakipIdList, opt => opt.MapFrom(src => src.trackingList.ToList()))
            .ForMember(dest => dest.SonucKodu, opt => opt.MapFrom(src => src.returnCode))
            .ForMember(dest => dest.SonucAciklamasi, opt => opt.MapFrom(src => src.returnMessage));

        //CreateMap<IstirakTebligatGonderCommandModel, msgParameterItemType>()
        //    .ForMember(dest => dest.trIdentityNo, opt => opt.MapFrom(src => src.TCKimlikNo))
        //    .ForMember(dest => dest.corporationExternalId, opt => opt.MapFrom(src => src.GonderimId));

    }
}
