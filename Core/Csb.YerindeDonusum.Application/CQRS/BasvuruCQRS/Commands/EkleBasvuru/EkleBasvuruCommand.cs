using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.AydinlatmaMetniCQRS.Queries;
using Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuranKisiSayisi;
using Csb.YerindeDonusum.Application.CQRS.HasarTespitCQRS.Queries;
using Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.KdsHasarTespitVeriByUid;
using Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiAdSoyadTcDen;
using Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiBilgileriTcDen;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Hubs;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Addons.StringAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.EkleBasvuru;

public class EkleBasvuruCommand : IRequest<ResultModel<EkleBasvuruCommandResponseModel>>
{
    public EkleBasvuruCommandModel? Model { get; set; }

    public class EkleBasvuruCommandHandler : IRequestHandler<EkleBasvuruCommand, ResultModel<EkleBasvuruCommandResponseModel>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IBasvuruRepository _appealRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IBasvuruTurRepository _basvuruTurRepository;
        private readonly IBasvuruDestekTurRepository _basvuruDestekTurRepository;
        private readonly IAydinlatmaMetniRepository _aydinlatmaMetniRepository;
        private readonly IBasvuruKanalRepository _basvuruKanalRepository;
        private readonly IConfiguration _configuration;
        private readonly ITakbisIlRepository _takbisIlRepository;
        private readonly IHubContext<KdsHub> _hubContext;
        private readonly IBasvuruDosyaTurRepository _basvuruDosyaTurRepository;

        public EkleBasvuruCommandHandler(
              IHubContext<KdsHub> hubContext
            , IMediator mediator
            , IMapper mapper
            , IBasvuruRepository appealRepository
            , IBasvuruTurRepository basvuruTurRepository
            , IBasvuruDestekTurRepository basvuruDestekTurRepository
            , IKullaniciBilgi kullaniciBilgi
            , IAydinlatmaMetniRepository aydinlatmaMetniRepository
            , IBasvuruKanalRepository basvuruKanalRepository
            , ITakbisIlRepository takbisIlRepository
            , IConfiguration configuration
            , IBasvuruDosyaTurRepository basvuruDosyaTurRepository
        )
        {
            _hubContext = hubContext;
            _mediator = mediator;
            _mapper = mapper;
            _appealRepository = appealRepository;
            _basvuruTurRepository = basvuruTurRepository;
            _basvuruDestekTurRepository = basvuruDestekTurRepository;
            _kullaniciBilgi = kullaniciBilgi;
            _aydinlatmaMetniRepository = aydinlatmaMetniRepository;
            _takbisIlRepository = takbisIlRepository;
            _basvuruKanalRepository = basvuruKanalRepository;
            _configuration = configuration;
            _basvuruDosyaTurRepository = basvuruDosyaTurRepository;
        }

