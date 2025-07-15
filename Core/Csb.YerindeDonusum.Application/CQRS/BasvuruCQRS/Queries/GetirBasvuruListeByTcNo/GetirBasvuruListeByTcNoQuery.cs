using AutoMapper;
using Csb.YerindeDonusum.Application.CQRS.AydinlatmaMetniCQRS.Queries;
using Csb.YerindeDonusum.Application.Enums;
using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Application.Models;
using MediatR;

namespace Csb.YerindeDonusum.Application.CQRS.BasvuruCQRS.Queries.GetirBasvuruListeByTcNo;

public class GetirBasvuruListeByTcNoQuery : IRequest<ResultModel<GetirBasvuruListeByTcNoResponseModel>>
{
    public GetirBasvuruListeByTcNoQueryModel? Model { get; set; }

    public class GetAllAppealByIdentificationNumberHandler : IRequestHandler<GetirBasvuruListeByTcNoQuery, ResultModel<GetirBasvuruListeByTcNoResponseModel>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IBasvuruRepository _appealRepository;

        public GetAllAppealByIdentificationNumberHandler(IMediator mediator, IMapper mapper, IBasvuruRepository appealRepository)
        {
            _mediator = mediator;
            _mapper = mapper;
            _appealRepository = appealRepository;
        }

