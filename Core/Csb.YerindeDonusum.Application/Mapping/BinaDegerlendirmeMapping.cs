using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirBinaDegerlendirmeDetay;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeOdemeBekleyenDegerlendirmeServerSide;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirNakdiYardimTaksitler;
using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirYapiDenetimSeviyeTespitBilgiler;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Domain.Entities;

namespace Csb.YerindeDonusum.Application.Mapping;

public class BinaDegerlendirmeMapping : Profile
{
    public BinaDegerlendirmeMapping()
    {
        CreateMap<BinaDegerlendirme, GetirListeOdemeBekleyenDegerlendirmeServerSideResponseModel>()
           .ForMember(dest => dest.OdemeListesiButonuAktifMi, opt => opt.MapFrom(src => src.AktifMi))
           // ada - parsel başvurudan alınıyor, yorum satırı yapılırsa BinaDegerlendirme tablosundan alınır.
           .ForMember(dest => dest.Ada,
                    opt => opt.MapFrom(src => src.Basvurus.FirstOrDefault(y => !string.IsNullOrWhiteSpace(y.UavtBinaAda)
                                                                        && !string.IsNullOrWhiteSpace(y.UavtBinaParsel)).UavtBinaAda))
           .ForMember(dest => dest.Parsel,
                    opt => opt.MapFrom(src => src.Basvurus.FirstOrDefault(y => !string.IsNullOrWhiteSpace(y.UavtBinaAda)
                                                                        && !string.IsNullOrWhiteSpace(y.UavtBinaParsel)).UavtBinaParsel))
           .ForMember(dest => dest.Seviye,
                    opt => opt.MapFrom(src => src.BinaOdemes.Where(y => y.AktifMi == true && y.SilindiMi == false)
                                                .MaxBy(y => y.Seviye).Seviye))
           .ForMember(dest => dest.BinaOdemeDurumAd,
                    opt => opt.MapFrom(src => src.BinaOdemes.Where(y => y.AktifMi == true && y.SilindiMi == false)
                                                .MaxBy(y => y.Seviye).BinaOdemeDurum.Ad))

           .ForMember(dest => dest.OdemeTutari,
                    opt => opt.MapFrom(src => src.BinaOdemes.Where(y => y.AktifMi == true && y.SilindiMi == false
                                                                    && y.BinaOdemeDurumId != (int)BinaOdemeDurumEnum.Reddedildi)
                                                                    .Sum(y => y.OdemeTutari)))
           .ForMember(dest => dest.HibeOdemeTutari,
                    opt => opt.MapFrom(src => src.BinaOdemes.Where(y => y.AktifMi == true && y.SilindiMi == false
                                                                    && y.BinaOdemeDurumId != (int)BinaOdemeDurumEnum.Reddedildi)
                                                                    .Sum(y => y.HibeOdemeTutari)))
           .ForMember(dest => dest.KrediOdemeTutari,
                    opt => opt.MapFrom(src => src.BinaOdemes.Where(y => y.AktifMi == true && y.SilindiMi == false
                                                                    && y.BinaOdemeDurumId != (int)BinaOdemeDurumEnum.Reddedildi)
                                                                    .Sum(y => y.KrediOdemeTutari)))
           .ReverseMap();

        CreateMap<BinaDegerlendirme, GetirBinaDegerlendirmeDetayQueryResponseModel>()
            .ForMember(dest => dest.BinaDegerlendirmeDosya, opt => opt.MapFrom(src => src.BinaDegerlendirmeDosyas
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
                                                                                    BinaMuteahhitTapuTurId=s.BinaMuteahhitTapuTurId

                                                                                }).FirstOrDefault()));
    }
}