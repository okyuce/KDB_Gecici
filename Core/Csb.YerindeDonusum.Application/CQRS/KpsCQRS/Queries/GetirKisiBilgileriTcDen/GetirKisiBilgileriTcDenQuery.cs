using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using CSB.Core.Entities.Responses;
using CSB.Core.Integration.KPS.Services;
using CSB.Core.LogHandler.Abstraction;
using KPSKutukService;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.KpsCQRS.Queries.GetirKisiBilgileriTcDen;

public class GetirKisiBilgileriTcDenQuery : IRequest<ResultModel<GetirKisiBilgileriTcDenQueryResponseModel>>, ICacheMediatrQuery
{
    public long? TcKimlikNo { get; set; }
    public DateTime? DogumTarih { get; set; }
    public bool? MaskelemeKapaliMi { get; set; }

    #region Cache Ayar
    public bool? CacheCustomUser => null;
    public int? CacheMinute => null;
    public bool CacheIsActive => true;
    #endregion

    public class GetirKisiBilgileriTcDenQueryHandler : IRequestHandler<GetirKisiBilgileriTcDenQuery, ResultModel<GetirKisiBilgileriTcDenQueryResponseModel>>
    {
        private readonly IKPSKutukService _kpsKutukServis;
        private readonly IIntegrationLogService _logService;

        public GetirKisiBilgileriTcDenQueryHandler(IKPSKutukService kpsKutukServis, IIntegrationLogService logService)
        {
            _kpsKutukServis = kpsKutukServis;
            _logService = logService;
        }

        public async Task<ResultModel<GetirKisiBilgileriTcDenQueryResponseModel>> Handle(GetirKisiBilgileriTcDenQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirKisiBilgileriTcDenQueryResponseModel>();

            var kpsKutukSonuc = await _logService.WrapServiceAsnyc<GetirKisiBilgileriTcDenQuery, ServiceResponse<BilesikKutukBilgileriSonucu>>(request, async () =>
            {
                return await _kpsKutukServis.KutukSorgulaAsync(new List<BilesikKutukSorgulaKimlikNoSorguKriteri> {
                    new BilesikKutukSorgulaKimlikNoSorguKriteri {
                        KimlikNo = request.TcKimlikNo,
                        DogumGun = request.DogumTarih.Value.Day,
                        DogumAy = request.DogumTarih.Value.Month,
                        DogumYil = request.DogumTarih.Value.Year
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

            result.Result = new GetirKisiBilgileriTcDenQueryResponseModel();

            if (sorguSonucu.TCVatandasiKisiKutukleri.KisiBilgisi.TemelBilgisi?.Ad != null)
            {
                result.Result.Ad = sorguSonucu.TCVatandasiKisiKutukleri.KisiBilgisi.TemelBilgisi.Ad;
                result.Result.Soyad = sorguSonucu.TCVatandasiKisiKutukleri.KisiBilgisi.TemelBilgisi.Soyad;
                result.Result.DogumTarih = new DateOnly(
                    year: sorguSonucu.TCVatandasiKisiKutukleri.KisiBilgisi.TemelBilgisi.DogumTarih.Yil.Value,
                    month: sorguSonucu.TCVatandasiKisiKutukleri.KisiBilgisi.TemelBilgisi.DogumTarih.Ay.Value,
                    day: sorguSonucu.TCVatandasiKisiKutukleri.KisiBilgisi.TemelBilgisi.DogumTarih.Gun.Value
                );

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
                result.Result.DogumTarih = new DateOnly(
                    year: sorguSonucu.MaviKartliKisiKutukleri.KisiBilgisi.TemelBilgisi.DogumTarih.Yil.Value,
                    month: sorguSonucu.MaviKartliKisiKutukleri.KisiBilgisi.TemelBilgisi.DogumTarih.Ay.Value,
                    day: sorguSonucu.MaviKartliKisiKutukleri.KisiBilgisi.TemelBilgisi.DogumTarih.Gun.Value
                );

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

            return result;
        }
    }
}