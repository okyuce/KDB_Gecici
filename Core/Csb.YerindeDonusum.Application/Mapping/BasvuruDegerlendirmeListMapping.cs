using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeMalikler;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Domain.Entities;
using System.Runtime.Intrinsics.X86;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BasvuruDegerlendirmeListMapping : Profile
{
    public BasvuruDegerlendirmeListMapping()
    {
        CreateMap<Basvuru, GetirListeMaliklerQueryResponseModel>()
            .ForMember(dest => dest.BasvuruGuid, opt => opt.MapFrom(src => src.BasvuruGuid))
            .ForMember(dest => dest.SonuclandirmaAciklamasi, opt => opt.MapFrom(src => src.SonuclandirmaAciklamasi != null
                                                                                    ? src.SonuclandirmaAciklamasi.Replace("kamu_ustlenecek_tablosuna_aktarildi", "")
                                                                                    : default))
            .ForMember<string>(dest => dest.BinaDisKapiNo, opt => opt.MapFrom(src => src.BinaDegerlendirme.BinaDisKapiNo))
            .ForMember<string>(dest => dest.BasvuruDurumAd, opt => opt.MapFrom(src => src.BasvuruDurum.Ad))
            .ForMember<string>(dest => dest.BasvuruDurumAd, opt => opt.MapFrom(src => src.BasvuruDurum.Ad))
            .ForMember<string>(dest => dest.BasvuruDegerlendirmeDurumAd, opt => opt.MapFrom(src => src.BinaDegerlendirme.BinaDegerlendirmeDurum.Ad))
            .ForMember<string>(dest => dest.BasvuruAfadDurumAd, opt => opt.MapFrom(src => src.BasvuruAfadDurum.Ad))
            .ForMember<string>(dest => dest.TcKimlikNo, opt => opt.MapFrom(src => StringAddon.ToMaskedWord(src.TcKimlikNo, 3)))
            .ForMember<string>(dest => dest.TcKimlikNoRaw, opt => opt.MapFrom(src => src.TcKimlikNo))
            .ForMember<string>(dest => dest.BasvuruTurAd, opt => opt.MapFrom(src => src.BasvuruTur.Ad))
            .ForMember<string>(dest => dest.BasvuruKanalAd, opt => opt.MapFrom(src => src.BasvuruKanal.Ad))
            .ForMember(dest => dest.HibeOdemeTutar, opt => opt.MapFrom(src => src.BasvuruImzaVerens.FirstOrDefault(x => !x.SilindiMi && x.AktifMi == true).HibeOdemeTutar))
            .ForMember(dest => dest.KrediOdemeTutar, opt => opt.MapFrom(src => src.BasvuruImzaVerens.FirstOrDefault(x => !x.SilindiMi && x.AktifMi == true).KrediOdemeTutar))
            .ForMember(dest => dest.BagimsizBolumAlani, opt => opt.MapFrom(src => src.BasvuruImzaVerens.FirstOrDefault(x => !x.SilindiMi && x.AktifMi == true).BagimsizBolumAlani))
            .ForMember(dest => dest.HissePay, opt => opt.MapFrom(src => src.BasvuruImzaVerens.FirstOrDefault(x => !x.SilindiMi && x.AktifMi == true).HissePay))
            .ForMember(dest => dest.HissePayda, opt => opt.MapFrom(src => src.BasvuruImzaVerens.FirstOrDefault(x => !x.SilindiMi && x.AktifMi == true).HissePayda))
            .ForMember(dest => dest.BagimsizBolumNo, opt => opt.MapFrom(src => src.BasvuruImzaVerens.FirstOrDefault(x => !x.SilindiMi && x.AktifMi == true).BagimsizBolumNo))
            .ForMember(dest => dest.IptalEdilebilirMi, opt => opt.MapFrom(src => src.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                                              && src.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir))
            .ForMember(dest => dest.ImzaVerilebilirMi, opt => opt.MapFrom(src => src.BasvuruAfadDurumId != (int)BasvuruAfadDurumEnum.Kabul
                                                                              && src.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                                              && src.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir))
            .ForMember<string>(dest => dest.Color, opt => opt.MapFrom(src =>
                        src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruIptalEdildi || src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir ? "#ffbebe" :
                        src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzOnaylanmistir ? "#64ff83" :
                        src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruReddedilmistir ? "#f14141" : ""
                )
            )
             .ForMember(dest => dest.FeragatnameDosyaId, opt => opt.MapFrom(src => src.BasvuruImzaVerens.Where(x =>
                            !x.SilindiMi && x.AktifMi == true).FirstOrDefault().BasvuruImzaVerenDosyas.Where(x => !x.SilindiMi && x.AktifMi == true
                            && x.BasvuruImzaVerenDosyaTurId == (long)BasvuruImzaVerenDosyaTurEnum.TaahhutnameBelgesi).FirstOrDefault().BasvuruImzaVerenDosyaGuid
             ))


             .ForMember(dest => dest.AfadDurumIptalDosyaId, opt => opt.MapFrom(src => src.BasvuruDosyas.FirstOrDefault(x => !x.SilindiMi && x.AktifMi == true && x.BasvuruDosyaTurId == (long)BasvuruDosyaTurEnum.AfadDurumGuncellemeBelgesi).BasvuruDosyaGuid))
            .ForMember(dest => dest.AfadDurumDegisebilirMi, opt => opt.MapFrom(src => src.BasvuruAfadDurumId == (long)BasvuruAfadDurumEnum.Kabul && src.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi && src.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir));

        CreateMap<BasvuruKamuUstlenecek, GetirListeMaliklerQueryResponseModel>()
            .ForMember<string>(dest => dest.BinaDisKapiNo, opt => opt.MapFrom(src => src.BinaDegerlendirme.BinaDisKapiNo))
            .ForMember<string>(dest => dest.BasvuruDurumAd, opt => opt.MapFrom(src => src.BasvuruDurum.Ad))
            .ForMember<string>(dest => dest.BasvuruAfadDurumAd, opt => opt.MapFrom(src => src.BasvuruAfadDurum.Ad))
            .ForMember<string>(dest => dest.TcKimlikNo, opt => opt.MapFrom(src => StringAddon.ToMaskedWord(src.TcKimlikNo, 3)))
            .ForMember<string>(dest => dest.TcKimlikNoRaw, opt => opt.MapFrom(src => src.TcKimlikNo))
            .ForMember<string>(dest => dest.BasvuruTurAd, opt => opt.MapFrom(src => src.BasvuruTur.Ad))
            .ForMember<string>(dest => dest.BasvuruDegerlendirmeDurumAd, opt => opt.MapFrom(_ => BinaDegerlendirmeDurumEnum.KamuUstlenecek.GetDisplayName()))
            .ForMember<string>(dest => dest.DurumClassName, opt => opt.MapFrom(_ => "primary"))
            //.ForMember<string>(dest => dest.Ad, opt => opt.MapFrom(src => StringAddon.ToMaskedWord(src.Ad, 2)))
            //.ForMember<string>(dest => dest.Soyad, opt => opt.MapFrom(src => StringAddon.ToMaskedWord(src.Soyad, 2)))
            .ForMember(dest => dest.HibeOdemeTutar, opt => opt.MapFrom(src => src.BasvuruImzaVerens.FirstOrDefault(x => !x.SilindiMi && x.AktifMi == true).HibeOdemeTutar))
            .ForMember(dest => dest.KrediOdemeTutar, opt => opt.MapFrom(src => src.BasvuruImzaVerens.FirstOrDefault(x => !x.SilindiMi && x.AktifMi == true).KrediOdemeTutar))
            .ForMember(dest => dest.BagimsizBolumAlani, opt => opt.MapFrom(src => src.BasvuruImzaVerens.FirstOrDefault(x => !x.SilindiMi && x.AktifMi == true).BagimsizBolumAlani))
            .ForMember(dest => dest.BagimsizBolumNo, opt => opt.MapFrom(src => src.BasvuruImzaVerens.FirstOrDefault(x => !x.SilindiMi && x.AktifMi == true) == null ? src.TapuBagimsizBolumNo : src.BasvuruImzaVerens.FirstOrDefault(x => !x.SilindiMi && x.AktifMi == true).BagimsizBolumNo == null ? src.TapuBagimsizBolumNo : null))
            .ForMember(dest => dest.HissePay, opt => opt.MapFrom(src => src.BasvuruImzaVerens.FirstOrDefault(x => !x.SilindiMi && x.AktifMi == true).HissePay))
            .ForMember(dest => dest.HissePayda, opt => opt.MapFrom(src => src.BasvuruImzaVerens.FirstOrDefault(x => !x.SilindiMi && x.AktifMi == true).HissePayda))
            .ForMember(dest => dest.IptalEdilebilirMi, opt => opt.MapFrom(src => src.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                                              && src.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir))
            .ForMember(dest => dest.ImzaVerilebilirMi, opt => opt.MapFrom(src => src.BasvuruAfadDurumId != (int)BasvuruAfadDurumEnum.Kabul
                                                                              && src.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                                              && src.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir))
            .ForMember<string>(dest => dest.Color, opt => opt.MapFrom(src =>
                        src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruIptalEdildi || src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir ? "#ffbebe" :
                        src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzOnaylanmistir ? "#64ff83" :
                        src.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruReddedilmistir ? "#f14141" : ""
                )
            )
            .ForMember(dest => dest.FeragatnameDosyaId, opt => opt.MapFrom(src => src.BasvuruImzaVerens.Where(x =>
                            !x.SilindiMi && x.AktifMi == true).FirstOrDefault().BasvuruImzaVerenDosyas.Where(x => !x.SilindiMi && x.AktifMi == true
                            && x.BasvuruImzaVerenDosyaTurId == (long)BasvuruImzaVerenDosyaTurEnum.TaahhutnameBelgesi).FirstOrDefault().BasvuruImzaVerenDosyaGuid
             ))
            .ForMember(dest => dest.AfadDurumDegisebilirMi, opt => opt.MapFrom(src => src.BasvuruAfadDurumId == (long)BasvuruAfadDurumEnum.Kabul && src.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi && src.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir));
    }
}