using AutoMapper;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirBasvuruListeDegisenAfadIcin;

public class GetirBasvuruListeDegisenAfadIcinQuery : IRequest<ResultModel<GetirBasvuruListeDegisenAfadIcinResponseModel>>
{
    public DateTime BaslangicTarihi { get; set; }
    public DateTime BitisTarihi { get; set; }
    public int Offset { get; set; }
    public long? IlId { get; set; }
    public long? IlceId { get; set; }
    public long? MahalleId { get; set; }

    public class GetirBasvuruListeDegisenAfadIcinQueryHandler : IRequestHandler<GetirBasvuruListeDegisenAfadIcinQuery, ResultModel<GetirBasvuruListeDegisenAfadIcinResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IKdsYerindedonusumBinabazliOranRepository _kdsYerindedonusumBinabazliOranRepository;
        private readonly IKdsYerindedonusumRezervAlanlarRepository _kdsYerindedonusumRezervAlanlarRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICacheService _cacheService;

        public GetirBasvuruListeDegisenAfadIcinQueryHandler(IMapper mapper, IWebHostEnvironment webHostEnvironment, ICacheService cacheService, IBasvuruRepository basvuruRepository, IKdsYerindedonusumBinabazliOranRepository kdsYerindedonusumBinabazliOranRepository, IKdsYerindedonusumRezervAlanlarRepository kdsYerindedonusumRezervAlanlarRepository)
        {
            _mapper = mapper;
            _basvuruRepository = basvuruRepository;
            _webHostEnvironment = webHostEnvironment;
            _cacheService = cacheService;
            _kdsYerindedonusumBinabazliOranRepository = kdsYerindedonusumBinabazliOranRepository;
            _kdsYerindedonusumRezervAlanlarRepository = kdsYerindedonusumRezervAlanlarRepository;
        }

        public async Task<ResultModel<GetirBasvuruListeDegisenAfadIcinResponseModel>> Handle(GetirBasvuruListeDegisenAfadIcinQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirBasvuruListeDegisenAfadIcinResponseModel>();

            var cacheKey = $"{_webHostEnvironment.EnvironmentName}_{nameof(GetirBasvuruListeDegisenAfadIcinQuery)}_{request.BaslangicTarihi.ToString("yyyy-MM-dd HH:mm:ss")}_{request.BitisTarihi.ToString("yyyy-MM-dd HH:mm:ss")}_{request.Offset}_{request.IlId}_{request.IlceId}_{request.MahalleId}";
            var redisCache = await _cacheService.GetValueAsync(cacheKey);
            if (redisCache != null)
            {
                result.Result = JsonConvert.DeserializeObject<GetirBasvuruListeDegisenAfadIcinResponseModel>(redisCache);
                return await Task.FromResult(result);
            }

            try
            {
                var query = _basvuruRepository
                    .GetWhere(x =>
                        (
                            (x.GuncellemeTarihi == null && x.OlusturmaTarihi >= request.BaslangicTarihi && x.OlusturmaTarihi <= request.BitisTarihi)
                            ||
                            (x.GuncellemeTarihi != null && x.GuncellemeTarihi >= request.BaslangicTarihi && x.GuncellemeTarihi <= request.BitisTarihi)
                        )
                        &&
                        x.AktifMi == true
                        &&
                        x.SilindiMi == false,
                            true,
                                i => i.BasvuruDurum,
                                i => i.BasvuruKanal,
                                i => i.BasvuruTur,
                                i => i.BinaDegerlendirme,
                                i => i.BinaDegerlendirme.BinaDegerlendirmeDurum,
                                i => i.BinaDegerlendirme.BinaYapiRuhsatIzinDosyas,
                                i => i.BasvuruImzaVerens
                    );

                if (FluentValidationExtension.NotEmpty(request.IlId))
                    query = query.Where(x => x.UavtIlNo == request.IlId);

                if (FluentValidationExtension.NotEmpty(request.IlceId))
                    query = query.Where(x => x.UavtIlceNo == request.IlceId);

                if (FluentValidationExtension.NotEmpty(request.MahalleId))
                    query = query.Where(x => x.UavtMahalleNo == request.MahalleId);

                var toplamBasvuruSayisi = await query.CountAsync();

                if (toplamBasvuruSayisi < request.Offset)
                    result.ErrorMessage($"Talep Edilen Offset Sayısı Kadar Başvuru Bulunmamaktadır!");
                else
                {
                    result.Result = new GetirBasvuruListeDegisenAfadIcinResponseModel
                    {
                        ToplamBasvuruSayisi = toplamBasvuruSayisi,
                        BasvuruListe = _mapper.Map<List<GetirAfadIcinBasvuruDetayDto>>(query.OrderBy(o => o.BasvuruId).Skip(request.Offset).Take(1000).ToList())
                    };

                    if (result.Result.BasvuruListe.Any())
                    {
                        #region Uzlaşma Oranı Kontrolleri
                        var kdsBasvuruOranListe = _kdsYerindedonusumBinabazliOranRepository
                                                    .GetWhere(x =>
                                                        x.Oran != null
                                                        &&
                                                        result.Result.BasvuruListe.Select(s => s.HasarTespitAskiKodu).Contains(x.HasarTespitAskiKodu),
                                                        true
                                                    )
                                                    .ToList();

                        foreach (var kdsBasvuruOran in kdsBasvuruOranListe)
                        {
                            var eslesenBasvuruListe = result.Result.BasvuruListe.Where(x => x.HasarTespitAskiKodu == kdsBasvuruOran.HasarTespitAskiKodu && x.UavtMahalleNo == kdsBasvuruOran.UavtMahalleNo).ToList();
                            foreach (var eslesenBasvuru in eslesenBasvuruListe)
                            {
                                eslesenBasvuru.UzlasmaOrani = kdsBasvuruOran.Oran;
                            }
                        }
                        #endregion

                        #region Rezerv Alan Kontrolleri
                        var kdsAskiRezervAlanListe = _kdsYerindedonusumRezervAlanlarRepository
                                                        .GetWhere(x =>
                                                            result.Result.BasvuruListe.Select(s => s.HasarTespitAskiKodu).Contains(x.HasarTespitAskiKodu),
                                                            true
                                                        )
                                                        .ToList();

                        foreach (var kdsAskiRezervAlan in kdsAskiRezervAlanListe)
                        {
                            var eslesenBasvuruListe = result.Result.BasvuruListe.Where(x => x.HasarTespitAskiKodu == kdsAskiRezervAlan.HasarTespitAskiKodu && x.UavtMahalleNo == kdsAskiRezervAlan.MahalleId).ToList();
                            foreach (var eslesenBasvuru in eslesenBasvuruListe)
                            {
                                eslesenBasvuru.RezervAlanMi = true;
                            }
                        }
                        #endregion
                    }

                    await _cacheService.SetValueAsync(cacheKey, JsonConvert.SerializeObject(result.Result), TimeSpan.FromMinutes(15));
                }
            }
            catch (Exception ex)
            {
                result.Exception(ex, "Başvuru Listesi Alınırken Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}