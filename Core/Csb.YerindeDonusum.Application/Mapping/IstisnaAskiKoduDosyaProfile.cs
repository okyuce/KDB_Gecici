using AutoMapper;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Mapping
{
    public class IstisnaAskiKoduDosyaProfile : Profile
    {
        public IstisnaAskiKoduDosyaProfile()
        {
            CreateMap<IstisnaAskiKoduDosya, IstisnaAskiKoduDosyaDto>();
            CreateMap<IstisnaAskiKoduDosyaCreateDto, IstisnaAskiKoduDosya>();
            CreateMap<IstisnaAskiKoduDosyaUpdateDto, IstisnaAskiKoduDosya>();
        }
    }
}
