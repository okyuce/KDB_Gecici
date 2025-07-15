using Csb.YerindeDonusum.Application.CQRS.KdsCQRS.Queries.KdsHasarTespitVeriByUid;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Commands.GuncelleBasvuru;

public class GuncelleBasvuruCommand : IRequest<ResultModel<GuncelleBasvuruCommandResponseModel>>
{
    public GuncelleBasvuruCommandModel? Model { get; set; }

    public class GuncelleBasvuruCommandHandler : IRequestHandler<GuncelleBasvuruCommand, ResultModel<GuncelleBasvuruCommandResponseModel>>
    {
        private readonly IMediator _mediator;
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IConfiguration _configuration;
        private readonly IBasvuruDosyaTurRepository _basvuruDosyaTurRepository;

        public GuncelleBasvuruCommandHandler(IMediator mediator, IBasvuruRepository basvuruRepository, IKullaniciBilgi kullaniciBilgi, IConfiguration configuration, IBasvuruDosyaTurRepository basvuruDosyaTurRepository)
        {
            _mediator = mediator;
            _basvuruRepository = basvuruRepository;
            _kullaniciBilgi = kullaniciBilgi;
            _configuration = configuration;
            _basvuruDosyaTurRepository = basvuruDosyaTurRepository;
        }

        public async Task<ResultModel<GuncelleBasvuruCommandResponseModel>> Handle(GuncelleBasvuruCommand request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GuncelleBasvuruCommandResponseModel>();

            Guid.TryParse(request?.Model?.BasvuruGuid, out Guid guidParse);

            var basvuru = await _basvuruRepository.GetWhere(x => x.BasvuruGuid == guidParse && x.TcKimlikNo == request.Model.TcKimlikNo)
                                                  .Include(x => x.BasvuruDosyas).FirstOrDefaultAsync();

            if (basvuru == null)
            {
                result.ErrorMessage("Başvuru bulunamadı.");
                return await Task.FromResult(result);
            }
            else if (basvuru.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruIptalEdildi || basvuru.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir)
            {
                result.ErrorMessage("Bu başvuru iptal edildiğinden güncelleyemezsiniz.");
                return await Task.FromResult(result);
            }

            var hasarTespitVeri = await _mediator.Send(new KdsHasarTespitVeriByUidQuery
            {
                HasarTespitUid = basvuru.HasarTespitUid?.Trim()
            });

            if (hasarTespitVeri?.IsError == true)
            {
                result.ErrorMessage(hasarTespitVeri.ErrorMessageContent);
                return await Task.FromResult(result);
            }
            else if (!hasarTespitVeri.Result.BasvuruYapabilirMi)
            {
                result.ErrorMessage(hasarTespitVeri.Result?.BilgilendirmeMesaji);
                return await Task.FromResult(result);
            }
            else if (hasarTespitVeri?.Result?.Detay?.IlKod != request?.Model?.UavtIlNo)
            {
                result.ErrorMessage("UAVT İli ve Askı Kodu İli Aynı Değil. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
                return await Task.FromResult(result);
            }
            else if (hasarTespitVeri?.Result?.Detay?.IlceKod != request?.Model?.UavtIlceNo)
            {
                result.ErrorMessage("UAVT İlçesi ve Askı Kodu İlçesi Aynı Değil. Lütfen Bilgilerinizi Kontrol Ederek Yeniden Deneyiniz.");
                return await Task.FromResult(result);
            }

            basvuru.UavtAdresNo = request?.Model?.UavtAdresNo;
            basvuru.UavtIlNo = request?.Model?.UavtIlNo;
            basvuru.UavtIlKodu = request?.Model?.UavtIlNo?.ToString();
            basvuru.UavtIlAdi = request?.Model?.UavtIlAdi;

            basvuru.UavtIlceNo = request?.Model?.UavtIlceNo;
            basvuru.UavtIlceAdi = request?.Model?.UavtIlceAdi;

            basvuru.UavtMahalleNo = request?.Model?.UavtMahalleNo;
            basvuru.UavtMahalleKodu = request?.Model?.UavtMahalleNo?.ToString();
            basvuru.UavtMahalleAdi = request?.Model?.UavtMahalleAdi;

            basvuru.UavtCaddeNo = request?.Model?.UavtCaddeNo;
            basvuru.UavtCsbm = request?.Model?.UavtCsbm;
            basvuru.UavtDisKapiNo = request?.Model?.UavtDisKapiNo;
            basvuru.UavtMeskenBinaNo = request?.Model?.UavtMeskenBinaNo;
            basvuru.UavtIcKapiNo = request?.Model?.UavtIcKapiNo;
            basvuru.UavtBeyanMi = request?.Model?.UavtBeyanMi;

            //tapu beyan ise ve hazine arazisi değiştirilmek isteniyorsa değişsin -> üzerine intikal olanı hazine olarak değiştirebilecekler ya da tam tersine yapabilecekler
            if (basvuru.TapuTasinmazId == null && request?.Model?.TapuHazineArazisiMi != null)
                basvuru.TapuHazineArazisiMi = request.Model.TapuHazineArazisiMi;

            var userInfo = _kullaniciBilgi.GetUserInfo();

            long.TryParse(userInfo.KullaniciId, out long kullaniciId);

            basvuru.GuncelleyenKullaniciId = kullaniciId;
            basvuru.GuncellemeTarihi = DateTime.Now;
            basvuru.GuncelleyenIp = userInfo.IpAdresi;

            #region ...: Başvuru Dosyaları :...
            var basvuruDosyasi = basvuru.BasvuruDosyas.Where(x => x.BasvuruId == basvuru.BasvuruId && x.BasvuruDosyaTurId == BasvuruDosyaTurEnum.HazineArazisiMuhtarBeyanBelgesi).FirstOrDefault();

            var requestBasvuruDosyasi = request.Model?.BasvuruDosyaListe?.FirstOrDefault();

            if (request?.Model?.BasvuruDosyaListe?.Any() == true && basvuru.UavtBeyanMi == true)
            {
                var dosyaTurId = _basvuruDosyaTurRepository.GetWhere(x => x.BasvuruDosyaTurGuid == requestBasvuruDosyasi.DosyaTurGuid).FirstOrDefault()?.BasvuruDosyaTurId;

                basvuruDosyasi ??= new BasvuruDosya
                {
                    BasvuruId = basvuru.BasvuruId,
                    BasvuruDosyaTurId = dosyaTurId ?? BasvuruDosyaTurEnum.TapuFotografi,
                    AktifMi = true,
                    SilindiMi = false
                };

                if (!string.IsNullOrWhiteSpace(requestBasvuruDosyasi.DosyaBase64))
                {
                    basvuruDosyasi.DosyaAdi = string.Concat(Guid.NewGuid(), requestBasvuruDosyasi.DosyaUzanti);
                    basvuruDosyasi.DosyaTuru = MimeTypes.GetMimeType(requestBasvuruDosyasi.DosyaUzanti);

                    byte[] data = Convert.FromBase64String(requestBasvuruDosyasi.DosyaBase64);

                    var isTheFileTypeAllowed = FileTypeVerifier.Verify(data);

                    if (!isTheFileTypeAllowed.IsVerified)
                    {
                        result.ErrorMessage("Geçersiz veya Hatalı Dosya Uzantısı.");

                        return await Task.FromResult(result);
                    }

                    basvuruDosyasi.DosyaYolu = DateTime.Now.ToString("yyyy-MM-dd");

                    var uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", basvuruDosyasi.DosyaYolu);

                    if (!Directory.Exists(uploadDirectoryPath))
                        Directory.CreateDirectory(uploadDirectoryPath);

                    var filePath = string.Concat(uploadDirectoryPath, "\\", basvuruDosyasi.DosyaAdi);

                    using var stream = File.Create(filePath);

                    stream.Write(data, 0, data.Length);

                    basvuru.BasvuruDosyas.Add(basvuruDosyasi);
                }
            }
            else if (basvuruDosyasi != null)
            {
                basvuruDosyasi.AktifMi = false;
                basvuruDosyasi.SilindiMi = true;
            }
            #endregion

            await _basvuruRepository.SaveChanges(cancellationToken);

            result.Result = new GuncelleBasvuruCommandResponseModel
            {
                BasvuruGuid = basvuru.BasvuruGuid,
                Mesaj = "Başvuru İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.",
                BasvuruKodu = basvuru.BasvuruKodu
            };

            return await Task.FromResult(result);
        }
    }
}