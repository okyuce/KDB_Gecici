using AutoMapper;
using Csb.YerindeDonusum.Application.Constants;
using Csb.YerindeDonusum.Application.CQRS.AydinlatmaMetniCQRS.Queries;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruImzaCQRS.Commands.KaydetBasvuruKendimKarsilamakIstiyorum;

public class KaydetBasvuruKendimKarsilamakIstiyorumCommand : IRequest<ResultModel<BasvuruImzaVerenDto>>
{
    public long? BasvuruId { get; set; }
    public long? BasvuruKamuUstlenecekId { get; set; }
    public long? BinaDegerlendirmeId { get; set; }
    public long? BasvuruImzaVerenId { get; set; }
    public long? BasvuruDestekTurId { get; set; } // dosya validasyonları için gerekli.
    public DosyaDto? TaahhutnameBelgesi { get; set; }

    public class KaydetBasvuruKendimKarsilamakIstiyorumCommandHandler : IRequestHandler<KaydetBasvuruKendimKarsilamakIstiyorumCommand, ResultModel<BasvuruImzaVerenDto>>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IBasvuruKamuUstlenecekRepository _basvuruKamuUstlenecekRepository;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IBasvuruImzaVerenRepository _basvuruImzaVerenRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IConfiguration _configuration;
        private readonly IBinaKotUstuSayiRepository _binaKotUstuSayiRepository;

        public KaydetBasvuruKendimKarsilamakIstiyorumCommandHandler(IMapper mapper, IBasvuruKamuUstlenecekRepository basvuruKamuUstlenecekRepository, IConfiguration configuration, IBasvuruRepository basvuruRepository, IBinaDegerlendirmeRepository binaDegerlendirmeRepository, IBasvuruImzaVerenRepository basvuruImzaVerenRepository, IKullaniciBilgi kullaniciBilgi, IBinaKotUstuSayiRepository binaKotUstuSayiRepository, IMediator mediator)
        {
            _mapper = mapper;
            _basvuruKamuUstlenecekRepository = basvuruKamuUstlenecekRepository;
            _configuration = configuration;
            _basvuruRepository = basvuruRepository;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _basvuruImzaVerenRepository = basvuruImzaVerenRepository;
            _kullaniciBilgi = kullaniciBilgi;
            _binaKotUstuSayiRepository = binaKotUstuSayiRepository;
            _mediator = mediator;
        }

        public async Task<ResultModel<BasvuruImzaVerenDto>> Handle(KaydetBasvuruKendimKarsilamakIstiyorumCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<BasvuruImzaVerenDto>();

            Basvuru? basvuru = null;
            BasvuruKamuUstlenecek? basvuruKamuUstlenecek = null;
            if (request?.BasvuruId > 0)
            {
                basvuru = _basvuruRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                        && x.BasvuruId == request.BasvuruId,
                                        true
                                    ).FirstOrDefault();

                if (basvuru == null)
                {
                    result.ErrorMessage("Başvuru bulunamadı.");
                    return await Task.FromResult(result);
                }

                var krediList = _basvuruRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                        && x.BasvuruDurumId != (int)BasvuruDurumEnum.BasvuruIptalEdildi
                                                        && x.BasvuruDurumId != (int)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                        && (x.BasvuruDestekTurId == (int)BasvuruDestekTurEnum.HibeVeKredi
                                                                || x.BasvuruDestekTurId == (int)BasvuruDestekTurEnum.Kredi)
                                                        );
                // tüzel kişi ise:
                if (basvuru.TuzelKisiTipId != null)
                {
                    if (basvuru.TuzelKisiVergiNo != null)
                        krediList = krediList.Where(x => x.TuzelKisiVergiNo == basvuru.TuzelKisiVergiNo);
                    else if (basvuru.TuzelKisiMersisNo != null)
                        krediList = krediList.Where(x => x.TuzelKisiMersisNo == basvuru.TuzelKisiMersisNo);
                    else
                    {
                        result.ErrorMessage("Tüzel kişi vergi no ya da mersis no olmadığından işleme devam edilemiyor!");
                        return await Task.FromResult(result);
                    }
                }
                // gerçek kişi ise:
                else
                    krediList = krediList.Where(x => x.TcKimlikNo == basvuru.TcKimlikNo);

