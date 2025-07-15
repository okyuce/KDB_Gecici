using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeMalikler;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetBelge;

public class KaydetBelgeCommand : IRequest<ResultModel<KaydetBelgeCommandResponseModel>>
{
    public long? BinaDegerlendirmeId { get; set; }
    public long? BasvuruId { get; set; }
    public long? BasvuruKamuUstlenecekId { get; set; }
    public DosyaDto? BasvuruImzaDosya { get; set; }
    public int? ImzalayanKisiSayisi { get; set; }

    public class KaydetBelgeCommandHandler : IRequestHandler<KaydetBelgeCommand, ResultModel<KaydetBelgeCommandResponseModel>>
    {
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IConfiguration _configuration;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IMediator _mediator;

        public KaydetBelgeCommandHandler(IBasvuruRepository basvuruRepository
            , IBinaDegerlendirmeRepository binaDegerlendirmeRepository
            , IKullaniciBilgi kullaniciBilgi
            , IConfiguration configuration
            , IMediator mediator)
        {
            _configuration = configuration;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _kullaniciBilgi = kullaniciBilgi;
            _basvuruRepository = basvuruRepository;
            _mediator = mediator;
        }

        public async Task<ResultModel<KaydetBelgeCommandResponseModel>> Handle(KaydetBelgeCommand request, CancellationToken cancellationToken)
        {
            ResultModel<KaydetBelgeCommandResponseModel> result = new();

            var binaDegerlendirme = await _binaDegerlendirmeRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId
                                                , asNoTracking: false,
                                                i => i.BinaDegerlendirmeDosyas.Where(x => x.AktifMi == true && !x.SilindiMi && x.BinaDegerlendirmeDosyaTurId == (long)BinaDegerlendirmeDosyaTurEnum.BinaDegerlendirme)
                                            ).FirstOrDefaultAsync();

            if (binaDegerlendirme == null)
            {
                result.ErrorMessage("Bina Değerlendirme bilgisi bulunamadı.");
                return await Task.FromResult(result);
            }

            var malikList = (await _mediator.Send(new GetirListeMaliklerQuery()
            {
                TapuAda = binaDegerlendirme.Ada,
                TapuParsel = binaDegerlendirme.Parsel,
                UavtMahalleNo = binaDegerlendirme.UavtMahalleNo,
                HasarTespitAskiKodu = binaDegerlendirme.HasarTespitAskiKodu,
                IptalEdilenlerAlinmasin = true
            }));

            malikList.Result = malikList.Result.Where(x => x.BinaDisKapiNo == binaDegerlendirme.BinaDisKapiNo).ToList();

            if (malikList.Result.Count != request.ImzalayanKisiSayisi)
            {
                result.ErrorMessage("Bina Değerlendirme bilgilerinde malik sayısı imza veren sayısından fazla olamaz.");
                return await Task.FromResult(result);
            }

            //if (binaDegerlendirme.BinaDegerlendirmeDosyas.Any())
            //{
            //    result.ErrorMessage("'Belge Bilgileri' alanında güncelleme yapılamaz.");
            //    return await Task.FromResult(result);
            //}

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            long.TryParse(kullaniciBilgi.KullaniciId, out long kullaniciId);

            #region Dosya Ekleme/Güncelleme İşlemleri
            if (!binaDegerlendirme.BinaDegerlendirmeDosyas.Any() && request?.BasvuruImzaDosya == null)
            {
                result.ErrorMessage("'İnşaat Yapım Taahhüt Sözleşme Belgesi' alanı boş olamaz.");
                return await Task.FromResult(result);
            }

            var binaDegerlendirmeDosya = binaDegerlendirme.BinaDegerlendirmeDosyas.FirstOrDefault();
            binaDegerlendirmeDosya ??= new BinaDegerlendirmeDosya
            {
                AktifMi = true,
                SilindiMi = false,
            };

            if (request.BasvuruImzaDosya != null)
            {
                binaDegerlendirmeDosya.DosyaAdi = string.Concat(Guid.NewGuid(), request.BasvuruImzaDosya?.DosyaUzanti);
                binaDegerlendirmeDosya.DosyaYolu = DateTime.Now.ToString("yyyy-MM-dd");
                binaDegerlendirmeDosya.DosyaTuru = MimeTypes.GetMimeType(request.BasvuruImzaDosya?.DosyaUzanti ?? "");
                binaDegerlendirmeDosya.BinaDegerlendirmeDosyaTurId = (long)BinaDegerlendirmeDosyaTurEnum.BinaDegerlendirme;

                byte[] data = Convert.FromBase64String(request.BasvuruImzaDosya?.DosyaBase64);

                if (!FileTypeVerifier.Verify(data).IsVerified)
                {
                    result.ErrorMessage("Geçersiz veya Hatalı Dosya Uzantısı.");
                    return await Task.FromResult(result);
                }

                string uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", binaDegerlendirmeDosya.DosyaYolu);

                if (!Directory.Exists(uploadDirectoryPath))
                    Directory.CreateDirectory(uploadDirectoryPath);

                string filePath = string.Concat(uploadDirectoryPath, "\\", binaDegerlendirmeDosya.DosyaAdi);
                using FileStream stream = File.Create(filePath);
                stream.Write(data, 0, data.Length);

                binaDegerlendirme.BinaDegerlendirmeDosyas.Add(binaDegerlendirmeDosya);
            }
            #endregion

            binaDegerlendirme.ImzalayanKisiSayisi = request.ImzalayanKisiSayisi;
            binaDegerlendirme.GuncelleyenKullaniciId = kullaniciId;
            binaDegerlendirme.GuncellemeTarihi = DateTime.Now;
            binaDegerlendirme.GuncelleyenIp = kullaniciBilgi.IpAdresi;
            binaDegerlendirme.BinaDegerlendirmeDurumId = (long)BinaDegerlendirmeDurumEnum.InsaatSozlesmesiImzalanmistir;

            _binaDegerlendirmeRepository.Update(binaDegerlendirme);
            await _binaDegerlendirmeRepository.SaveChanges();

            result.Result = new KaydetBelgeCommandResponseModel
            {
                Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.",
                BinaDosyaGuid = binaDegerlendirmeDosya.BinaDosyaGuid,
                DosyaAdi = binaDegerlendirmeDosya.DosyaAdi,
            };

            return await Task.FromResult(result);
        }
    }
}