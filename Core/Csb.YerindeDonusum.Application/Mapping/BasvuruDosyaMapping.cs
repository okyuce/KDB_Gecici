using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BasvuruDosyaCQRS.Queries;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BasvuruDosyaMapping : Profile
{
    public BasvuruDosyaMapping()
    {
        CreateMap<BasvuruDosya, GetirDosyaListeByBasvuruIdQueryResponseModel>().ReverseMap();
    }
}