using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using CSB.Core.Entities.Responses;
using CSB.Core.Integration.KPS.Services;
using CSB.Core.LogHandler.Abstraction;
using KPSKimlikNoTemelKisiBilgiService;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiAdSoyadTcDen;

public class GetirKisiAdSoyadTcDenQuery : IRequest<ResultModel<GetirKisiAdSoyadTcDenQueryResponseModel>>, ICacheMediatrQuery
{
    public long? TcKimlikNo { get; set; }
    public bool? MaskelemeKapaliMi { get; set; }

    #region Cache Ayar
    public bool? CacheCustomUser => null;
    public int? CacheMinute => null;
    public bool CacheIsActive => true;
    #endregion

    public class GetirKisiAdSoyadTcDenQueryHandler : IRequestHandler<GetirKisiAdSoyadTcDenQuery, ResultModel<GetirKisiAdSoyadTcDenQueryResponseModel>>
    {
        private readonly IKPSKimlikNoTemelKisiBilgiService _kPSKimlikNoTemelKisiBilgiService;
        private readonly IIntegrationLogService _logService;

        public GetirKisiAdSoyadTcDenQueryHandler(IKPSKimlikNoTemelKisiBilgiService kPSKimlikNoTemelKisiBilgiService, IIntegrationLogService logService)
        {
            _kPSKimlikNoTemelKisiBilgiService = kPSKimlikNoTemelKisiBilgiService;
            _logService = logService;
        }

        public async Task<ResultModel<GetirKisiAdSoyadTcDenQueryResponseModel>> Handle(GetirKisiAdSoyadTcDenQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirKisiAdSoyadTcDenQueryResponseModel>();

            try
            {
                var kpsKutukSonuc = await _logService.WrapServiceAsnyc<GetirKisiAdSoyadTcDenQuery, ServiceResponse<TemelKisiBilgileriSonucu>>(request, async () =>
                {
                    return await _kPSKimlikNoTemelKisiBilgiService.TemelKisiBilgiSorgulaAsync(new KPSKimlikNoTemelKisiBilgiService.TemelKisiBilgileriSorgulaSorguKriteri[]
                    {
                    new KPSKimlikNoTemelKisiBilgiService.TemelKisiBilgileriSorgulaSorguKriteri
                    {
                        KimlikNo = request.TcKimlikNo
                    }
                    });
                });

                if (!kpsKutukSonuc.IsSuccess)
                {
                    result.ErrorMessage("Kimlik bilgileri sorgulanamadı!");
                    return result;
                }

                if (kpsKutukSonuc.Data.HataBilgisi != null)
                {
                    result.ErrorMessage($"Kimlik bilgileri sorgulanırken hata oluştu: {kpsKutukSonuc.Data.HataBilgisi.Aciklama}");
                    return result;
                }

                var sorguSonucu = kpsKutukSonuc.Data.SorguSonucu.FirstOrDefault();

                if (sorguSonucu?.TCVatandasiKisiKutukleri?.KisiBilgisi?.TemelBilgisi == null || sorguSonucu?.MaviKartliKisiKutukleri?.KisiBilgisi?.TemelBilgisi == null)
                {
                    result.ErrorMessage($"Girilen veriye ait kişi bulunamadı!");
                    return result;
                }

                result.Result = new GetirKisiAdSoyadTcDenQueryResponseModel();

                if (sorguSonucu.TCVatandasiKisiKutukleri.KisiBilgisi.TemelBilgisi?.Ad != null)
                {
                    result.Result.Ad = sorguSonucu.TCVatandasiKisiKutukleri.KisiBilgisi.TemelBilgisi.Ad;
                    result.Result.Soyad = sorguSonucu.TCVatandasiKisiKutukleri.KisiBilgisi.TemelBilgisi.Soyad;

                    if (sorguSonucu.TCVatandasiKisiKutukleri.KisiBilgisi.DurumBilgisi?.OlumTarih?.Yil != null)
                    {
                        result.Result.OlumTarih = new DateOnly(
                            year: sorguSonucu.TCVatandasiKisiKutukleri.KisiBilgisi.DurumBilgisi.OlumTarih.Yil.Value,
                            month: sorguSonucu.TCVatandasiKisiKutukleri.KisiBilgisi.DurumBilgisi.OlumTarih.Ay.Value,
                            day: sorguSonucu.TCVatandasiKisiKutukleri.KisiBilgisi.DurumBilgisi.OlumTarih.Gun.Value
                        );
                    }
                }
                else
                {
                    //mavi kartlı kişi
                    result.Result.Ad = sorguSonucu.MaviKartliKisiKutukleri.KisiBilgisi.TemelBilgisi.Ad;
                    result.Result.Soyad = sorguSonucu.MaviKartliKisiKutukleri.KisiBilgisi.TemelBilgisi.Soyad;

                    if (sorguSonucu.MaviKartliKisiKutukleri.KisiBilgisi.DurumBilgisi?.OlumTarih?.Yil != null)
                    {
                        result.Result.OlumTarih = new DateOnly(
                            year: sorguSonucu.MaviKartliKisiKutukleri.KisiBilgisi.DurumBilgisi.OlumTarih.Yil.Value,
                            month: sorguSonucu.MaviKartliKisiKutukleri.KisiBilgisi.DurumBilgisi.OlumTarih.Ay.Value,
                            day: sorguSonucu.MaviKartliKisiKutukleri.KisiBilgisi.DurumBilgisi.OlumTarih.Gun.Value
                        );
                    }
                }

                if (request.MaskelemeKapaliMi != true)
                {
                    result.Result.Ad = StringAddon.ToMaskedWord(result.Result.Ad);
                    result.Result.Soyad = StringAddon.ToMaskedWord(result.Result.Soyad);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return result;
        }
    }
}