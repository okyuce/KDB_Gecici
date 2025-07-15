using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class KullaniciHesapTipMapping : Profile
{
    public KullaniciHesapTipMapping()
    {
        CreateMap<KullaniciHesapTip, KullaniciHesapTipDto>().ReverseMap();
    }
}