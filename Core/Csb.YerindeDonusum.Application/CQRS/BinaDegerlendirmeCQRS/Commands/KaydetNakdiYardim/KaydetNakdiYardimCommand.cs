using Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeMalikler;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Commands.KaydetNakdiYardim;

public class KaydetNakdiYardimCommand : IRequest<ResultModel<string>>
{
    public long? BinaDegerlendirmeId { get; set; }
    public long? NakdiYardimTaksitYuzdesi { get; set; }
    public DosyaDto? BelgeNakdiYardimTaksitDosya { get; set; }

    public class KaydetNakdiYardimCommandHandler : IRequestHandler<KaydetNakdiYardimCommand, ResultModel<string>>
    {

        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IBinaNakdiYardimTaksitRepository _binaNakdiYardimTaksitRepository;
        private readonly IBinaNakdiYardimTaksitDosyaRepository _binaNakdiYardimTaksitDosyaRepository;
        private readonly IConfiguration _configuration;
        private readonly IKullaniciBilgi _kullaniciBilgi;
        private readonly IMediator _mediator;

        public KaydetNakdiYardimCommandHandler(IBinaDegerlendirmeRepository binaDegerlendirmeRepository
            , IBinaNakdiYardimTaksitRepository binaNakdiYardimTaksitRepository
            , IBinaNakdiYardimTaksitDosyaRepository binaNakdiYardimTaksitDosyaRepository
            , IKullaniciBilgi kullaniciBilgi
            , IMediator mediator
            , IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _binaNakdiYardimTaksitRepository = binaNakdiYardimTaksitRepository;
            _binaNakdiYardimTaksitDosyaRepository = binaNakdiYardimTaksitDosyaRepository;
            _kullaniciBilgi = kullaniciBilgi;
        }

        public async Task<ResultModel<string>> Handle(KaydetNakdiYardimCommand request, CancellationToken cancellationToken)
        {
            ResultModel<string> result = new();

            var binaDegerlendirme = await _binaDegerlendirmeRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                            && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId
                                                , asNoTracking: true
                                            ).FirstOrDefaultAsync();

            if (binaDegerlendirme == null) 
            {
                result.ErrorMessage("Bina Değerlendirme bilgisi bulunamadı.");
                return result;
            }

            // Bütün Malikler İmza Vermemişse İşleme Devam Edilemez.
            var butunMaliklerImzaVerdiMiResult = await DomainServices.ButunMaliklerImzaVerdiMi(_mediator, binaDegerlendirme);
            if (butunMaliklerImzaVerdiMiResult.IsError)
            {
                result.ErrorMessage(butunMaliklerImzaVerdiMiResult.ErrorMessageContent);
                return result;
            }

            long.TryParse(_kullaniciBilgi.GetUserInfo().KullaniciId, out long kullaniciId);
            string? ipAdresi = _kullaniciBilgi.GetUserInfo()?.IpAdresi;

            #region Nakdi Yardım Taksit Ekleme / Güncelleme

            BinaNakdiYardimTaksit? binaNakdiYardimTaksit = await _binaNakdiYardimTaksitRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                            && x.TaksitYuzdesi == request.NakdiYardimTaksitYuzdesi
                                            && x.BinaDegerlendirmeId == binaDegerlendirme.BinaDegerlendirmeId
                                            ).FirstOrDefaultAsync();

            // Update
            if (binaNakdiYardimTaksit != null)
            {
                binaNakdiYardimTaksit.TaksitYuzdesi = request.NakdiYardimTaksitYuzdesi ?? 0;
                binaNakdiYardimTaksit.GuncellemeTarihi = DateTime.Now;
                binaNakdiYardimTaksit.GuncelleyenIp = ipAdresi;
                binaNakdiYardimTaksit.GuncelleyenKullaniciId = kullaniciId;
                _binaNakdiYardimTaksitRepository.Update(binaNakdiYardimTaksit);
            }
            // Add
            else
            {
                binaNakdiYardimTaksit = new BinaNakdiYardimTaksit()
                {
                    AktifMi = true,
                    SilindiMi = false,
                    OlusturmaTarihi = DateTime.Now,
                    OlusturanIp = ipAdresi,
                    OlusturanKullaniciId = kullaniciId,
                    BinaDegerlendirmeId = binaDegerlendirme.BinaDegerlendirmeId,
                    TaksitYuzdesi = request.NakdiYardimTaksitYuzdesi ?? 0,
                };
                await _binaNakdiYardimTaksitRepository.AddAsync(binaNakdiYardimTaksit);
            }

            await _binaNakdiYardimTaksitRepository.SaveChanges();

            #endregion

            #region Nakdi Yardım Taksit Dosya Ekleme / Güncelleme

            // Başvuruya Ait Dosya Var Mı Kontrol Edilir Varsa Güncellenir Yoksa Yeni Yüklenir.
            BinaNakdiYardimTaksitDosya? nakdiYardimTaksitDosya = await _binaNakdiYardimTaksitDosyaRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                            && x.BinaNakdiYardimTaksitId == binaNakdiYardimTaksit.BinaNakdiYardimTaksitId
                                            ).FirstOrDefaultAsync();

            // File Exists Add Or Update
            if (request?.BelgeNakdiYardimTaksitDosya != null)
            {
                nakdiYardimTaksitDosya ??= new BinaNakdiYardimTaksitDosya
                {
                    AktifMi = true,
                    SilindiMi = false,
                    BinaNakdiYardimTaksitId = binaNakdiYardimTaksit.BinaNakdiYardimTaksitId,
                };

                nakdiYardimTaksitDosya.DosyaAdi = string.Concat(Guid.NewGuid(), request.BelgeNakdiYardimTaksitDosya.DosyaUzanti);
                nakdiYardimTaksitDosya.DosyaYolu = DateTime.Now.ToString("yyyy-MM-dd");
                nakdiYardimTaksitDosya.DosyaTuru = MimeTypes.GetMimeType(request.BelgeNakdiYardimTaksitDosya.DosyaUzanti ?? "");

                byte[] data = Convert.FromBase64String(request.BelgeNakdiYardimTaksitDosya.DosyaBase64);

                if (!FileTypeVerifier.Verify(data).IsVerified)
                {
                    result.ErrorMessage("Geçersiz veya Hatalı Dosya Uzantısı.");

                    return await Task.FromResult(result);
                }

                string uploadDirectoryPath = string.Concat(_configuration.GetSection("UploadFile:Path").Value!, "\\", nakdiYardimTaksitDosya.DosyaYolu);

                if (!Directory.Exists(uploadDirectoryPath))
                    Directory.CreateDirectory(uploadDirectoryPath);

                string filePath = string.Concat(uploadDirectoryPath, "\\", nakdiYardimTaksitDosya.DosyaAdi);

                using FileStream stream = File.Create(filePath);

                stream.Write(data, 0, data.Length);

                _binaNakdiYardimTaksitDosyaRepository.Update(nakdiYardimTaksitDosya);
                await _binaNakdiYardimTaksitDosyaRepository.SaveChanges();
            }
            #endregion

            result.Result = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır.";

            return await Task.FromResult(result);
        }

    }
}