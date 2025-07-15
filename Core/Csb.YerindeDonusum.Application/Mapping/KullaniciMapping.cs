using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Commands.KullaniciGiris;
using Csb.YerindeDonusum.Application.CQRS.HesapCQRS.Queries;
using Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS.Commands;
using Csb.YerindeDonusum.Application.CQRS.KullaniciCQRS.Commands.EkleKullanici;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class KullaniciMapping : Profile
{
    public KullaniciMapping()
    {
        CreateMap<GetirKullaniciGirisQuery, KullaniciGirisDto>()
            .ForMember(dest => dest.KullaniciAdi, opt => opt.MapFrom(src => src.KullaniciAdi))
            .ForMember(dest => dest.Sifre, opt => opt.MapFrom(src => src.Sifre))
            .ReverseMap();

        CreateMap<KullaniciGirisCommand, KullaniciGirisDto>()
            .ForMember(dest => dest.KullaniciAdi, opt => opt.MapFrom(src => src.KullaniciAdi))
            .ForMember(dest => dest.Sifre, opt => opt.MapFrom(src => src.Sifre))
            .ReverseMap();

        CreateMap<EkleKullaniciCommand, KullaniciDto>()
            .ReverseMap();

        CreateMap<EkleKullaniciCommand, Kullanici>().ReverseMap();

        CreateMap<GuncelleKullaniciCommand, Kullanici>().ReverseMap();

        CreateMap<GuncelleKullaniciCommand, KullaniciDto>().ReverseMap();

        CreateMap<Kullanici, KullaniciDto>()
             .ForMember(dest => dest.SecilenRolIdList, opt => opt.MapFrom(src => src.KullaniciRols.Select(x => x.RolId).ToArray()))
             .ForMember(dest => dest.SifreZamanAsimiMi, opt => opt.MapFrom(src => src.KullaniciHesapTipId == (long)Enums.KullaniciHesapTipEnum.Local &&  src.SonSifreDegisimTarihi < DateTime.Now))
            .ReverseMap();

        CreateMap<Kullanici, KullaniciListeDto>()
            .ForMember(dest => dest.KullaniciRolAdListe, opt => opt.MapFrom(src => src.KullaniciRols.Select(s => s.Rol.Ad)))
            .ForMember(dest => dest.BirimAdi, opt => opt.MapFrom(src => src.Birim.Ad))
            .ForMember(dest => dest.BirimId, opt => opt.MapFrom(src => src.Birim.BirimId.ToString()))
            .ForMember(dest => dest.KullaniciHesapTipAdi, opt => opt.MapFrom(src => src.KullaniciHesapTip.Ad))
            .ForMember(dest => dest.TcKimlikNo, opt => opt.MapFrom(src => StringAddon.ToMaskedWord(src.TcKimlikNo, 3)))
            .ForMember(dest => dest.CepTelefonu, opt => opt.MapFrom(src => StringAddon.ToClearPhone(src.CepTelefonu)))
            ;
    }
}