                var sayi = krediList.Count();
                if (sayi > 3)
                {
                    result.ErrorMessage("TC Kimlik Numarası, tüzel kişi vergi no ya da tüzel kişi mersis noya bağlı Kredi sayısı 3' ten fazla olduğu için işleme devam edilemiyor. Lütfen önce "
                                            + (sayi - 3) + " adet başvurunuzu iptal ediniz.");
                    return await Task.FromResult(result);
                }
            }
            else
            {
                basvuruKamuUstlenecek = _basvuruKamuUstlenecekRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                    && x.BasvuruKamuUstlenecekId == request.BasvuruKamuUstlenecekId
                                        , true
                                        ).FirstOrDefault();

                if (basvuruKamuUstlenecek == null)
                {
                    result.ErrorMessage("Başvuru bulunamadı.");
                    return await Task.FromResult(result);
                }
            }

            var binaDegerlendirmeQuery = _binaDegerlendirmeRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                                && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId
                                                        , true
                                                        , x => x.BinaYapiRuhsatIzinDosyas.Where(x => x.SilindiMi == false && x.AktifMi == true));
            if (basvuru != null)
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => x.UavtMahalleNo == basvuru.UavtMahalleNo && x.HasarTespitAskiKodu == basvuru.HasarTespitAskiKodu);
            else if (basvuruKamuUstlenecek != null)
                binaDegerlendirmeQuery = binaDegerlendirmeQuery.Where(x => x.UavtMahalleNo == basvuruKamuUstlenecek.UavtMahalleNo
                                                                        && x.Ada == basvuruKamuUstlenecek.TapuAda
                                                                        && x.Parsel == basvuruKamuUstlenecek.TapuParsel
                                                                );

            var binaDegerlendirme = binaDegerlendirmeQuery.FirstOrDefault();

            if (binaDegerlendirme != null)
            {
                if (binaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiRuhsatinizOnaylanmistir
                     || binaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiIlerlemeSeviyesiYuzde20
                     || binaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiIlerlemeSeviyesiYuzde60
                     || binaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiTamamlanmistir)
                {
                    result.ErrorMessage("Yapı ruhsatınız onaylandığı için işleme devam edilemiyor.");
                    return await Task.FromResult(result);
                }
                var ayar = await _mediator.Send(new GetirAyarQuery());
                if (ayar.IsError)
                {
                    result.ErrorMessage("Ayarlar Okunamadığı için işleme devam edilemiyor.");

                    return await Task.FromResult(result);
                }
                var binaKotUstSayisi = _binaKotUstuSayiRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                                               && x.UavtIlId == binaDegerlendirme.UavtIlNo
                                                                                && (x.UavtIlceId == null || x.UavtIlceId == binaDegerlendirme.UavtIlceNo)
                                                                                && (x.UavtMahalleId == null || x.UavtMahalleId == binaDegerlendirme.UavtMahalleNo)
                                                                                && (x.Ada == null || x.Ada == binaDegerlendirme.Ada)
                                                                                && (x.Parsel == null || x.Parsel == binaDegerlendirme.Parsel)
                                                                               , false).FirstOrDefault()?
                                                                               //.KotUstuKatSayisi ?? AyarConstants.KotUstKatSayisi;
                                                                               .KotUstuKatSayisi ?? ayar.Result.Basvuru.KotUstKatSayisi;

                // izin belgesi girilmemişse ve BültenNo NULL değilse, KotUstKatSayisi 4' den büyük olanlar işleme devam edemez.
                if (binaDegerlendirme.BinaYapiRuhsatIzinDosyas?.Any(x => x.SilindiMi == false && x.AktifMi == true) != true
                        && binaDegerlendirme.BultenNo > 0
                        && binaDegerlendirme.KotUstKatSayisi > binaKotUstSayisi)
                {
                    result.ErrorMessage("Kot Üstü Kat sayısı " + binaKotUstSayisi + "' den fazla olduğu için işleme devam edilemiyor.");
                    return await Task.FromResult(result);
                }
            }

            var basvuruImzaVerenQuery = _basvuruImzaVerenRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true,
                                                    true,
                                                    i => i.BasvuruImzaVerenDosyas.Where(y => y.SilindiMi == false && y.AktifMi == true));

            if (basvuru != null)
                basvuruImzaVerenQuery = basvuruImzaVerenQuery.Where(x => x.BasvuruId == request.BasvuruId);
            else if (basvuruKamuUstlenecek != null)
                basvuruImzaVerenQuery = basvuruImzaVerenQuery.Where(x => x.BasvuruKamuUstlenecekId == request.BasvuruKamuUstlenecekId);

            var basvuruImzaVeren = basvuruImzaVerenQuery.FirstOrDefault();

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            long.TryParse(kullaniciBilgi.KullaniciId, out long kullaniciId);
            string? ipAdresi = kullaniciBilgi?.IpAdresi;

            basvuruImzaVeren ??= new BasvuruImzaVeren
            {
                SilindiMi = false,
                AktifMi = true,
                OlusturmaTarihi = DateTime.Now,
                OlusturanKullaniciId = kullaniciId,
                OlusturanIp = ipAdresi,
                BasvuruId = basvuru?.BasvuruId,
                BasvuruKamuUstlenecekId = basvuruKamuUstlenecek?.BasvuruKamuUstlenecekId,
                BasvuruDestekTurId = basvuru != null ? basvuru.BasvuruDestekTurId : basvuruKamuUstlenecek.BasvuruDestekTurId,
            };

            basvuruImzaVeren.BasvuruTurId = basvuru?.BasvuruTurId ?? basvuruKamuUstlenecek.BasvuruTurId;
            basvuruImzaVeren.HibeOdemeTutar = 0;
            basvuruImzaVeren.KrediOdemeTutar = 0;

            if (basvuruImzaVeren.BasvuruImzaVerenId > 0)
            {
                basvuruImzaVeren.GuncellemeTarihi = DateTime.Now;
                basvuruImzaVeren.GuncelleyenKullaniciId = kullaniciId;
                basvuruImzaVeren.GuncelleyenIp = kullaniciBilgi.IpAdresi;

                if (FluentValidationExtension.NotEmpty(request?.TaahhutnameBelgesi))
                {
                    byte[] data = Convert.FromBase64String(request?.TaahhutnameBelgesi?.DosyaBase64);
                    var isTheFileTypeAllowed = FileTypeVerifier.Verify(data);
                    if (isTheFileTypeAllowed.IsVerified)
                    {
                        var taahhutnameBelgesi = basvuruImzaVeren.BasvuruImzaVerenDosyas.FirstOrDefault(x =>
                                                                        x.SilindiMi == false
                                                                        &&
                                                                        x.AktifMi == true
                                                                        &&
                                                                        x.BasvuruImzaVerenDosyaTurId == (long)BasvuruImzaVerenDosyaTurEnum.TaahhutnameBelgesi
                                                                    );

                        var DosyaAdi = string.Concat(Guid.NewGuid(), request?.TaahhutnameBelgesi?.DosyaUzanti);
                        var DosyaTuru = MimeTypes.GetMimeType(request?.TaahhutnameBelgesi?.DosyaUzanti);
                        var DosyaYolu = DateTime.Now.ToString("yyyy-MM-dd");

                        if (taahhutnameBelgesi == null)
                        {
                            taahhutnameBelgesi = new BasvuruImzaVerenDosya()
                            {
                                BasvuruImzaVerenDosyaTurId = (long)BasvuruImzaVerenDosyaTurEnum.TaahhutnameBelgesi,
                                DosyaAdi = DosyaAdi,
                                DosyaTuru = DosyaTuru,
                                DosyaYolu = DosyaYolu,
                                OlusturanKullaniciId = kullaniciId,
                                OlusturanIp = kullaniciBilgi.IpAdresi,
                                OlusturmaTarihi = DateTime.Now,
                                AktifMi = true,
                                SilindiMi = false,
                            };

                            basvuruImzaVeren.BasvuruImzaVerenDosyas.Add(taahhutnameBelgesi);
                        }
                        else
                        {
                            taahhutnameBelgesi.BasvuruImzaVerenDosyaTurId = (long)BasvuruImzaVerenDosyaTurEnum.TaahhutnameBelgesi;
                            taahhutnameBelgesi.DosyaAdi = DosyaAdi;
                            taahhutnameBelgesi.DosyaTuru = DosyaTuru;
                            taahhutnameBelgesi.DosyaYolu = DosyaYolu;
                            taahhutnameBelgesi.GuncellemeTarihi = DateTime.Now;
                            taahhutnameBelgesi.GuncelleyenKullaniciId = kullaniciId;
                            taahhutnameBelgesi.GuncelleyenIp = kullaniciBilgi.IpAdresi;
                        }

                        var uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", DosyaYolu);
                        if (!Directory.Exists(uploadDirectoryPath))
                            Directory.CreateDirectory(uploadDirectoryPath);
                        var filePath = string.Concat(uploadDirectoryPath, "\\", DosyaAdi);
                        using var stream = File.Create(filePath);
                        stream.Write(data, 0, data.Length);
                    }
                }
                _basvuruImzaVerenRepository.Update(basvuruImzaVeren);
            }
            else
            {
                if (FluentValidationExtension.NotEmpty(request?.TaahhutnameBelgesi))
                {
                    byte[] data = Convert.FromBase64String(request?.TaahhutnameBelgesi?.DosyaBase64);
                    var isTheFileTypeAllowed = FileTypeVerifier.Verify(data);
                    if (isTheFileTypeAllowed.IsVerified)
                    {
                        var taahhutnameBelgesi = basvuruImzaVeren.BasvuruImzaVerenDosyas.FirstOrDefault(x =>
                                                                        x.SilindiMi == false
                                                                        &&
                                                                        x.AktifMi == true
                                                                        &&
                                                                        x.BasvuruImzaVerenDosyaTurId == (long)BasvuruImzaVerenDosyaTurEnum.TaahhutnameBelgesi
                                                                    );

                        var DosyaAdi = string.Concat(Guid.NewGuid(), request?.TaahhutnameBelgesi?.DosyaUzanti);
                        var DosyaTuru = MimeTypes.GetMimeType(request?.TaahhutnameBelgesi?.DosyaUzanti);
                        var DosyaYolu = DateTime.Now.ToString("yyyy-MM-dd");

                        if (taahhutnameBelgesi == null)
                        {
                            taahhutnameBelgesi = new BasvuruImzaVerenDosya()
                            {
                                BasvuruImzaVerenDosyaTurId = (long)BasvuruImzaVerenDosyaTurEnum.TaahhutnameBelgesi,
                                DosyaAdi = DosyaAdi,
                                DosyaTuru = DosyaTuru,
                                DosyaYolu = DosyaYolu,
                                OlusturanKullaniciId = kullaniciId,
                                OlusturanIp = kullaniciBilgi.IpAdresi,
                                OlusturmaTarihi = DateTime.Now,
                                AktifMi = true,
                                SilindiMi = false,
                            };

                            basvuruImzaVeren.BasvuruImzaVerenDosyas.Add(taahhutnameBelgesi);
                        }
                        else
                        {
                            taahhutnameBelgesi.BasvuruImzaVerenDosyaTurId = (long)BasvuruImzaVerenDosyaTurEnum.TaahhutnameBelgesi;
                            taahhutnameBelgesi.DosyaAdi = DosyaAdi;
                            taahhutnameBelgesi.DosyaTuru = DosyaTuru;
                            taahhutnameBelgesi.DosyaYolu = DosyaYolu;
                            taahhutnameBelgesi.GuncellemeTarihi = DateTime.Now;
                            taahhutnameBelgesi.GuncelleyenKullaniciId = kullaniciId;
                            taahhutnameBelgesi.GuncelleyenIp = kullaniciBilgi.IpAdresi;
                        }

                        var uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", DosyaYolu);
                        if (!Directory.Exists(uploadDirectoryPath))
                            Directory.CreateDirectory(uploadDirectoryPath);
                        var filePath = string.Concat(uploadDirectoryPath, "\\", DosyaAdi);
                        using var stream = File.Create(filePath);
                        stream.Write(data, 0, data.Length);
                    }
                }
                await _basvuruImzaVerenRepository.AddAsync(basvuruImzaVeren);
            }


            await _basvuruImzaVerenRepository.SaveChanges();
            result.Result = _mapper.Map<BasvuruImzaVerenDto>(basvuruImzaVeren);

            return await Task.FromResult(result);
        }
    }
}