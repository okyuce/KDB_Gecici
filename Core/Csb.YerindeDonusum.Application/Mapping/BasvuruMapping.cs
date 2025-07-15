using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.EkleBasvuru;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruDetayById;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruListeByTcNo;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruListeServerSide;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBelediyeBasvuruListeServerSide;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Domain.Entities;
using System.Globalization;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BasvuruMapping : Profile
{
    public BasvuruMapping()
    {
        CreateMap<Basvuru, GetirBasvuruListeServerSideResponseModel>()
            .ForMember(dest => dest.OlusturanKullaniciId, opt => opt.MapFrom(src => src.OlusturanKullaniciId))
            .ForMember(dest => dest.GuncelleyenKullaniciId, opt => opt.MapFrom(src => src.GuncelleyenKullaniciId))
            .ForMember(dest => dest.SonuclandirmaAciklamasi, opt => opt.MapFrom(src => src.SonuclandirmaAciklamasi != null
                                                            ? src.SonuclandirmaAciklamasi.Replace("kamu_ustlenecek_tablosuna_aktarildi", "")
                                                            : default))
            .ForMember<string>(dest => dest.BasvuruDurumAd, opt => opt.MapFrom(src => src.BasvuruDurum.Ad))
            .ForMember<string>(dest => dest.BasvuruIptalAciklamasi, opt => opt.MapFrom(src => src.BasvuruIptalTur.Ad))
            //.ForMember<string>(dest => dest.BasvuruDegerlendirmeDurumAd, opt => opt.MapFrom(src => src.BasvuruDegerlendirmeDurum.Ad))
            .ForMember<string>(dest => dest.BasvuruAfadDurumAd, opt => opt.MapFrom(src => src.BasvuruAfadDurum.Ad))
            .ForMember<string>(dest => dest.TcKimlikNoMasked, opt => opt.MapFrom(src => StringAddon.ToMaskedWord(src.TcKimlikNo, 3)))
            .ForMember<string>(dest => dest.ListeAd,
                    opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Ad) && !string.IsNullOrWhiteSpace(src.Ad)
                                        ? string.Concat(src.Ad, " ", src.Soyad) : src.TuzelKisiAdi))
            .ForMember<string>(dest => dest.BasvuruTurAd, opt => opt.MapFrom(src => src.BasvuruTur.Ad))
            .ForMember<string>(dest => dest.BasvuruKanalAd, opt => opt.MapFrom(src => src.BasvuruKanal.Ad))
            .ForMember<string>(dest => dest.BasvuruDestekTurAd, opt => opt.MapFrom(src => src.BasvuruDestekTur.Ad))
            .ForMember(dest => dest.IptalEdilebilirMi
                , opt => opt.MapFrom(src =>
                !(src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruIptalEdildi
                    || src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                    || src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzOnaylanmistir)
            )).ForMember(dest => dest.SonuclandirilabilirMi
                , opt => opt.MapFrom(src => src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzAlinmistir))
            .ForMember<string>(dest => dest.Color
                , opt => opt.MapFrom(src =>
                    src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruIptalEdildi || src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir ? "#ffbebe" :
                        src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzOnaylanmistir ? "#64ff83" :
                        src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruReddedilmistir ? "#f14141" : ""
                )
            ); 
        CreateMap<Basvuru, GetirBelediyeBasvuruListeServerSideResponseModel>()
            .ForMember<string>(dest => dest.TcKimlikNo, opt => opt.MapFrom(src => StringAddon.ToMaskedWord(src.TcKimlikNo, 3)))
            .ForMember<string>(dest => dest.AdSoyad, opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Ad) && !string.IsNullOrWhiteSpace(src.Ad) ? string.Concat(src.Ad, " ", src.Soyad) : src.TuzelKisiAdi)
        );

        CreateMap<EkleBasvuruCommandModel, Basvuru>()
            .ForMember(x => x.BasvuruTurId, opt => opt.Ignore())
            .ForMember(x => x.AydinlatmaMetniId, opt => opt.Ignore())
            .ForMember(x => x.BasvuruKanalId, opt => opt.Ignore())
            .ForMember(x => x.BasvuruDestekTurId, opt => opt.Ignore())
            .ForMember(x => x.CepTelefonu, opt => opt.MapFrom(src => StringAddon.ToClearPhone(src.CepTelefonu)));
        //.ReverseMap();

        CreateMap<Basvuru, GetirBasvuruListeByTcNoDetayModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BasvuruGuid))
            .ForMember(dest => dest.BasvuruDurumu, opt => opt.MapFrom(src => src.BasvuruDurum.Ad))
            .ForMember(dest => dest.BasvuruTuru, opt => opt.MapFrom(src => src.BasvuruTur.Ad))
            .ForMember(dest => dest.BasvuruDestekTuru, opt => opt.MapFrom(src => src.BasvuruDestekTur.Ad))
            .ForMember(dest => dest.BasvuruKanali, opt => opt.MapFrom(src => src.BasvuruKanal.Ad))
            .ForMember(dest => dest.IptalEdilebilirMi
                , opt => opt.MapFrom(src =>
                (src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruIptalEdildi
                    || src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                    || src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzOnaylanmistir
                ) ? false : true))
            //.ForMember(dest => dest.DosyaBasvuruListe, opt => opt.MapFrom(src => src.BasvuruDosyas.Select(s => new DosyaIceriksizDto { Id = s.BasvuruDosyaGuid, DosyaAdi = s.DosyaAdi, DosyaTurId = s.BasvuruDosyaTurId })))
            .ForMember(dest => dest.OlusturmaTarihi, opt => opt.MapFrom(src => src.OlusturmaTarihi.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.IlAdi, opt => opt.MapFrom(src => src.TapuIlAdi))
            .ForMember(dest => dest.IlceAdi, opt => opt.MapFrom(src => src.TapuIlceAdi))
            .ForMember(dest => dest.MahalleAdi, opt => opt.MapFrom(src => src.TapuMahalleAdi))
            .ForMember(dest => dest.Blok, opt => opt.MapFrom(src => string.Concat(
                string.IsNullOrEmpty(src.TapuBagimsizBolumNo) ? "-" : src.TapuBagimsizBolumNo,
                " / ",
                src.TapuKat == null ? "-" : src.TapuKat,
                " / ",
                string.IsNullOrEmpty(src.TapuBlok) ? "-" : src.TapuBlok,
                " / ",
                string.IsNullOrEmpty(src.TapuGirisBilgisi) ? "-" : src.TapuGirisBilgisi)
                )
            );
        //.ReverseMap();

        CreateMap<Basvuru, GetirBasvuruDetayByIdQueryResponseModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BasvuruGuid))
            .ForMember(dest => dest.BasvuruDurumu, opt => opt.MapFrom(src => src.BasvuruDurum.Ad))
            .ForMember(dest => dest.BasvuruIptalAciklamasi, opt => opt.MapFrom(src => src.BasvuruIptalTur.Ad))
            .ForMember(dest => dest.BasvuruTuru, opt => opt.MapFrom(src => src.BasvuruTur.Ad))
            .ForMember(dest => dest.BasvuruDestekTuru, opt => opt.MapFrom(src => src.BasvuruDestekTur.Ad))
            .ForMember(dest => dest.BasvuruKanali, opt => opt.MapFrom(src => src.BasvuruKanal.Ad))
            .ForMember(dest => dest.IptalEdilebilirMi
                , opt => opt.MapFrom(src =>
                (src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruIptalEdildi
                    || src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                    || src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzOnaylanmistir
                ) ? false : true))
            .ForMember(dest => dest.DosyaBasvuruListe, opt => opt.MapFrom(src => src.BasvuruDosyas.Select(s => new DosyaIceriksizDto { Id = s.BasvuruDosyaGuid, DosyaAdi = s.DosyaAdi, DosyaTurId = s.BasvuruDosyaTur.BasvuruDosyaTurGuid, DosyaTurAdi = s.BasvuruDosyaTur.Ad })))
            .ForMember(dest => dest.OlusturmaTarihi, opt => opt.MapFrom(src => src.OlusturmaTarihi.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.TapuBlok, opt => opt.MapFrom(src => string.Concat(
                string.IsNullOrEmpty(src.TapuBagimsizBolumNo) ? "-" : src.TapuBagimsizBolumNo,
                " / ",
                src.TapuKat == null ? "-" : src.TapuKat,
                " / ",
                string.IsNullOrEmpty(src.TapuBlok) ? "-" : src.TapuBlok,
                " / ",
                string.IsNullOrEmpty(src.TapuGirisBilgisi) ? "-" : src.TapuGirisBilgisi)
                )
            )
            .ForMember(dest => dest.UavtBeyanMi, opt => opt.MapFrom(src => src.UavtBeyanMi));
        //.ReverseMap();

        CreateMap<Basvuru, GetirAfadIcinBasvuruDetayDto>()
            .ForMember(dest => dest.BasvuruDurumu, opt => opt.MapFrom(src => src.BasvuruDurum.Ad))
            .ForMember(dest => dest.BasvuruKanali, opt => opt.MapFrom(src => src.BasvuruKanal.Ad))
            .ForMember(dest => dest.IlAdi, opt => opt.MapFrom(src => src.UavtIlAdi))
            .ForMember(dest => dest.IlceAdi, opt => opt.MapFrom(src => src.UavtIlceAdi))
            .ForMember(dest => dest.MahalleAdi, opt => opt.MapFrom(src => src.UavtMahalleAdi))
            .ForMember(dest => dest.BasvuruTurAdi, opt => opt.MapFrom(src => src.BasvuruTur.Ad))
            .ForMember(dest => dest.MalikUstlenecekMi, opt => opt.MapFrom(src => src.BasvuruImzaVerens.Any(x => x.KrediOdemeTutar == 0 && x.HibeOdemeTutar == 0 && x.AktifMi == true && !x.SilindiMi)))
            .ForMember(dest => dest.GuncellemeTarihi, opt => opt.MapFrom(src =>
                src.BinaDegerlendirme.BinaDegerlendirmeId == null
                    ? src.GuncellemeTarihi
                    : (
                        src.GuncellemeTarihi == null
                            ? src.BinaDegerlendirme.GuncellemeTarihi ?? src.BinaDegerlendirme.OlusturmaTarihi
                            : (
                                src.GuncellemeTarihi > (src.BinaDegerlendirme.GuncellemeTarihi ?? src.BinaDegerlendirme.OlusturmaTarihi)
                                    ? src.GuncellemeTarihi
                                    : src.BinaDegerlendirme.GuncellemeTarihi ?? src.BinaDegerlendirme.OlusturmaTarihi
                            )
                    )
            ))
            .ForMember(dest => dest.BinaDegerlendirmeDurumId, opt => opt.MapFrom(src => src.BinaDegerlendirme.BinaDegerlendirmeDurumId))
            .ForMember(dest => dest.BinaDegerlendirmeDurumAdi, opt => opt.MapFrom(src => src.BinaDegerlendirme.BinaDegerlendirmeDurum.Ad))
            .ForMember(dest => dest.BinaDegerlendirmeYapiRuhsatGirildiMi, opt => opt.MapFrom(src => src.BinaDegerlendirme.BultenNo > 0 || src.BinaDegerlendirme.BinaYapiRuhsatIzinDosyas.Any(x => x.AktifMi == true && !x.SilindiMi)));
    }
}