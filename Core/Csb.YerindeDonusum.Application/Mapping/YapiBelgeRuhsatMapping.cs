using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.YapiBelgeCQRS.Queries.GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNo;
using Csb.YerindeDonusum.Application.CQRS.YapiBelgeCQRS.Queries.GetirYapiBelgeByYapiKimlikNo;
using Csb.YerindeDonusum.Application.Models.NVIYapiBelge.YapiRuhsatOku;
using NVIYapiBelgeSorgulamaService;

namespace Csb.YerindeDonusum.Application.Mapping;

public class YapiBelgeRuhsatMapping : Profile
{
    public YapiBelgeRuhsatMapping()
    {
        CreateMap<YapiRuhsatBilgi, GetirYapiBelgeRuhsatByBultenNoQueryResponseModel>();
        CreateMap<GetirYapiBelgeRuhsatByBultenNoQueryResponseModel, GetirBinaDegerlendirmeYapiBelgeRuhsatByBultenNoQueryResponseModel>();
    }
}