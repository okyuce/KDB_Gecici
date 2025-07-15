using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.AfadCQRS.Commands.GuncelleBasvuruAfadDurum;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Extensions;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Interfaces.InfrastructureInterfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Application.CQRS.BinaDegerlendirmeCQRS.Queries.GetirListeMalikler;

public class GetirListeMaliklerQuery : IRequest<ResultModel<List<GetirListeMaliklerQueryResponseModel>>>
{
    public long? BinaDegerlendirmeId { get; set; }
    public int? UavtMahalleNo { get; set; }
    public string? HasarTespitAskiKodu { get; set; }
    public string? TapuAda { get; set; }
    public string? TapuParsel { get; set; }
    public bool? BagimsizBolumlerAlinmasin { get; set; }
    public bool? SadeceImzaVerenlerAlinsin { get; set; }
    public bool? IptalEdilenlerAlinmasin { get; set; }

    // sorgular:
    public string? TcKimlikNo { get; set; }
    public string? Ad { get; set; }
    public string? Eposta { get; set; }
    public string? CepTelefonu { get; set; }
    public string? TuzelKisiVergiNo { get; set; }
    public string? TuzelKisiMersisNo { get; set; }
    public long? BasvuruAfadDurumId { get; set; }
    public long? BinaDegerlendirmeDurumId { get; set; }
    public string? BinaDisKapiNo { get; set; }

    public class GetirListeMaliklerQueryHandler : IRequestHandler<GetirListeMaliklerQuery, ResultModel<List<GetirListeMaliklerQueryResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IBasvuruRepository _basvuruRepository;
        private readonly IBasvuruKamuUstlenecekRepository _basvuruKamuUstlenecekRepository;
        private readonly ITakbisService _takbisService;
        private readonly IMediator _mediator;
        private readonly IKullaniciBilgi _kullaniciBilgi;

        public GetirListeMaliklerQueryHandler(IKullaniciBilgi kullaniciBilgi, IMapper mapper, IBasvuruRepository basvuruRepository, IBasvuruKamuUstlenecekRepository basvuruKamuUstlenecekRepository, ITakbisService takbisService, IMediator mediator)
        {
            _kullaniciBilgi = kullaniciBilgi;
            _mapper = mapper;
            _basvuruRepository = basvuruRepository;
            _basvuruKamuUstlenecekRepository = basvuruKamuUstlenecekRepository;
            _takbisService = takbisService;
            _mediator = mediator;
        }