        public async Task<ResultModel<EkleBasvuruCommandResponseModel>> Handle(EkleBasvuruCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<EkleBasvuruCommandResponseModel>();

            try
            {
                var ayar = await _mediator.Send(new GetirAyarQuery());

                if (ayar.IsError == true)
                {
                    result.Exception(new ArgumentNullException("Başvuru İşlemleri İçin Gerekli Olan Ayarlar Alınamadı."), "Başvuru İşlemleri İçin Gerekli Olan Ayarlar Alınamadı.");

                    return await Task.FromResult(result);
                }
                else if (ayar.Result.Basvuru.EnFazlaKrediSayisi <= 0 || ayar.Result.Basvuru.EnFazlaEvHibeSayisi <= 0 || ayar.Result.Basvuru.EnFazlaTicarethaneHibeSayisi <= 0)
                {
                    result.Exception(new ArgumentNullException("Başvuru İşlemleri İçin Gerekli Olan Hibe ve Kredi Ayarları Alınamadı."), "Başvuru İşlemleri İçin Gerekli Olan Hibe ve Kredi Ayarları Alınamadı.");

                    return await Task.FromResult(result);
                }

                ValidationInputValue(request.Model, result, ayar);

                if (result.IsError == true)
                    return await Task.FromResult(result);

                var userInfo = _kullaniciBilgi.GetUserInfo();

                long.TryParse(userInfo.KullaniciId, out long kullaniciId);

                var basvuru = _mapper.Map<Basvuru>(request?.Model);
                basvuru.OlusturmaTarihi = DateTime.Now;
                basvuru.OlusturanKullaniciId = kullaniciId;
                basvuru.OlusturanIp = userInfo.IpAdresi;
                basvuru.BasvuruKodu = basvuru.BasvuruKodu.GenerateAppeadlCode();

                basvuru.BasvuruDurumId = (long)BasvuruDurumEnum.BasvurunuzAlinmistir;
                                
                #region ...: Başvuru Kanalı Atama :...

                var basvuruKanali = _basvuruKanalRepository.GetWhere(x => x.BasvuruKanalGuid == Guid.Parse(request.Model.BasvuruKanalId), true).FirstOrDefault();

                if (basvuruKanali == null)
                {
                    result.ErrorMessage("Başvuru Kanalı Hatalı. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
                    return await Task.FromResult(result);
                }


                basvuru.BasvuruKanalId = basvuruKanali.BasvuruKanalId;

                #endregion

                #region ...: Başvuru Destek Tür Atama :...

                var basvuruDestekTur = _basvuruDestekTurRepository.GetWhere(x => x.BasvuruDestekTurGuid == Guid.Parse(request.Model.BasvuruDestekTurId), true).FirstOrDefault();

                if (basvuruDestekTur == null)
                {
                    result.ErrorMessage("Başvuru Destek Türü Hatalı. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
                    return await Task.FromResult(result);
                }

                basvuru.BasvuruDestekTurId = basvuruDestekTur.BasvuruDestekTurId;

                #endregion

                #region ...: Başvuru Tür Atama :...

                var basvuruTur = _basvuruTurRepository.GetWhere(x => x.BasvuruTurGuid == Guid.Parse(request.Model.BasvuruTurId), true).FirstOrDefault();

                if (basvuruTur == null)
                {
                    result.ErrorMessage("Başvuru Türü Hatalı. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
                    return await Task.FromResult(result);
                }

                basvuru.BasvuruTurId = basvuruTur.BasvuruTurId;

                #endregion

                #region ...: Aydinlatma Metni Atama :...
                var aydinlatmaMetni = _aydinlatmaMetniRepository.GetWhere(x => x.AydinlatmaMetniGuid == Guid.Parse(request.Model.AydinlatmaMetniId), true).FirstOrDefault();

                if (aydinlatmaMetni == null)
                {
                    result.ErrorMessage("Aydınlatma Metni Hatalı. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
                    return await Task.FromResult(result);
                }

                basvuru.AydinlatmaMetniId = aydinlatmaMetni.AydinlatmaMetniId;
                #endregion

                #region ...: Aynı Bilgilere Ait Başvuru Zaten Var :...

                //basvuru.UavtCsbmKodu ??= basvuru.UavtCaddeNo?.ToString();

                var ayniBasvuruDahaOnceYapildiMi = _appealRepository.GetWhere(x =>
                            x.BasvuruTurId == basvuru.BasvuruTurId
                        &&
                            x.TapuTasinmazId == basvuru.TapuTasinmazId
                        &&
                            !(x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruIptalEdildi || x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir)
                        &&
                            x.UavtCsbmKodu == basvuru.UavtCsbmKodu
                        &&
                            x.UavtDisKapiNo == basvuru.UavtDisKapiNo
                        &&
                            x.UavtIcKapiNo == basvuru.UavtIcKapiNo
                        &&
                            x.SilindiMi == false
                    , true);

                if (basvuru.TapuTasinmazId == null)
                    ayniBasvuruDahaOnceYapildiMi = ayniBasvuruDahaOnceYapildiMi.Where(x =>
                        x.TapuMahalleId == basvuru.TapuMahalleId
                        &&
                        x.TapuAda == basvuru.TapuAda
                        &&
                        x.TapuParsel == basvuru.TapuParsel
                        &&
                        x.TapuBagimsizBolumNo == basvuru.TapuBagimsizBolumNo
                    );

                if (basvuru.TuzelKisiTipId != null)
                    ayniBasvuruDahaOnceYapildiMi = ayniBasvuruDahaOnceYapildiMi.Where(x =>
                        x.TuzelKisiTipId != null
                        &&
                        x.TuzelKisiMersisNo == basvuru.TuzelKisiMersisNo
                    );
                else
                    ayniBasvuruDahaOnceYapildiMi = ayniBasvuruDahaOnceYapildiMi.Where(x =>
                        x.TcKimlikNo == basvuru.TcKimlikNo.Trim()
                        &&
                        x.TuzelKisiTipId == null
                    );

                if (ayniBasvuruDahaOnceYapildiMi.Any())
                {
                    result.ErrorMessage("Aynı Bilgilere Ait Başvurunuz Bulunduğu İçin Tekrar Başvuru Yapamazsınız.");
                    return await Task.FromResult(result);
                }

                if (basvuru.BasvuruDestekTurId == BasvuruDestekTurEnum.Hibe || basvuru.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi)
                    ayniBasvuruDahaOnceYapildiMi = ayniBasvuruDahaOnceYapildiMi.Where(x =>
                        x.BasvuruDestekTurId == BasvuruDestekTurEnum.Hibe
                        ||
                        x.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi
                    );
                else if (basvuru.BasvuruDestekTurId == BasvuruDestekTurEnum.Kredi || basvuru.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi)
                    ayniBasvuruDahaOnceYapildiMi = ayniBasvuruDahaOnceYapildiMi.Where(x =>
                        x.BasvuruDestekTurId == BasvuruDestekTurEnum.Kredi
                        ||
                        x.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi
                    );

                if (ayniBasvuruDahaOnceYapildiMi.Any())
                {
                    if (basvuru.BasvuruDestekTurId == BasvuruDestekTurEnum.Hibe)
                    {
                        result.ErrorMessage("Aynı Bilgilere Ait Hibe Veya Hibe Kredi Türünde Başvurunuz Bulunduğu İçin Tekrar Başvuru Yapamazsınız.");
                    }
                    else if (basvuru.BasvuruDestekTurId == BasvuruDestekTurEnum.Kredi)
                    {
                        result.ErrorMessage("Aynı Bilgilere Ait Kredi Veya Hibe Kredi Türünde Başvurunuz Bulunduğu İçin Tekrar Başvuru Yapamazsınız.");
                    }
                    else
                    {
                        result.ErrorMessage("Aynı Bilgilere Ait Hibe Veya Kredi Türünde Başvurunuz Bulunduğu İçin Tekrar Başvuru Yapamazsınız.");
                    }
                    return await Task.FromResult(result);
                }

                #endregion

                #region ...: Askı Kodu İşlemleri :...

                var hasarTespitVeri = await _mediator.Send(new KdsHasarTespitVeriByUidQuery
                {
                    HasarTespitUid = basvuru.HasarTespitUid?.Trim()
                });

                if (hasarTespitVeri.IsError)
                {
                    result.ErrorMessage(hasarTespitVeri.ErrorMessageContent);
                    return await Task.FromResult(result);
                }
                else if (!hasarTespitVeri.Result.BasvuruYapabilirMi)
                {
                    result.ErrorMessage(hasarTespitVeri.Result.BilgilendirmeMesaji);
                    return await Task.FromResult(result);
                }

                //basvuru.UavtIlKodu ??= basvuru.UavtIlNo?.ToString();
                //basvuru.UavtIlceKodu ??= basvuru.UavtIlceNo?.ToString();

                if (hasarTespitVeri?.Result?.Detay?.IlKod?.ToString() != basvuru?.UavtIlKodu?.Trim())
                {
                    result.ErrorMessage("UAVT İli ve Askı Kodu İli Aynı Değil. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
                    return await Task.FromResult(result);
                }
                else if (hasarTespitVeri?.Result?.Detay?.IlceKod?.ToString() != basvuru?.UavtIlceKodu?.Trim())
                {
                    result.ErrorMessage("UAVT İlçesi ve Askı Kodu İlçesi Aynı Değil. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
                    return await Task.FromResult(result);
                }

                #region ...: Hasar Tespit İli ve Tapu İli Kontrolü :...

                var tabkisIl = _takbisIlRepository.GetWhere(x => x.IlKod == hasarTespitVeri.Result.Detay.IlKod && x.Aktif == true).FirstOrDefault();
                if (tabkisIl == null)
                {
                    result.ErrorMessage("Tapu İl Bilgisi Bulunamadı. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
                    return await Task.FromResult(result);
                }
                else
                {
                    if (basvuru.TapuIlId != null && tabkisIl.TakbisIlKod != basvuru.TapuIlId)
                    {
                        //tapu_il_id dolu geliyorsa beyandır
                        result.ErrorMessage("Seçtiğiniz Tapu İli ile Hasar Tespit İli Uyuşmadığı İçin İşleminize Devam Edilemiyor. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
                        return await Task.FromResult(result);
                    }
                    else
                    {
                        //tapu_il_id boş geliyorsa tapu seçilerek gelmiştir
                        var tapkbisIlList = _takbisIlRepository.GetAll().ToList().Select(s => StringAddon.ToSlugUrl(s.Ad)).ToList();
                        if (!tapkbisIlList.Contains(StringAddon.ToSlugUrl(hasarTespitVeri.Result.Detay.IlAd)))
                        {
                            result.ErrorMessage("Seçtiğiniz Tapu İli ile Hasar Tespit İli Uyuşmadığı İçin İşleminize Devam Edilemiyor. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");

                            return await Task.FromResult(result);
                        }
                    }
                }

                #endregion


                if (basvuruKanali.BasvuruKanalId == BasvuruKanalEnum.EDevlet)
                {
                    var yapiRuhsatiOnaylanmisBasvuruVarMi = _appealRepository.GetWhere(x => x.AktifMi == true && x.SilindiMi == false
                                                                                        && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                                                        && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                                                        && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruReddedilmistir
                                                                                        && x.BinaDegerlendirme.AktifMi == true && x.BinaDegerlendirme.SilindiMi == false
                                                                                        && (
                                                                                        x.BinaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiRuhsatinizOnaylanmistir
                                                                                        || x.BinaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiIlerlemeSeviyesiYuzde20
                                                                                        || x.BinaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiIlerlemeSeviyesiYuzde60
                                                                                        || x.BinaDegerlendirme.BinaDegerlendirmeDurumId == (long)BinaDegerlendirmeDurumEnum.YapiTamamlanmistir
                                                                                        )
                                                                                        && x.HasarTespitAskiKodu == hasarTespitVeri.Result.Detay.AskiKodu.Trim()
                                                                                        && x.UavtMahalleNo == request.Model.UavtMahalleNo 
                        ).Any();
                    if (yapiRuhsatiOnaylanmisBasvuruVarMi)
                    {
                        result.ErrorMessage($"Başvuru Yapılan Hasar Tespit Askı Koduna Ait '{BinaDegerlendirmeDurumEnum.YapiRuhsatinizOnaylanmistir.GetDisplayName()}' Durumuna Sahip Başvuru Olduğu İçin İşleminize Devam Edilemiyor. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");

                        return await Task.FromResult(result);
                    }
                }


                basvuru.HasarTespitUid = basvuru.HasarTespitUid?.Trim();
                basvuru.HasarTespitAskiKodu = hasarTespitVeri.Result.Detay.AskiKodu.Trim();
                basvuru.HasarTespitHasarDurumu = hasarTespitVeri.Result.Detay.HasarDurumu?.Trim();
                basvuru.HasarTespitItirazSonucu = hasarTespitVeri.Result.Detay.ItirazSonucu?.Trim();
                basvuru.HasarTespitGuclendirmeMahkemeSonucu = hasarTespitVeri.Result.Detay.GuclendirmeMahkemeSonucu?.Trim();
                basvuru.HasarTespitIlAdi = hasarTespitVeri.Result.Detay.IlAd?.Trim();
                basvuru.HasarTespitIlceAdi = hasarTespitVeri.Result.Detay.IlceAd?.Trim();
                basvuru.HasarTespitMahalleAdi = hasarTespitVeri.Result.Detay.MahalleAd?.Trim();
                basvuru.HasarTespitAda = hasarTespitVeri.Result.Detay.AdaNo?.Trim();
                basvuru.HasarTespitParsel = hasarTespitVeri.Result.Detay.ParselNo?.Trim();
                basvuru.HasarTespitDisKapiNo = hasarTespitVeri.Result.Detay.DisKapiNo?.Trim();

                #endregion

                #region ...: Başvuru Tapu Bilgi :...

                if (request?.Model?.BasvuruTapuBilgiListe?.Any() == true)
                {
                    basvuru.BasvuruTapuBilgis ??= new List<BasvuruTapuBilgi>();

                    foreach (var basvuruTapuBilgi in request.Model.BasvuruTapuBilgiListe)
                    {
                        basvuru.BasvuruTapuBilgis.Add(_mapper.Map<BasvuruTapuBilgi>(basvuruTapuBilgi));
                    }
                }

                #endregion

                #region ...: Başvuru Dosyaları :...

                if (request?.Model?.BasvuruDosyaListe?.Any() == true)
                {
                    basvuru.BasvuruDosyas ??= new List<BasvuruDosya>();

                    foreach (var basvuruDosyaItem in request.Model.BasvuruDosyaListe)
                    {
                        var dosyaTurId = _basvuruDosyaTurRepository.GetWhere(x => x.BasvuruDosyaTurGuid == basvuruDosyaItem.DosyaTurGuid).FirstOrDefault()?.BasvuruDosyaTurId;

                        var basvuruDosya = new BasvuruDosya
                        {
                            BasvuruId = basvuru.BasvuruId,
                            BasvuruDosyaTurId = dosyaTurId ?? BasvuruDosyaTurEnum.TapuFotografi,
                            AktifMi = true,
                            SilindiMi = false
                        };

                        if (!string.IsNullOrWhiteSpace(basvuruDosyaItem.DosyaBase64))
                        {
                            //appealFiles.FileName = string.Concat(Guid.NewGuid(), request.AppialCommandModel.AppealFile.FileName);

                            // validator' e alındı.
                            //var fileExtention = basvuruDosyaItem.DosyaUzanti.Substring(0, 1) == "." ? basvuruDosyaItem.DosyaUzanti : null;

                            //if (fileExtention == null)
                            //{
                            //    result.ErrorMessage("Geçersiz veya Hatalı Dosya Seçimi. Lütfen Geçerli Bir Dosya Seçerek İşleme Devam Ediniz.");
                            //    return await Task.FromResult(result);
                            //}

                            basvuruDosya.DosyaAdi = string.Concat(Guid.NewGuid(), basvuruDosyaItem.DosyaUzanti);
                            //basvuruDosya.DosyaYolu = _configuration.GetSection("UploadFile:Path").Value;
                            basvuruDosya.DosyaTuru = MimeTypes.GetMimeType(basvuruDosyaItem.DosyaUzanti);

                            //var filePath = string.Concat(appealFiles.FilePath, "\\", appealFiles.FileName);
                            byte[] data = Convert.FromBase64String(basvuruDosyaItem.DosyaBase64);
                            // string decodedString = Encoding.UTF8.GetString(data);

                            var isTheFileTypeAllowed = FileTypeVerifier.Verify(data);

                            if (!isTheFileTypeAllowed.IsVerified)
                            {
                                result.ErrorMessage("Geçersiz veya Hatalı Dosya Uzantısı.");

                                return await Task.FromResult(result);
                            }

                            basvuruDosya.DosyaYolu = DateTime.Now.ToString("yyyy-MM-dd");

                            var uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", basvuruDosya.DosyaYolu);

                            if (!Directory.Exists(uploadDirectoryPath))
                                Directory.CreateDirectory(uploadDirectoryPath);

                            var filePath = string.Concat(uploadDirectoryPath, "\\", basvuruDosya.DosyaAdi);

                            using var stream = File.Create(filePath);

                            stream.Write(data, 0, data.Length);

                            basvuru.BasvuruDosyas.Add(basvuruDosya);
                        }
                    }
                }
                if (request?.Model?.BasvuruTuzelYetkiDosyaListe?.Any() == true)
                {
                    basvuru.BasvuruDosyas ??= new List<BasvuruDosya>();

                    foreach (var basvuruDosyaItem in request.Model.BasvuruTuzelYetkiDosyaListe)
                    {
                        var basvuruDosya = new BasvuruDosya
                        {
                            BasvuruId = basvuru.BasvuruId,
                            BasvuruDosyaTurId = BasvuruDosyaTurEnum.TuzelKisilikYetkiliOldugunuGosterirBelge,
                            AktifMi = true,
                            SilindiMi = false
                        };

                        if (!string.IsNullOrWhiteSpace(basvuruDosyaItem.DosyaBase64))
                        {
                            //appealFiles.FileName = string.Concat(Guid.NewGuid(), request.AppialCommandModel.AppealFile.FileName);

                            // validator' e alındı.
                            //var fileExtention = basvuruDosyaItem.DosyaUzanti.Substring(0, 1) == "." ? basvuruDosyaItem.DosyaUzanti : null;

                            //if (fileExtention == null)
                            //{
                            //    result.ErrorMessage("Geçersiz veya Hatalı Dosya Seçimi. Lütfen Geçerli Bir Dosya Seçerek İşleme Devam Ediniz.");
                            //    return await Task.FromResult(result);
                            //}

                            basvuruDosya.DosyaAdi = string.Concat(Guid.NewGuid(), basvuruDosyaItem.DosyaUzanti);
                            //basvuruDosya.DosyaYolu = _configuration.GetSection("UploadFile:Path").Value;
                            basvuruDosya.DosyaTuru = MimeTypes.GetMimeType(basvuruDosyaItem.DosyaUzanti);

                            //var filePath = string.Concat(appealFiles.FilePath, "\\", appealFiles.FileName);
                            byte[] data = Convert.FromBase64String(basvuruDosyaItem.DosyaBase64);
                            // string decodedString = Encoding.UTF8.GetString(data);

                            var isTheFileTypeAllowed = FileTypeVerifier.Verify(data);

                            if (!isTheFileTypeAllowed.IsVerified)
                            {
                                result.ErrorMessage("Geçersiz veya Hatalı Dosya Uzantısı.");

                                return await Task.FromResult(result);
                            }

                            basvuruDosya.DosyaYolu = DateTime.Now.ToString("yyyy-MM-dd");

                            var uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", basvuruDosya.DosyaYolu);

                            if (!Directory.Exists(uploadDirectoryPath))
                                Directory.CreateDirectory(uploadDirectoryPath);

                            var filePath = string.Concat(uploadDirectoryPath, "\\", basvuruDosya.DosyaAdi);

                            using var stream = File.Create(filePath);

                            stream.Write(data, 0, data.Length);

                            basvuru.BasvuruDosyas.Add(basvuruDosya);
                        }
                    }
                }

                #endregion

                #region ...: Kişi Ad Soyad Boş İse KPS Den Al :...
                if (string.IsNullOrWhiteSpace(basvuru.Ad) || string.IsNullOrWhiteSpace(basvuru.Soyad))
                {
                    var kpsKisiBilgileri = await _mediator.Send(new GetirKisiAdSoyadTcDenQuery { TcKimlikNo = long.Parse(basvuru.TcKimlikNo), MaskelemeKapaliMi = true });
                    if (!kpsKisiBilgileri.IsError)
                    {
                        if (kpsKisiBilgileri.Result.OlumTarih != null)
                        {
                            result.ErrorMessage("Başvuracak Kişi Vefat Ettiği İçin Başvuruya Devam Edemezsiniz.");
                            return await Task.FromResult(result);
                        }

                        basvuru.Ad = kpsKisiBilgileri.Result.Ad;
                        basvuru.Soyad = kpsKisiBilgileri.Result.Soyad;
                    }
                }
                #endregion

                #region ...: Başvuru Huid Atama :...

                var basvuruHuId = await _mediator.Send(new GetHuIdQuery() { HasarTespitAskiKodu = basvuru.HasarTespitAskiKodu, IcKapiNo = basvuru.UavtIcKapiNo });
                basvuru.Huid = basvuruHuId.Result?.Replace("\"", "").Trim();
                basvuru.HuidKontrolTarihi = DateTime.Now;

                #endregion

                await _appealRepository.AddAsync(basvuru);

                await _appealRepository.SaveChanges(cancellationToken);

                #region ...: SignalR Hub :...

                await _hubContext.Clients.All.SendAsync("basvuruSayisi", new
                {
                    basvuruSayisi = _appealRepository.Count(p => p.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzAlinmistir && p.AktifMi == true && !p.SilindiMi)
                });

                var basvuranKisiSayisi = await _mediator.Send(new GetirBasvuranKisiSayisiQuery());
                if (!basvuranKisiSayisi.IsError)
                {
                    await _hubContext.Clients.All.SendAsync("basvuranSayisi", new
                    {
                        basvuranKisiSayisi.Result.BasvuranGercekKisiSayisi,
                        basvuranKisiSayisi.Result.BasvuranTuzelKisiSayisi
                    });
                }

                #endregion

                result.Result = new EkleBasvuruCommandResponseModel
                {
                    BasvuruId = basvuru.BasvuruGuid,
                    Mesaj = "Başvuru İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.",
                    BasvuruKodu = basvuru.BasvuruKodu
                };
            }
            catch (IOException ex)
            {
                result.Exception(ex, "Başvuru Dosyanız Yüklenirken Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz.");
            }
            catch (ArgumentNullException ex)
            {
                result.Exception(ex, "Başvuru İşleminiz Sırasında Bir Hata Meydana Geldi. Hatalı veya Geçersiz (Boş) Bilgi Gönderildi. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
            }
            catch (ArgumentException ex)
            {
                result.Exception(ex, "Başvuru İşleminiz Sırasında Bir Hata Meydana Geldi. Hatalı veya Geçersiz Bilgi Gönderildi. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    result.Exception(ex, "Başvuru İşleminiz Sırasında Bir Hata Meydana Geldi. Hatalı veya Geçersiz İlişkili Bilgi Gönderildi. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
                }
                else
                {
                    result.Exception(ex, "Başvuru İşleminiz Sırasında Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz.");
                }
            }

            return await Task.FromResult(result);
        }

        private void ValidationInputValue(EkleBasvuruCommandModel model, ResultModel<EkleBasvuruCommandResponseModel> result, ResultModel<AyarDto> ayar)
        {
            var basvuruDestekTur = _basvuruDestekTurRepository.GetWhere(x => x.BasvuruDestekTurGuid == Guid.Parse(model.BasvuruDestekTurId), true).FirstOrDefault();
            if (basvuruDestekTur == null)
            {
                result.ErrorMessage("Başvuru Destek Türü Alınamadı. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
                return;
            }

            var basvuruTur = _basvuruTurRepository.GetWhere(x => x.BasvuruTurGuid == Guid.Parse(model.BasvuruTurId), true).FirstOrDefault();
            if (basvuruTur == null)
            {
                result.Exception(new ArgumentNullException("Başvuru Türü Alınamadı. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz."), "Başvuru Türü Alınamadı. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
                return;
            }

            if (basvuruDestekTur.BasvuruDestekTurId == BasvuruDestekTurEnum.Hibe || basvuruDestekTur.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi)
            {
                #region ...: Hibe Kontrolü :...

                var queryHibeBasvuru = _appealRepository.GetWhere(x => 
                            x.TcKimlikNo == model.TcKimlikNo.Trim()
                            &&
                            (x.BasvuruDestekTurId == BasvuruDestekTurEnum.Hibe || x.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi)
                            &&
                            !(x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruIptalEdildi || x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir)
                            &&
                            x.SilindiMi == false
                        , true);

                if (model.TuzelKisiTipId != null)
                    queryHibeBasvuru = queryHibeBasvuru.Where(x =>
                        x.TuzelKisiTipId != null
                        &&
                        x.TuzelKisiMersisNo == model.TuzelKisiMersisNo
                    );
                else
                    queryHibeBasvuru = queryHibeBasvuru.Where(x =>
                        
                        x.TuzelKisiTipId == null
                    );

                if (basvuruTur.BasvuruTurId == BasvuruTurEnum.Ticarethane)
                {
                    #region ...: Ticarethane Hibe Kontrolü :...

                    if (queryHibeBasvuru.Count(x => x.BasvuruTurId == BasvuruTurEnum.Ticarethane) >= ayar.Result.Basvuru.EnFazlaTicarethaneHibeSayisi)
                    {
                        result.Exception(new ArgumentNullException($"İş Yeri İçin Başvurabileceğiniz En Fazla Hibe Başvuru Sayısına ({ayar.Result.Basvuru.EnFazlaTicarethaneHibeSayisi} adet) Ulaştığınız İçin Tekrar Başvuru Yapamazsınız."), $"İş Yeri İçin Başvurabileceğiniz En Fazla Hibe Başvuru Sayısına ({ayar.Result.Basvuru.EnFazlaTicarethaneHibeSayisi} adet) Ulaştığınız İçin Tekrar Başvuru Yapamazsınız.");

                        return;
                    }

                    #endregion
                }
                else
                {
                    #region ...: Ev Hibe Kontrolü :...

                    if (queryHibeBasvuru.Count(x => x.BasvuruTurId != BasvuruTurEnum.Ticarethane) >= ayar.Result.Basvuru.EnFazlaEvHibeSayisi)
                    {
                        result.Exception(new ArgumentNullException($"Konut İçin Başvurabileceğiniz En Fazla Hibe Başvuru Sayısına {ayar.Result.Basvuru.EnFazlaEvHibeSayisi} Ulaştığınız İçin Tekrar Başvuru Yapamazsınız."), $"Konut İçin Başvurabileceğiniz En Fazla Hibe Başvuru Sayısına ({ayar.Result.Basvuru.EnFazlaEvHibeSayisi} adet) Ulaştığınız İçin Tekrar Başvuru Yapamazsınız.");

                        return;
                    }

                    #endregion
                }

                #endregion
            }

            if (basvuruDestekTur.BasvuruDestekTurId == BasvuruDestekTurEnum.Kredi || basvuruDestekTur.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi)
            {
                #region ...: Kredi Kontrolü :...

                var queryKrediBasvuru = _appealRepository.GetWhere(x =>
                            x.TcKimlikNo == model.TcKimlikNo.Trim()
                    &&
                        (x.BasvuruDestekTurId == BasvuruDestekTurEnum.Kredi || x.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi)
                    &&
                        !(x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruIptalEdildi || x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir)
                    &&
                        x.SilindiMi == false
                , true);

                if (model.TuzelKisiTipId != null)
                    queryKrediBasvuru = queryKrediBasvuru.Where(x =>
                        x.TuzelKisiTipId != null
                        &&
                        x.TuzelKisiMersisNo == model.TuzelKisiMersisNo
                    );
                else
                    queryKrediBasvuru = queryKrediBasvuru.Where(x =>
                        x.TuzelKisiTipId == null
                    );

                if (ayar.Result.Basvuru.EnFazlaKrediSayisi > 0)
                {
                    if (queryKrediBasvuru.Count() >= ayar.Result.Basvuru.EnFazlaKrediSayisi)
                    {
                        result.Exception(new ArgumentNullException($"Başvurabileceğiniz En Fazla Kredi Başvuru Sayısına ({ayar.Result.Basvuru.EnFazlaKrediSayisi} adet) Ulaştığınız İçin Tekrar Başvuru Yapamazsınız."), $"Başvurabileceğiniz En Fazla Kredi Başvuru Sayısına ({ayar.Result.Basvuru.EnFazlaKrediSayisi} adet) Ulaştığınız İçin Tekrar Başvuru Yapamazsınız.");
                        return;
                    }
                }
                else
                {
                    if (basvuruTur.BasvuruTurId == BasvuruTurEnum.Ticarethane)
                    {
                        #region ...: Ticarethane Kredi Kontrolü :...
                        if (queryKrediBasvuru.Count(x => x.BasvuruTurId == BasvuruTurEnum.Ticarethane) >= ayar.Result.Basvuru.EnFazlaTicarethaneKrediSayisi)
                        {
                            result.Exception(new ArgumentNullException($"İş Yeri İçin Başvurabileceğiniz En Fazla Kredi Başvuru Sayısına ({ayar.Result.Basvuru.EnFazlaTicarethaneKrediSayisi} adet) Ulaştığınız İçin Tekrar Başvuru Yapamazsınız."), $"İş Yeri İçin Başvurabileceğiniz En Fazla Kredi Başvuru Sayısına ({ayar.Result.Basvuru.EnFazlaTicarethaneKrediSayisi} adet) Ulaştığınız İçin Tekrar Başvuru Yapamazsınız.");
                            return;
                        }
                        #endregion
                    }
                    else
                    {
                        #region ...: Ev Kredi Kontrolü :...
                        if (queryKrediBasvuru.Count(x => x.BasvuruTurId != BasvuruTurEnum.Ticarethane) >= ayar.Result.Basvuru.EnFazlaEvKrediSayisi)
                        {
                            result.Exception(new ArgumentNullException($"Konut İçin Başvurabileceğiniz En Fazla Kredi Başvuru Sayısına ({ayar.Result.Basvuru.EnFazlaEvKrediSayisi} adet) Ulaştığınız İçin Tekrar Başvuru Yapamazsınız."), $"Konut İçin Başvurabileceğiniz En Fazla Kredi Başvuru Sayısına ({ayar.Result.Basvuru.EnFazlaEvKrediSayisi} adet) Ulaştığınız İçin Tekrar Başvuru Yapamazsınız.");
                            return;
                        }
                        #endregion
                    }
                }

                #endregion
            }

            //if (model.BasvuruDosyaListe?.Any() == true)
            //{
            //    foreach (var basvuruDosyaItem in model.BasvuruDosyaListe)
            //    {
            //        if (string.IsNullOrWhiteSpace(basvuruDosyaItem.DosyaUzanti))
            //        {
            //            result.Exception(new ArgumentNullException("Dosya Uzantı Bilgisi Boş Olamaz. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz."), "Dosya Uzantı Bilgisi Boş Olamaz. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");

            //            return;
            //        }

            //        if (string.IsNullOrWhiteSpace(basvuruDosyaItem.DosyaBase64))
            //        {
            //            result.Exception(new ArgumentNullException("Dosya İçeriği Boş Olamaz. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz."), "Dosya İçeriği Boş Olamaz. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");

            //            return;
            //        }
            //    }
            //}
        }

    }
}