        public async Task<ResultModel<GetirBasvuruListeByTcNoResponseModel>> Handle(GetirBasvuruListeByTcNoQuery request, CancellationToken cancellationToken)
        {
            var result = new ResultModel<GetirBasvuruListeByTcNoResponseModel>();   
            try
            {
                if (request?.Model?.TuzelMi == true && string.IsNullOrWhiteSpace(request?.Model?.TuzelKisiMersisNo))
                {
                    result.Exception(new ArgumentNullException("Mersis No Bilgisi Boş Olamaz."), "Mersis No Bilgisi Boş Olamaz.");
                    return await Task.FromResult(result);
                }
                var ayar = await _mediator.Send(new GetirAyarQuery());

                if (ayar.IsError)
                {
                    result.Exception(new ArgumentNullException("Ayarlar Okunamadığı için Başvuru Listesi Alınamadı."), "Başvuru Listesi Alınamadı.");

                    return await Task.FromResult(result);
                }

                var queryBasvuruListesi = _appealRepository
                    .GetWhere(x => x.AktifMi == true && x.SilindiMi == false,
                        true,
                            i => i.BasvuruDurum,
                            i => i.BasvuruTur,
                            i => i.BasvuruDestekTur,
                            i => i.BasvuruKanal
                );

                if (request.Model?.TuzelMi == true)
                    queryBasvuruListesi = queryBasvuruListesi.Where(x =>
                        x.TuzelKisiTipId != null
                        &&
                        x.TuzelKisiMersisNo == request.Model.TuzelKisiMersisNo.Trim()
                    );
                else
                    queryBasvuruListesi = queryBasvuruListesi.Where(x =>
                        x.TcKimlikNo == request.Model.TcKimlikNo.Trim()
                        &&
                        x.TuzelKisiTipId == null
                    );

                var basvuruListesi = queryBasvuruListesi
                    .OrderByDescending(o => o.BasvuruId)
                    .ToList();

                if (basvuruListesi?.Any() == true)
                {
                    bool ticarethaneHibeBasvurabilir = false;
                    bool evHibeBasvurabilir = false;
                    bool krediBasvurabilir = false;

                    #region ...: Hibe Kontrolü :...

                    var queryHibeBasvuru = _appealRepository.GetWhere(x =>
                            (x.BasvuruDestekTurId == BasvuruDestekTurEnum.Hibe || x.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi)
                            &&
                            !(x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruIptalEdildi || x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir)
                            &&
                            x.SilindiMi == false
                        , true);


                    if (request.Model?.TuzelMi == true)
                        queryHibeBasvuru = queryHibeBasvuru.Where(x =>
                            x.TuzelKisiTipId != null
                            &&
                            x.TuzelKisiMersisNo == request.Model.TuzelKisiMersisNo.Trim()
                        );
                    else
                        queryHibeBasvuru = queryHibeBasvuru.Where(x =>
                            x.TcKimlikNo == request.Model.TcKimlikNo.Trim()
                            &&
                            x.TuzelKisiTipId == null
                        );

                    int olusturulanTicarethaneHibeSayisi = queryHibeBasvuru.Count(x => x.BasvuruTurId == BasvuruTurEnum.Ticarethane);
                    int olusturulanEvHibeSayisi = queryHibeBasvuru.Count(x => x.BasvuruTurId == BasvuruTurEnum.Konut || x.BasvuruTurId == BasvuruTurEnum.AhirliKonut);

                    ticarethaneHibeBasvurabilir = olusturulanTicarethaneHibeSayisi < ayar.Result.Basvuru.EnFazlaTicarethaneHibeSayisi;

                    evHibeBasvurabilir = olusturulanEvHibeSayisi < ayar.Result.Basvuru.EnFazlaEvHibeSayisi;

                    #endregion

                    #region ...: Kredi Kontrolü :...

                    var queryKrediBasvuru = _appealRepository.GetWhere(x =>
                            (x.BasvuruDestekTurId == BasvuruDestekTurEnum.Kredi || x.BasvuruDestekTurId == BasvuruDestekTurEnum.HibeVeKredi)
                            &&
                            !(x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvuruIptalEdildi || x.BasvuruDurumId == (long)BasvuruDurumEnum.BasvurunuzIptalEdilmistir)
                            &&
                            x.SilindiMi == false
                        , true);

                    if (request.Model?.TuzelMi == true)
                        queryKrediBasvuru = queryKrediBasvuru.Where(x =>
                            x.TuzelKisiTipId != null
                            &&
                            x.TuzelKisiMersisNo == request.Model.TuzelKisiMersisNo.Trim()
                        );
                    else
                        queryKrediBasvuru = queryKrediBasvuru.Where(x =>
                            x.TcKimlikNo == request.Model.TcKimlikNo.Trim()
                            &&
                            x.TuzelKisiTipId == null
                        );


                    int olusturulanKrediSayisi = queryKrediBasvuru.Count();

                    krediBasvurabilir = olusturulanKrediSayisi < ayar.Result.Basvuru.EnFazlaKrediSayisi;

                    #endregion

                    result.Result = new GetirBasvuruListeByTcNoResponseModel()
                    {
                        YeniBasvuruEklenebilirMi = false,
                        /* 01 Temmuz 2025 İtibariyle E-Devlet 'ten Yeni Başvurular "false" çekilmiştir. Sadece Saha'dan başvuru alınabilir. */ 
                        //YeniBasvuruEklenebilirMi = ticarethaneHibeBasvurabilir || evHibeBasvurabilir || krediBasvurabilir,
                        BasvuruListe = _mapper.Map<List<GetirBasvuruListeByTcNoDetayModel>>(basvuruListesi)
                    };

                    #region ...: Bilgilendirme Mesajı :...

                    if (result.Result.YeniBasvuruEklenebilirMi)
                    {
                        var mesajListOlusturulan = new List<string>();

                        if (!evHibeBasvurabilir)
                            mesajListOlusturulan.Add($"Konut için {olusturulanEvHibeSayisi} adet hibe");

                        if (!ticarethaneHibeBasvurabilir)
                            mesajListOlusturulan.Add($"{(mesajListOlusturulan.Any() ? "iş yeri" : "İş yeri")} için {olusturulanTicarethaneHibeSayisi} adet hibe");

                        if (!krediBasvurabilir)
                            mesajListOlusturulan.Add($"{olusturulanKrediSayisi} adet kredi");

                        if (mesajListOlusturulan.Any())
                        {
                            result.Result.BilgilendirmeMesaji = $"{string.Join(", ", mesajListOlusturulan)} başvurusu oluşturdunuz. Toplamda konut için {ayar.Result.Basvuru.EnFazlaEvHibeSayisi} adet hibe, iş yeri için {ayar.Result.Basvuru.EnFazlaTicarethaneHibeSayisi} adet hibe ve {ayar.Result.Basvuru.EnFazlaKrediSayisi} adet kredi başvurusunda bulunabilirsiniz.";
                        }
                        else
                        {
                            result.Result.BilgilendirmeMesaji = $"Konut için {ayar.Result.Basvuru.EnFazlaEvHibeSayisi} adet hibe, iş yeri için {ayar.Result.Basvuru.EnFazlaTicarethaneHibeSayisi} adet hibe başvurusunda bulunabilirsiniz. Toplamda {ayar.Result.Basvuru.EnFazlaKrediSayisi} adet kredi başvurusunda bulunabilirsiniz.";
                        }
                    }
                    else
                    {
                        result.Result.BilgilendirmeMesaji = $"Deprem Bölgesinde Yerinde Dönüşüm Hibe/Kredi Ön Başvurusu Kabul Edilmemektedir.";
                        //result.Result.BilgilendirmeMesaji = $"Konut için {ayar.Result.Basvuru.EnFazlaEvHibeSayisi} adet hibe, iş yeri için {ayar.Result.Basvuru.EnFazlaTicarethaneHibeSayisi} adet hibe başvurusunda bulunabilirsiniz. Toplamda {ayar.Result.Basvuru.EnFazlaKrediSayisi} adet kredi başvurusunda bulunabilirsiniz.";
                    }

                    #endregion
                }
                else
                {
                    result.Result = new GetirBasvuruListeByTcNoResponseModel()
                    {
                        /* 01 Temmuz 2025 İtibariyle E-Devlet 'ten Yeni Başvurular "false" çekilmiştir. Sadece Saha'dan başvuru alınabilir. */
                        YeniBasvuruEklenebilirMi = false
                    };

                    result.Result.BilgilendirmeMesaji = $"Deprem Bölgesinde Yerinde Dönüşüm Hibe/Kredi Ön Başvurusu Kabul Edilmemektedir.";
                    //result.Result.BilgilendirmeMesaji = $"Konut için {ayar.Result.Basvuru.EnFazlaEvHibeSayisi} adet hibe, iş yeri için {ayar.Result.Basvuru.EnFazlaTicarethaneHibeSayisi} adet hibe başvurusunda bulunabilirsiniz. Toplamda {ayar.Result.Basvuru.EnFazlaKrediSayisi} adet kredi başvurusunda bulunabilirsiniz.";
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