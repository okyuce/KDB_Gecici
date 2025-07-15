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

namespace Csb.YerindeDonusum.Application.CQRS.YeniYapiCQRS.Commands.GuncelleYeniYapi;

public class GuncelleYeniYapiCommand : IRequest<ResultModel<GuncelleYeniYapiCommandResponseModel>>
{
    public long? BinaDegerlendirmeId { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public string? Ada { get; set; }
    public string? Parsel { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? BinaDisKapiNo { get; set; }

    public class GuncelleYeniYapiCommandHandler : IRequestHandler<GuncelleYeniYapiCommand, ResultModel<GuncelleYeniYapiCommandResponseModel>>
    {
        private readonly IBinaDegerlendirmeRepository _binaDegerlendirmeRepository;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GuncelleYeniYapiCommandHandler(IBinaDegerlendirmeRepository binaDegerlendirmeRepository, IKullaniciBilgi kullaniciBilgi)
        {
            _binaDegerlendirmeRepository = binaDegerlendirmeRepository;
            _kullaniciBilgi = kullaniciBilgi;
        }

        public async Task<ResultModel<GuncelleYeniYapiCommandResponseModel>> Handle(GuncelleYeniYapiCommand request, CancellationToken cancellationToken)
        {
            ResultModel<GuncelleYeniYapiCommandResponseModel> result = new();

            request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            long.TryParse(kullaniciBilgi.KullaniciId, out long kullaniciId);

            // normalde sadece binadegerlendirmeId yeterli ancak bilerek yanlış bir id gonderilirse diye
            // ada, parsel, aski koduna göre teyit ediyoruz.
            var binaDegerlendirme = await _binaDegerlendirmeRepository.GetWhere(x => x.SilindiMi == false && x.AktifMi == true
                                                        && x.BinaDegerlendirmeDurumId != (long)BinaDegerlendirmeDurumEnum.Reddedildi
                                                        && x.BinaDegerlendirmeId == request.BinaDegerlendirmeId
                                                        && x.UavtMahalleNo == request.UavtMahalleNo
                                                        && (string.IsNullOrWhiteSpace(request.Ada) || x.Ada == request.Ada)
                                                        && (string.IsNullOrWhiteSpace(request.Parsel) || x.Parsel == request.Parsel)
                                                        && (string.IsNullOrEmpty(request.HasarTespitAskiKodu) || x.HasarTespitAskiKodu == request.HasarTespitAskiKodu),
                                                asNoTracking: false
                                            ).FirstOrDefaultAsync();
            if (binaDegerlendirme == null)
            {
                result.ErrorMessage("Bina Değerlendirme bilgisi bulunamadı.");
                return result;
            }

            // bu bina grubu içersinde aynı dış kapıya sahip bir bina varsa işleme devam edilemeyecek.
            // yani aynı ada, parsel / aski koduna sahip diğer binalara bakıyoruz.
            var disKapiVarMi = _binaDegerlendirmeRepository.GetWhere(x => !x.SilindiMi && x.AktifMi == true
                                                            && x.BinaDegerlendirmeId != request.BinaDegerlendirmeId
                                                            && x.BinaDegerlendirmeDurumId != (long)BinaDegerlendirmeDurumEnum.Reddedildi
                                                            && x.BinaDisKapiNo.ToLower() == request.BinaDisKapiNo.Trim().ToLower()
                                                            && x.UavtMahalleNo == request.UavtMahalleNo
                                                            && (string.IsNullOrWhiteSpace(request.Ada) || x.Ada == request.Ada)
                                                            && (string.IsNullOrWhiteSpace(request.Parsel) || x.Parsel == request.Parsel)
                                                            && (string.IsNullOrEmpty(request.HasarTespitAskiKodu) || x.HasarTespitAskiKodu == request.HasarTespitAskiKodu),
                                                asNoTracking: false
                                            ).Any();

            if (disKapiVarMi)
            {
                result.ErrorMessage("Bu Dış Kapı No bilgisine sahip bir Yapı sistemde mevcut.");
                return result;
            }

            // ada/parsel güncelleme eklendi.
            binaDegerlendirme.Ada = request?.Ada?.Trim() ?? binaDegerlendirme.Ada;
            binaDegerlendirme.Parsel = request?.Parsel?.Trim() ?? binaDegerlendirme.Parsel;
            binaDegerlendirme.BinaDisKapiNo = request?.BinaDisKapiNo?.Trim() ?? binaDegerlendirme.BinaDisKapiNo;
            binaDegerlendirme.HasarTespitAskiKodu = request?.HasarTespitAskiKodu ?? binaDegerlendirme.HasarTespitAskiKodu;

            _binaDegerlendirmeRepository.Update(binaDegerlendirme);
            await _binaDegerlendirmeRepository.SaveChanges();

            result.Result = new GuncelleYeniYapiCommandResponseModel
            {
                Mesaj = "İşleminiz Başarılı Bir Şekilde Tamamlanmıştır."
            };

            return await Task.FromResult(result);
        }
    }
}