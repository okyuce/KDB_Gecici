using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Dtos;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Domain.Addons.FileAddons;
using Csb.YerindeDonusum.Domain.Entities;
using CSB.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.EkleYeniYapi;

public class EkleYeniYapiCommand : IRequest<ResultModel<EkleYeniYapiCommandResponseModel>>
{
    public string? HasarTespitAskiKodu { get; set; }
    public string? Ada { get; set; }
    public string? Parsel { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? BinaDisKapiNo { get; set; }

    public class EkleYeniYapiCommandHandler : IRequestHandler<EkleYeniYapiCommand, ResultModel<EkleYeniYapiCommandResponseModel>>
    {
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IConfiguration _configuration;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public EkleYeniYapiCommandHandler(IBasvuruRepository basvuruRepository
            , IBinaDegerlendirmeRepository binaDegerlendirmeRepository
            , IKullaniciBilgi kullaniciBilgi
            , IConfiguration configuration)
        {
            _configuration = configuration;
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _kullaniciBilgi = kullaniciBilgi;
            _basvuruRepository = basvuruRepository;
        }

        public async Task<ResultModel<EkleYeniYapiCommandResponseModel>> Handle(EkleYeniYapiCommand request, CancellationToken cancellationToken)
        {
            ResultModel<EkleYeniYapiCommandResponseModel> result = new();

            var askiList = HasarTespitAddon.AskiKoduToUpper(request?.HasarTespitAskiKodu).Split(",").Select(x => x.Trim()).ToList();

            request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            long.TryParse(kullaniciBilgi.KullaniciId, out long kullaniciId);

            var basvuru = await _basvuruRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                                        && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                        && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                        && x.UavtMahalleNo == request.UavtMahalleNo
                                                        && x.HasarTespitAskiKodu == request.HasarTespitAskiKodu
                                                        && (string.IsNullOrWhiteSpace(request.Ada) || x.TapuAda == request.Ada)
                                                        && (string.IsNullOrWhiteSpace(request.Parsel) || x.TapuParsel == request.Parsel)
                                            , asNoTracking: true
                                            ).FirstOrDefaultAsync();

            if (basvuru == null)
            {
                result.ErrorMessage("Başvuru bilgisi bulunamadı.");
                return result;
            }

            var disKapiVarMi = _binaDegerlendirmeRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                && x.BinaDegerlendirmeDurumId != (long)BinaDegerlendirmeDurumEnum.Reddedildi
                                                && x.BinaDisKapiNo.ToLower() == request.BinaDisKapiNo.Trim().ToLower()
                                                && x.UavtMahalleNo == request.UavtMahalleNo
                                                && (string.IsNullOrWhiteSpace(request.Ada) || x.Ada == request.Ada)
                                                && (string.IsNullOrWhiteSpace(request.Parsel) || x.Parsel == request.Parsel)
                                                && x.HasarTespitAskiKodu == request.HasarTespitAskiKodu
                                        , asNoTracking: false
                                        ).Any();

            if (disKapiVarMi)
            {
                result.ErrorMessage("Bu Dış Kapı No bilgisine sahip bir yapı sistemde mevcut.");
                return result;
            }

            BinaDegerlendirme binaDegerlendirmeEkle = new()
            {
                BinaDisKapiNo = request?.BinaDisKapiNo?.Trim(),
                OlusturanKullaniciId = kullaniciId,
                OlusturmaTarihi = DateTime.Now,
                OlusturanIp = kullaniciBilgi.IpAdresi,
                AktifMi = true,
                SilindiMi = false,
                BinaDegerlendirmeDurumId = (long)BinaDegerlendirmeDurumEnum.BasvurunuzDegerlendirmeyeAlinmistir,
                UavtIlAdi = basvuru.UavtIlAdi,
                UavtIlNo = basvuru.UavtIlNo ?? 0,
                UavtIlceAdi = basvuru.UavtIlceAdi,
                UavtIlceNo = basvuru.UavtIlceNo ?? 0,
                UavtMahalleAdi = basvuru.UavtMahalleAdi,
                UavtMahalleNo = basvuru.UavtMahalleNo ?? 0,
                Ada = request?.Ada?.Trim() ?? "",
                Parsel = request?.Parsel?.Trim() ?? "",
                HasarTespitAskiKodu = request?.HasarTespitAskiKodu ?? "",
            };

            await _binaDegerlendirmeRepository.AddAsync(binaDegerlendirmeEkle);
            await _binaDegerlendirmeRepository.SaveChanges();

            result.Result = new EkleYeniYapiCommandResponseModel
            {
                Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır."
            };

            return await Task.FromResult(result);
        }
    }
}