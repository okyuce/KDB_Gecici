using AutoMapper;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.Kds;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Queries.GetirBasvuruListeByTcNoAfadIcin;

public class GetirBasvuruListeByTcNoAfadIcinQuery : IRequest<ResultModel<List<GetirAfadIcinBasvuruDetayDto>>>, ICacheMediatrQuery
{
    public string? TcKimlikNo { get; set; }

    #region Cache Ayar
    public bool? CacheCustomUser => null;
    public int? CacheMinute => 15;
    public bool CacheIsActive => true;
    #endregion

    public class GetAllAppealByIdentificationNumberHandler : IRequestHandler<GetirBasvuruListeByTcNoAfadIcinQuery, ResultModel<List<GetirAfadIcinBasvuruDetayDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IKdsYerindedonusumBinabazliOranRepository _kdsYerindedonusumBinabazliOranRepository;
        private readonly IKdsYerindedonusumRezervAlanlarRepository _kdsYerindedonusumRezervAlanlarRepository;

        public GetAllAppealByIdentificationNumberHandler(IMapper mapper, IBasvuruRepository basvuruRepository, IKdsYerindedonusumBinabazliOranRepository kdsYerindedonusumBinabazliOranRepository, IKdsYerindedonusumRezervAlanlarRepository kdsYerindedonusumRezervAlanlarRepository)
        {
            _mapper = mapper;
            _basvuruRepository = basvuruRepository;
            _kdsYerindedonusumBinabazliOranRepository = kdsYerindedonusumBinabazliOranRepository;
            _kdsYerindedonusumRezervAlanlarRepository = kdsYerindedonusumRezervAlanlarRepository;
        }

        public async Task<ResultModel<List<GetirAfadIcinBasvuruDetayDto>>> Handle(GetirBasvuruListeByTcNoAfadIcinQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<List<GetirAfadIcinBasvuruDetayDto>>();

            try
            {
                var basvuruListesi = _basvuruRepository
                    .GetWhere(x =>
                        x.TcKimlikNo == request.TcKimlikNo.Trim()
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
                    )
                .OrderBy(o => o.BasvuruId)
                .ToList();

                if (basvuruListesi.Any())
                {
                    result.Result = _mapper.Map<List<GetirAfadIcinBasvuruDetayDto>>(basvuruListesi);

                    #region Uzlaşma Oranı Kontrolleri
                    var kdsBasvuruOranListe = _kdsYerindedonusumBinabazliOranRepository
                                                .GetWhere(x =>
                                                    x.Oran != null
                                                    &&
                                                    result.Result.Select(s => s.HasarTespitAskiKodu).Contains(x.HasarTespitAskiKodu),
                                                    true
                                                )
                                                .ToList();

                    foreach (var kdsBasvuruOran in kdsBasvuruOranListe)
                    {
                        var eslesenBasvuruListe = result.Result.Where(x => x.HasarTespitAskiKodu == kdsBasvuruOran.HasarTespitAskiKodu && x.UavtMahalleNo == kdsBasvuruOran.UavtMahalleNo).ToList();
                        foreach (var eslesenBasvuru in eslesenBasvuruListe)
                        {
                            eslesenBasvuru.UzlasmaOrani = kdsBasvuruOran.Oran;
                        }
                    }
                    #endregion

                    #region Rezerv Alan Kontrolleri
                    var kdsAskiRezervAlanListe = _kdsYerindedonusumRezervAlanlarRepository
                                                    .GetWhere(x =>
                                                        result.Result.Select(s => s.HasarTespitAskiKodu).Contains(x.HasarTespitAskiKodu),
                                                        true
                                                    )
                                                    .ToList();

                    foreach (var kdsAskiRezervAlan in kdsAskiRezervAlanListe)
                    {
                        var eslesenBasvuruListe = result.Result.Where(x => x.HasarTespitAskiKodu == kdsAskiRezervAlan.HasarTespitAskiKodu && x.UavtMahalleNo == kdsAskiRezervAlan.MahalleId).ToList();
                        foreach (var eslesenBasvuru in eslesenBasvuruListe)
                        {
                            eslesenBasvuru.RezervAlanMi = true;
                        }
                    }
                    #endregion
                }
                else
                    result.ErrorMessage("Başvuru bulunamadı.");
            }
            catch (Exception ex)
            {
                result.Exception(ex, "Başvuru Listesi Alınırken Bir Hata Meydana Geldi. Lütfen Daha Sonra Tekrar Deneyiniz.");
            }

            return await Task.FromResult(result);
        }
    }
}