        public async Task<ResultModel<List<GetirListeMaliklerQueryResponseModel>>> Handle(GetirListeMaliklerQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<List<GetirListeMaliklerQueryResponseModel>>();

            request.HasarTespitAskiKodu = HasarTespitAddon.AskiKoduToUpper(request.HasarTespitAskiKodu);

            var kullaniciBilgi = _kullaniciBilgi.GetUserInfo();
            int birimIlId = kullaniciBilgi.BirimIlId;

            try
            {
                if (request?.BagimsizBolumlerAlinmasin != true)
                {
                    await _mediator.Send(new GuncelleTabloBasvuruKamuUstlenecekJob
                    {
                        JobMuCalisiyor = false,
                        UavtMahalleNo = request?.UavtMahalleNo,
                        HasarTespitAskiKodu = request?.HasarTespitAskiKodu,
                        TapuAda = request?.TapuAda,
                        TapuParsel = request?.TapuParsel,
                    });
                }
                
                var basvuruQuery = _basvuruRepository.GetAllQueryable().Include(i => i.BasvuruDurum)
                                                    .Include(i => i.BasvuruDosyas)
                                                    .Include(i => i.BasvuruTur)
                                                    .Include(i => i.BasvuruDestekTur)
                                                    .Include(i => i.BasvuruKanal)
                                                    .Include(i => i.BasvuruAfadDurum)
                                                    .Include(i => i.BinaDegerlendirme).ThenInclude(i => i.BinaDegerlendirmeDurum)
                                                    .Include(i => i.BasvuruImzaVerens).ThenInclude(i=>i.BasvuruImzaVerenDosyas)
                                                    .Where(x => x.SilindiMi == false && x.AktifMi == true
                                                        && x.UavtMahalleNo == request.UavtMahalleNo
                                                        && x.HasarTespitAskiKodu == request.HasarTespitAskiKodu
                                                        && (string.IsNullOrWhiteSpace(x.SonuclandirmaAciklamasi)
                                                                || !x.SonuclandirmaAciklamasi.Contains("kamu_ustlenecek_tablosuna_aktarildi")))


                                                    
                                                ;

                var basvuruKamuUstlenecekQuery = _basvuruKamuUstlenecekRepository.GetAllQueryable().Include(i => i.BasvuruDurum)
                                .Include(i => i.BasvuruTur)
                                .Include(i => i.BasvuruDestekTur)
                                .Include(i => i.BasvuruAfadDurum)
                                .Include(i => i.BinaDegerlendirme).ThenInclude(i => i.BinaDegerlendirmeDurum)
                                .Include(i => i.BasvuruImzaVerens).ThenInclude(i => i.BasvuruImzaVerenDosyas)
                                .Where(x => x.SilindiMi == false && x.AktifMi == true
                                                            && x.UavtMahalleNo == request.UavtMahalleNo);                               
                             

                if (FluentValidationExtension.NotEmpty(request.BinaDegerlendirmeId))
                {
                    basvuruQuery = basvuruQuery.Where(x => x.BinaDegerlendirmeId == request.BinaDegerlendirmeId);
                    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => x.BinaDegerlendirmeId == request.BinaDegerlendirmeId);
                }
                if (FluentValidationExtension.NotEmpty(birimIlId))
                {
                    basvuruQuery = basvuruQuery.Where(x => x.UavtIlNo == birimIlId);
                    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => x.UavtIlNo == birimIlId);
                }
                //if (FluentValidationExtension.NotEmpty(request?.TapuAda))
                //{
                //    basvuruQuery = basvuruQuery.Where(x => x.TapuAda == request.TapuAda);
                //    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => x.TapuAda == request.TapuAda);
                //}
                //if (FluentValidationExtension.NotEmpty(request?.TapuParsel))
                //{
                //    basvuruQuery = basvuruQuery.Where(x => x.TapuParsel == request.TapuParsel);
                //    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => x.TapuParsel == request.TapuParsel);
                //}
                if (FluentValidationExtension.NotEmpty(request?.TcKimlikNo))
                {
                    basvuruQuery = basvuruQuery.Where(x => x.TcKimlikNo == request.TcKimlikNo);
                    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => x.TcKimlikNo == request.TcKimlikNo);
                }
                if (FluentValidationExtension.NotEmpty(request?.Ad))
                {
                    basvuruQuery = basvuruQuery.Where(x => x.Ad == request.Ad);
                    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => x.Ad == request.Ad);
                }
                if (FluentValidationExtension.NotEmpty(request?.Eposta))
                {
                    basvuruQuery = basvuruQuery.Where(x => x.Eposta == request.Eposta);
                    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => x.Eposta == request.Eposta);
                }
                if (FluentValidationExtension.NotEmpty(request?.CepTelefonu))
                {
                    basvuruQuery = basvuruQuery.Where(x => x.CepTelefonu == request.CepTelefonu);
                    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => x.CepTelefonu == request.CepTelefonu);
                }
                if (FluentValidationExtension.NotEmpty(request?.TuzelKisiVergiNo))
                {
                    basvuruQuery = basvuruQuery.Where(x => x.TuzelKisiVergiNo == request.TuzelKisiVergiNo);
                    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => x.TuzelKisiVergiNo == request.TuzelKisiVergiNo);
                }
                if (FluentValidationExtension.NotEmpty(request?.TuzelKisiMersisNo))
                {
                    basvuruQuery = basvuruQuery.Where(x => x.TuzelKisiMersisNo == request.TuzelKisiMersisNo);
                    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => x.TuzelKisiMersisNo == request.TuzelKisiMersisNo);
                }
                if (FluentValidationExtension.NotEmpty(request?.BasvuruAfadDurumId))
                {
                    basvuruQuery = basvuruQuery.Where(x => x.BasvuruAfadDurumId == request.BasvuruAfadDurumId);
                    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => x.BasvuruAfadDurumId == request.BasvuruAfadDurumId);
                }
                if (FluentValidationExtension.NotEmpty(request?.BinaDegerlendirmeDurumId))
                {
                    basvuruQuery = basvuruQuery.Where(x => x.BinaDegerlendirme.BinaDegerlendirmeDurumId == request.BinaDegerlendirmeDurumId);
                    basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => x.BinaDegerlendirme.BinaDegerlendirmeDurumId == request.BinaDegerlendirmeDurumId);
                }
                if (FluentValidationExtension.NotEmpty(request?.BinaDisKapiNo))
                {
                    if (request.BinaDisKapiNo == "belirtilmemis")
                    {
                        basvuruQuery = basvuruQuery.Where(x => string.IsNullOrWhiteSpace(x.BinaDegerlendirme.BinaDisKapiNo));
                        basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => string.IsNullOrWhiteSpace(x.BinaDegerlendirme.BinaDisKapiNo));
                    }
                    else
                    {
                        basvuruQuery = basvuruQuery.Where(x => x.BinaDegerlendirme.BinaDisKapiNo == request.BinaDisKapiNo.Trim());
                        basvuruKamuUstlenecekQuery = basvuruKamuUstlenecekQuery.Where(x => x.BinaDegerlendirme.BinaDisKapiNo == request.BinaDisKapiNo.Trim());
                    }
                }

                if (request?.IptalEdilenlerAlinmasin == true)
                {
                    basvuruQuery = basvuruQuery.Where(x => x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                          && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                    );
                }
                result.Result = _mapper.Map<List<GetirListeMaliklerQueryResponseModel>>(basvuruQuery.ToList());


                #region İptal Başvuruya Ait Aktif Başvuru Yoksa İptal Başvuruları Listeye Gelsin
                var basvuruIptalDurumIdListe = new List<int?> { (int)BasvuruDurumEnum.BasvurunuzIptalEdilmistir, (int)BasvuruDurumEnum.BasvuruIptalEdildi };
                var fazlalikIptanOlanBasvuruIdList = new List<long?>();

                foreach (var iptalBasvuru in result.Result.Where(x => basvuruIptalDurumIdListe.Contains(x.BasvuruDurumId) && x.TapuTasinmazId != null))
                {
                    //iptal olan başvuruyla aynı tapuyu seçen aynı kişiye ait aktif başvuru varsa iptal başvurusu listede olmamalı
                    if (result.Result.Any(x => x.TcKimlikNo == iptalBasvuru.TcKimlikNo && x.TapuTasinmazId == iptalBasvuru.TapuTasinmazId && x.TuzelKisiAdi == iptalBasvuru.TuzelKisiAdi && !basvuruIptalDurumIdListe.Contains(x.BasvuruDurumId)))
                        fazlalikIptanOlanBasvuruIdList.Add(iptalBasvuru.BasvuruId);
                }

                if (fazlalikIptanOlanBasvuruIdList.Any())
                    result.Result = result.Result.Where(x => !fazlalikIptanOlanBasvuruIdList.Contains(x.BasvuruId)).ToList();
                #endregion
                var kamuUstlenecekListeMapped = _mapper.Map<List<GetirListeMaliklerQueryResponseModel>>(basvuruKamuUstlenecekQuery.ToList());

                result.Result.AddRange(kamuUstlenecekListeMapped.Where(x => !result.Result.Any(y => y.TcKimlikNoRaw == x.TcKimlikNoRaw && y.TapuTasinmazId == x.TapuTasinmazId && y.TuzelKisiVergiNo == x.TuzelKisiVergiNo) 
                                                            && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                            && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir).ToList());
                
                if (request?.SadeceImzaVerenlerAlinsin == true)
                {
                    result.Result = result.Result.Where(x => (x.HibeOdemeTutar > 0 || x.KrediOdemeTutar > 0)
                                                            && x.BasvuruAfadDurumId != (long)BasvuruAfadDurumEnum.Kabul
                                                            && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                            && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                    ).ToList();
                }
                if (request?.IptalEdilenlerAlinmasin == true)
                {
                    result.Result = result.Result.Where(x => x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvuruIptalEdildi
                                                          && x.BasvuruDurumId != (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir
                                                    ).ToList();
                }
                result.Result = result.Result.OrderByDescending(x => x.BinaDisKapiNo).ThenBy(x=>x.BinaDegerlendirmeId).ThenByDescending(x=>x.OlusturmaTarihi).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }

            return await Task.FromResult(result);
        }
    }
}