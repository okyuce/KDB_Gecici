using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeDetay;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiServerSideGroupped;
using Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Queries.GetirListeYeniYapiServerSideGruplanmamis;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class YeniYapiMapping : Profile
{
    public YeniYapiMapping()
    {
        CreateMap<BinaDegerlendirme, BinaDegerlendirmeDto>()
            .ForMember(dest => dest.TapuIlAdi, opt => opt.MapFrom(src => src.Basvurus.Where(x => x.AktifMi == true && x.SilindiMi == false).FirstOrDefault().TapuIlAdi))
            .ForMember(dest => dest.TapuIlceAdi, opt => opt.MapFrom(src => src.Basvurus.Where(x => x.AktifMi == true && x.SilindiMi == false).FirstOrDefault().TapuIlceAdi))
            .ForMember(dest => dest.TapuMahalleAdi, opt => opt.MapFrom(src => src.Basvurus.Where(x => x.AktifMi == true && x.SilindiMi == false).FirstOrDefault().TapuMahalleAdi))
            .ForMember(dest => dest.TapuIlNo, opt => opt.MapFrom(src => src.Basvurus.Where(x => x.AktifMi == true && x.SilindiMi == false).FirstOrDefault().TapuIlId))
            .ForMember(dest => dest.TapuIlceNo, opt => opt.MapFrom(src => src.Basvurus.Where(x => x.AktifMi == true && x.SilindiMi == false).FirstOrDefault().TapuIlceId))
            .ForMember(dest => dest.TapuMahalleNo, opt => opt.MapFrom(src => src.Basvurus.Where(x => x.AktifMi == true && x.SilindiMi == false).FirstOrDefault().TapuMahalleId))
            .ForMember(dest => dest.EskiTapuAda, opt => opt.MapFrom(src => src.Basvurus.Where(x => x.AktifMi == true && x.SilindiMi == false).FirstOrDefault().EskiTapuAda))
            .ForMember(dest => dest.EskiTapuParsel, opt => opt.MapFrom(src => src.Basvurus.Where(x => x.AktifMi == true && x.SilindiMi == false).FirstOrDefault().EskiTapuParsel))
            .ForMember(dest => dest.AdaParselGuncellemeTipId, opt => opt.MapFrom(src => src.AdaParselGuncellemeTipiId))
            .ForMember(dest => dest.AdaParselGuncellemeTipiAdi, opt => opt.MapFrom(src => src.AdaParselGuncellemeTipiId == null ? "" : ((AdaParselGuncellemeTipiEnum)src.AdaParselGuncellemeTipiId).GetDisplayName()))
            .ForMember(dest => dest.AdaParselGuncelleDosyaGuid, opt => opt.MapFrom(src => src.BinaDegerlendirmeDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true && y.BinaDegerlendirmeDosyaTurId == (long)BinaDegerlendirmeDosyaTurEnum.AdaParselGuncelleme).FirstOrDefault().BinaDosyaGuid))
            .ForMember(dest => dest.DegerlendirmeButonuAktifMi, opt => opt.MapFrom(src => src.Basvurus.Count(y => y.BasvuruImzaVerens.Any(z => z.BasvuruImzaVerenDosyas.Any()))
                                                                                    + src.BasvuruKamuUstleneceks.Count(y => y.BasvuruImzaVerens.Any(z => z.BasvuruImzaVerenDosyas.Any())) > 0))
            .ForMember(dest => dest.BasvuruDegerlendirmeDurumAd, opt => opt.MapFrom(src => src.BinaDegerlendirmeDurum.Ad))
            .ForMember(dest => dest.BinaDegerlendirmeDosya, opt => opt.MapFrom(src => src.BinaDegerlendirmeDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true && y.BinaDegerlendirmeDosyaTurId == (long)BinaDegerlendirmeDosyaTurEnum.BinaDegerlendirme)
                                .Select(s => new GetirBinaDegerlendirmeDetayBinaDegerlendirmeDosyaModel
                                {
                                    BinaDosyaGuid = s.BinaDosyaGuid,
                                    DosyaAdi = s.DosyaAdi
                                }).FirstOrDefault()))
            .ForMember(dest => dest.BinaYapiRuhsatIzinDosya, opt => opt.MapFrom(src => src.BinaYapiRuhsatIzinDosyas
                                .Select(s => new GetirBinaDegerlendirmeDetayBinaYapiRuhsatIzinDosyaModel
                                {
                                    BinaDosyaGuid = s.BinaYapiRuhsatIzinDosyaGuid,
                                    DosyaAdi = s.DosyaAdi
                                }).FirstOrDefault()))
            .ForMember(dest => dest.BinaYapiDenetimSeviyeTespit, opt => opt.MapFrom(src => src.BinaYapiDenetimSeviyeTespits
                                .Select(s => new GetirBinaDegerlendirmeDetayBinaYapiDenetimSeviyeTespitModel
                                {
                                    BinaYapiDenetimSeviyeTespitId = s.BinaYapiDenetimSeviyeTespitId,
                                    IlerlemeYuzdesi = s.IlerlemeYuzdesi
                                }).FirstOrDefault()))
            .ForMember(dest => dest.Muteahhit, opt => opt.MapFrom(src => src.BinaMuteahhits
                                .Select(s => new GetirBinaDegerlendirmeDetayMuteahhitModel
                                {
                                    AdSoyadUnvan = s.Adsoyadunvan,
                                    Adres = s.Adres,
                                    CepTelefonu = s.CepTelefonu,
                                    Telefon = s.Telefon,
                                    Eposta = s.Eposta,
                                    VergiKimlikNo = s.VergiKimlikNo,
                                    YetkiBelgeNo = s.YetkiBelgeNo,
                                    Aciklama = s.Aciklama,
                                    IbanNo = s.IbanNo,
                                    BinaMuteahhitTapuTurId = s.BinaMuteahhitTapuTurId
                                }).FirstOrDefault()))
            .ForMember(dest => dest.OlusturanKullaniciId, opt => opt.MapFrom(src => src.OlusturanKullaniciId))
            .ForMember(dest => dest.GuncelleyenKullaniciId, opt => opt.MapFrom(src => src.GuncelleyenKullaniciId));

        CreateMap<GetirListeYeniYapiServerSideGrouppedResponseModel, GetirListeYeniYapiServerSideGrouppedResponseModel>()
            .ForMember(dest => dest.AdaParselGuncellemeButonuAktifMi, opt => opt.MapFrom(src => (src.AdaParselGuncellemeTipiId !=null  ||  src.YeniYapiList.Any(z => z.BinaDegerlendirmeDurumId != (long)BinaDegerlendirmeDurumEnum.BasvurunuzDegerlendirmeyeAlinmistir)) ? false : true));

        CreateMap<BinaDegerlendirme, GetirListeYeniYapiServerSideGruplanmamisResponseModel>()
            .ForMember(dest => dest.OlusturanKullaniciId, opt => opt.MapFrom(src => src.OlusturanKullaniciId))
            .ForMember(dest => dest.GuncelleyenKullaniciId, opt => opt.MapFrom(src => src.GuncelleyenKullaniciId))
            .ForMember(dest => dest.BasvuruDegerlendirmeDurumAd, opt => opt.MapFrom(src => src.BinaDegerlendirmeDurum.Ad))
            .ForMember(dest => dest.AdaParsel, opt => opt.MapFrom(src => (src.Ada.Length > 0 ? src.Ada : "-") + "/" + (src.Parsel.Length > 0 ? src.Parsel : "-")));

    }
}